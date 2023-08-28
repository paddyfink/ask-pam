using AskPam.Crm.Authorization;
using AskPam.Crm.Contacts;
using AskPam.Crm.Conversations;
using AskPam.Crm.Tags;
using AskPam.Domain.Repositories;
using AskPam.EntityFramework.Repositories;
using Moq;
using System;
using AskPam.Crm.Common;

namespace AskPam.Crm.UnitTests.Controllers.Contacts
{


    public class BaseContactsControllerTestsController : BaseControllerTest
    {

        protected IRepository<Contact> ContactRepository;
        protected IRepository<ContactGroup> ContactGroupRepository;
        protected Mock<IContactManager> MockContactManager;
        protected Mock<IConversationsManager> MockConversationManager;
        protected Mock<ITagsManager> MockTagsManager;
        protected Mock<IUserManager> MockUserManager;
        protected Mock<IPhoneNumberLookupService> MockPhoneLookupService;


        protected ContactsController CreateContactsController()
        {
            ContactRepository = new EfCoreRepositoryBase<Contact>(Context);
            ContactGroupRepository = new EfCoreRepositoryBase<ContactGroup>(Context);
            MockContactManager = new Mock<IContactManager>();
            MockConversationManager = new Mock<IConversationsManager>();
            MockTagsManager = new Mock<ITagsManager>();
            MockUserManager = new Mock<IUserManager>();
            MockPhoneLookupService = new Mock<IPhoneNumberLookupService>();

            return  new ContactsController(
                MockCrmSession,
                MockMapper,
                ContactRepository,
                MockContactManager.Object,
               MockConversationManager.Object,
               MockTagsManager.Object, 
               MockUserManager.Object,
               MockPhoneLookupService.Object
            );
        }

    }
}
