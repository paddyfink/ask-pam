using AskPam.Crm.Authorization;
using AskPam.Crm.Conversations;
using AskPam.Crm.EntityFramework;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AskPam.Crm.UnitTests.Controllers.Conversations
{
    public class WhenArchive : ConversationsControllerTests
    {
        [Fact(DisplayName = "WhenArchive_ShouldCallArchiveFomManager")]
        public async Task ShouldCallArchiveFomManager()
        {
            MockCrmSession = new MockCrmSession { UserId = TestHelper.User1Id, OrganizationId = new Guid(TestHelper.Orgnization1Id) };
            Context = new CrmDbContext(ContextOptions, MockCrmSession);
            var controller = CreateConversationsController();

            var orgId = new Guid(TestHelper.Orgnization1Id);
            var conversation = new Conversation(orgId,"conv", smoochUserId:"1234");


            MockConversationManager.Setup(d => d.FindByIdAsync(1, orgId)).ReturnsAsync(conversation);


            await controller.Archive(1);
            MockConversationManager.Verify(d => d.Archive(conversation, It.Is<User>(u=>u.Id==TestHelper.User1Id)), Times.AtLeastOnce());

        }
    }
}
