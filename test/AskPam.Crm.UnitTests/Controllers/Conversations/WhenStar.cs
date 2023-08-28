using AskPam.Crm.Authorization;
using AskPam.Crm.Conversations;
using AskPam.Crm.EntityFramework;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AskPam.Crm.UnitTests.Controllers.Conversations
{
    public class WhenStar : ConversationsControllerTests
    {
        public WhenStar()
        {
            MockCrmSession = new MockCrmSession { UserId = TestHelper.User1Id, OrganizationId = new Guid(TestHelper.Orgnization1Id) };
            Context = new CrmDbContext(ContextOptions, MockCrmSession);
            CurrentUser = TestHelper.Users.Find(u => u.Id == TestHelper.User1Id);
        }
        [Fact(DisplayName = "WhenStar_ShouldStar")]
        public async Task ShouldStar()
        {
            var controller = CreateConversationsController();
            var orgId = new Guid(TestHelper.Orgnization1Id);
            var conversation = new Conversation(orgId, "1234");

            MockConversationManager.Setup(d => d.FindByIdAsync(1, orgId)).ReturnsAsync(conversation);
            MockUserManager.Setup(u => u.FindByIdAsync(TestHelper.User1Id)).ReturnsAsync(CurrentUser);

            await controller.Star(1, false);
            MockStarsManager.Verify(d => d.Star(CurrentUser, conversation, null), Times.AtLeastOnce());
        }

        [Fact(DisplayName = "WhenStar_ShouldUnStar")]
        public async Task ShouldUnStar()
        {
            var orgId = new Guid(TestHelper.Orgnization1Id);
            var controller = CreateConversationsController();
            var conversation = new Conversation(orgId, "1234");

            MockConversationManager.Setup(d => d.FindByIdAsync(1, orgId)).ReturnsAsync(conversation);
            MockUserManager.Setup(u => u.FindByIdAsync(TestHelper.User1Id)).ReturnsAsync(CurrentUser);

            await controller.Star(1, true);
            MockStarsManager.Verify(d => d.UnStar(CurrentUser, conversation, null), Times.AtLeastOnce());
        }
    }
}
