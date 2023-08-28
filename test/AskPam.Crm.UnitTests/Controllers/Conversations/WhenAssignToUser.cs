using AskPam.Crm.Authorization;
using AskPam.Crm.Conversations;
using AskPam.Crm.EntityFramework;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AskPam.Crm.UnitTests.Controllers.Conversations
{
    public class WhenAssignToUser : ConversationsControllerTests
    {
        public WhenAssignToUser()
        {
            MockCrmSession = new MockCrmSession { UserId = TestHelper.User1Id, OrganizationId = new Guid(TestHelper.Orgnization1Id) };
            Context = new CrmDbContext(ContextOptions, MockCrmSession);
        }
        [Fact(DisplayName = "WhenAssignToUser_ShouldAddAssignment")]
        public async Task ShouldAssign()
        {
            var controller = CreateConversationsController();
            var orgId = new Guid(TestHelper.Orgnization1Id);
            var conversation = new Conversation(orgId, "conv","1234");
            var assigner = TestHelper.Users.Find(u => u.Id == TestHelper.User1Id);
            var assignee = TestHelper.Users.Find(u => u.Id == TestHelper.User2Id);


            MockConversationManager.Setup(d => d.FindByIdAsync(1, orgId)).ReturnsAsync(conversation);
            MockUserManager.Setup(u => u.FindByIdAsync(TestHelper.User1Id)).ReturnsAsync(assigner);
            MockUserManager.Setup(u => u.FindByIdAsync(TestHelper.User2Id)).ReturnsAsync(assignee);



            await controller.AssignToUser(1, TestHelper.User2Id);
            MockConversationManager.Verify(d => d.AssigntoUser(conversation, assignee, assigner), Times.AtLeastOnce());

        }

        [Fact(DisplayName = "WhenAssignToUser_ShouldRemoveAssignment")]
        public async Task ShouldRemoveAssign()
        {

            var controller = CreateConversationsController();
            var orgId = new Guid(TestHelper.Orgnization1Id);

            var conversation = new Conversation(orgId, "1234");
            MockConversationManager.Setup(d => d.FindByIdAsync(1, orgId)).ReturnsAsync(conversation);

            await controller.AssignToUser(1, null);
            MockConversationManager.Verify(d => d.RemoveUserAssignment(conversation), Times.AtLeastOnce());
        }
    }
}
