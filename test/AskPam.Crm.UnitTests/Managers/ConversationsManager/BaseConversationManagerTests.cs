using AskPam.Crm.AI;
using AskPam.Crm.Configuration;
using AskPam.Crm.Contacts;
using AskPam.Crm.Conversations;
using AskPam.Crm.EntityFramework;
using AskPam.Crm.Settings;
using AskPam.Crm.Storage;
using AskPam.EntityFramework.Repositories;
using AskPam.Events;
using Microsoft.Extensions.Options;
using Moq;
using System;
using AskPam.Crm.Organizations;
using AskPam.Domain.Repositories;
using Xunit;

namespace AskPam.Crm.UnitTests.Managers.Conversations
{
    public class BaseConversationManagerTests : BaseManagerTest
    {
        protected MockRepository MockRepository;

        protected Mock<IDomainEvents> MockDomainEvents;
        protected Mock<IStorageService> MockStorageService;
        protected Mock<IPostmarkService> MockPostmarkService;
        protected Mock<ISettingManager> MockSettingManager;
        protected Mock<IQnAMakerManager> MockQnAMakerManager;
        protected IRepository<Conversation> ConversationRepository;
        protected IRepository<Contact> ContactRepository;
        protected IRepository<Organization, Guid> OrganizationRepository;
        protected IRepository<Message> MessageRepository;
        protected Mock<ISmoochConversationService> MockSmoochService;

        public BaseConversationManagerTests()
        {
            MockRepository = new MockRepository(MockBehavior.Loose);

        }

        protected ConversationsManager CreateManager()
        {
            mockUnitOfWork = new UnitOfWork<CrmDbContext>(Context);
            MockDomainEvents = MockRepository.Create<IDomainEvents>();
            MockStorageService = MockRepository.Create<IStorageService>();
            MockPostmarkService = MockRepository.Create<IPostmarkService>();
            MockSettingManager = MockRepository.Create<ISettingManager>();
            MockQnAMakerManager = MockRepository.Create<IQnAMakerManager>();
            MockSmoochService = MockRepository.Create<ISmoochConversationService>();
            ConversationRepository = new EfCoreRepositoryBase<Conversation>(Context);
            ContactRepository = new EfCoreRepositoryBase<Contact>(Context);
            ConversationRepository = new EfCoreRepositoryBase<Conversation>(Context);
            MessageRepository = new EfCoreRepositoryBase<Message>(Context);
            OrganizationRepository = new EfCoreRepositoryBase<Organization, Guid>(Context);

            return new ConversationsManager(
                mockUnitOfWork,
                MockDomainEvents.Object,
                MockStorageService.Object,
                MockPostmarkService.Object,
                MockSettingManager.Object,
                MockQnAMakerManager.Object,
                MockSmoochService.Object,
                MessageRepository,
                ConversationRepository,
                ContactRepository,
                OrganizationRepository);
        }
    }
}
