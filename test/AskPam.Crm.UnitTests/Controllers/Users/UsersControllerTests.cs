using AskPam.Crm.Authorization;
using AskPam.Crm.Users;
using AskPam.EntityFramework.Repositories;
using Moq;

namespace AskPam.Crm.UnitTests.Controllers.Users
{
    public class UsersControllerTests : BaseControllerTest
    {
        protected MockRepository MockRepository;

        protected Mock<IUserManager> MockUserManager;
      
        protected IUnitOfWork UnitOfWork;

        protected UsersController CreateConversationsController()
        {
            MockRepository = new MockRepository(MockBehavior.Loose);

            MockUserManager = MockRepository.Create<IUserManager>();

            return new UsersController(
                MockCrmSession,
                MockMapper,
                MockUserManager.Object);
        }

    }
}
