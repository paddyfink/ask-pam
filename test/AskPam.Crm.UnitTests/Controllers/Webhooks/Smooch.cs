using System;
using System.Threading.Tasks;
using AskPam.Crm.Configuration;
using AskPam.Crm.Contacts;
using AskPam.Crm.Conversations;
using AskPam.Crm.EntityFramework;
using AskPam.Crm.Organizations;
using AskPam.Crm.Runtime.Session;
using AskPam.Crm.UnitTests.TestData;
using AskPam.Crm.WebHooks;
using AskPam.EntityFramework.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json.Linq;
using Shouldly;
using Xunit;

namespace AskPam.Crm.UnitTests.Controllers.Webhooks
{
    public class Smooch : BaseControllerTest
    {
        protected MockRepository MockRepository;
        private readonly SmoochController _controller;
        private readonly Mock<IConversationsManager> mockConversationManager;

        public Smooch()
        {
            Context = new CrmDbContext(ContextOptions, new NullCrmSession());

            MockRepository = new MockRepository(MockBehavior.Loose);
            mockConversationManager = MockRepository.Create<IConversationsManager>();
            var organizationRepository = new EfCoreRepositoryBase<Organization, Guid>(Context);
            var conversationsRepository = new EfCoreRepositoryBase<Conversation>(Context);
            var messageRepository = new EfCoreRepositoryBase<Message>(Context);
            var deliveryRepository = new EfCoreRepositoryBase<DeliveryStatus, long>(Context);
            var contactRepository = new EfCoreRepositoryBase<Contact>(Context);
            var settingRepository = new EfCoreRepositoryBase<Setting>(Context);
            var unitOfWork = new UnitOfWork<CrmDbContext>(Context);


            _controller = new SmoochController(organizationRepository,
                    conversationsRepository,
                    messageRepository,
                    contactRepository,
                    mockConversationManager.Object,
                    deliveryRepository,
                    unitOfWork,
                settingRepository
                   );
        }

        [Fact(DisplayName = "WhenReceiveNewMessage_ShouldCreateNewClient")]
        public async Task ShouldProcessNewMessage()
        {
            var json = MessageContent.SmoochJson.Replace("{0}", TestHelper.Orgnization1Id);
            var result = await _controller.Income(JObject.Parse(json));

            var conversation = await Context.Conversations.FirstAsync();

            conversation.SmoochUserId.ShouldBe("eb1ee9757cbefe1682d11001");
            conversation.Channels.Count.ShouldBe(3);
            mockConversationManager.Verify(d => d.ProcessNewMessage(It.IsAny<Message>(),
                It.Is<Conversation>(c => c.SmoochUserId == "eb1ee9757cbefe1682d11001" && c.Name == "John Doe"),
                It.IsAny<Organization>()), Times.AtLeastOnce());
        }

        [Fact(DisplayName = "WhenReceiveNewMessage_ShouldProcessMessageForExistingClient")]
        public async Task ShouldProcessNewMessageForExistingConversation()
        {
            var json = MessageContent.SmoochJson.Replace("{0}", TestHelper.Orgnization1Id);
            await Context.Conversations.AddAsync(new Conversation(new Guid(TestHelper.Orgnization1Id), "John Doe", "eb1ee9757cbefe1682d11001"));
            await Context.SaveChangesAsync();

            await _controller.Income(JObject.Parse(json));

            var conversation = await Context.Conversations.FirstAsync();
            (await Context.Conversations.CountAsync()).ShouldBe(1);
            conversation.SmoochUserId.ShouldBe("eb1ee9757cbefe1682d11001");
            conversation.Channels.Count.ShouldBe(3);
            mockConversationManager.Verify(d => d.ProcessNewMessage(It.IsAny<Message>(),
                It.Is<Conversation>(c => c.SmoochUserId == "eb1ee9757cbefe1682d11001"),
                It.IsAny<Organization>()), Times.AtLeastOnce());
        }
    }
}
