using System;
using System.Collections.Generic;
using System.Text;
using AskPam.Crm.Conversations;
using AskPam.Crm.EntityFramework;
using AskPam.Crm.Organizations;
using AskPam.Crm.Runtime.Session;
using AskPam.Crm.WebHooks;
using AskPam.EntityFramework.Repositories;
using Moq;

namespace AskPam.Crm.UnitTests.Controllers.Webhooks.Postmark
{
    public class BasePostMarkController : BaseControllerTest
    {

        protected MockRepository MockRepository;
        protected readonly PostMarkController _controller;
        protected readonly Mock<IConversationsManager> _mockConversationManager;
        public BasePostMarkController()
        {
            Context = new CrmDbContext(ContextOptions, new NullCrmSession());

            MockRepository = new MockRepository(MockBehavior.Loose);
            _mockConversationManager = MockRepository.Create<IConversationsManager>();
            var organizationRepository = new EfCoreRepositoryBase<Organization, Guid>(Context);
            var conversationsRepository = new EfCoreRepositoryBase<Conversation>(Context);
            var messageRepository = new EfCoreRepositoryBase<Message>(Context);
            var deliveryRepository = new EfCoreRepositoryBase<DeliveryStatus, long>(Context);
            var unitOfWork = new UnitOfWork<CrmDbContext>(Context);


            _controller = new PostMarkController(organizationRepository,
                conversationsRepository,
                messageRepository,
                _mockConversationManager.Object,
                deliveryRepository,
                unitOfWork
            );
        }
    }
}
