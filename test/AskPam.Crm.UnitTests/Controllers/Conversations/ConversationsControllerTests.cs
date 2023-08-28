using AskPam.Crm.Authorization;
using AskPam.Crm.Contacts;
using AskPam.Crm.Conversations;
using AskPam.Crm.EntityFramework;
using AskPam.Crm.Followers;
using AskPam.Crm.Organizations;
using AskPam.Crm.Stars;
using AskPam.Domain.Repositories;
using AskPam.EntityFramework.Repositories;
using AskPam.Events;
using Moq;

namespace AskPam.Crm.UnitTests.Controllers.Conversations
{
    public class ConversationsControllerTests : BaseControllerTest
    {
        protected MockRepository MockRepository;

        protected Mock<IConversationsManager> MockConversationManager;
        protected IRepository<Contact> MockContactRepository;
        protected Mock<IFollowersManager> MockFollowersManager;
        protected Mock<IUserManager> MockUserManager;
        protected Mock<IDomainEvents> MockDomainEvents;
        protected Mock<IOrganizationManager> MockOrganizationManager;
        protected IRepository<Conversation> MockRepositoryConversation;
        protected Mock<IStarsManager> MockStarsManager;
        protected IRepository<Channel> MockRepositoryChannel;

        protected IRepository<Message> MockMessageRepository;
        protected IUnitOfWork UnitOfWork;

        protected ConversationsController CreateConversationsController()
        {
            MockRepository = new MockRepository(MockBehavior.Loose);

            MockConversationManager = MockRepository.Create<IConversationsManager>();
            MockContactRepository = new EfCoreRepositoryBase<Contact>(Context);
            MockFollowersManager = MockRepository.Create<IFollowersManager>();
            MockUserManager = MockRepository.Create<IUserManager>();
            MockDomainEvents = MockRepository.Create<IDomainEvents>();
            MockOrganizationManager = MockRepository.Create<IOrganizationManager>();
            MockRepositoryConversation = new EfCoreRepositoryBase<Conversation>(Context);
            MockStarsManager = MockRepository.Create<IStarsManager>();
            MockMessageRepository = new EfCoreRepositoryBase<Message>(Context);
            UnitOfWork = new UnitOfWork<CrmDbContext>(Context);


            return new ConversationsController(
                MockCrmSession,
                MockMapper,
                MockConversationManager.Object,
                MockContactRepository,
                MockFollowersManager.Object,
                MockUserManager.Object,
                MockDomainEvents.Object,
                MockOrganizationManager.Object,
                MockRepositoryConversation,
                MockStarsManager.Object,
                MockMessageRepository,
                UnitOfWork);
        }

    }
}
