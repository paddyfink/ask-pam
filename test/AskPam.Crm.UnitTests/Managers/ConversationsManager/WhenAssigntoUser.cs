using AskPam.Crm.Conversations.Events;
using AskPam.Crm.EntityFramework;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AskPam.Crm.UnitTests.Managers.Conversations
{
    public class WhenAssigntoUser : BaseConversationManagerTests
    {
        public WhenAssigntoUser()
        {
            CreateTestData();
            MockCrmSession = new MockCrmSession { UserId = TestHelper.User1Id, OrganizationId = new Guid(TestHelper.Orgnization1Id) };
            Context = new CrmDbContext(ContextOptions, MockCrmSession);
        }

        [Fact(DisplayName ="ShouldAssignConversationToUser")]
        public async Task ShouldAssignConversationToUser()
        {
            // Arrange
            TestHelper.GenerateConversationsdataTest(ContextOptions, new Guid(TestHelper.Orgnization1Id), 1, 1);
            var conversation = await Context.Conversations.FindAsync((long)1);
            var assignee = await Context.Users.FindAsync(TestHelper.User2Id);
            var conversationManager = CreateManager();

            // Act
            await conversationManager.AssigntoUser(conversation, assignee);

            // Assert
            (await Context.Conversations.FindAsync((long)1)).AssignedToId.ShouldBe(TestHelper.User2Id);
            MockDomainEvents.Verify(d => d.RaiseAsync(It.Is<ConversationAssignedEvent>(m => m.Conversation == conversation
            && m.Assignee == assignee
            && m.Assigner == null)),
            Times.AtLeastOnce());


            // Act
            await conversationManager.RemoveUserAssignment(conversation);

            // Assert
            (await Context.Conversations.FindAsync((long)1)).AssignedToId.ShouldBeNull();
        }

        [Fact(DisplayName = "ShouldAssignConversationThenRemoveAssignment")]
        public async Task ShouldAssignConversationToUserWithAssigner()
        {
            // Arrange
            TestHelper.GenerateConversationsdataTest(ContextOptions, new Guid(TestHelper.Orgnization1Id), 1, 1, isArchived: false);
            var conversation = await Context.Conversations.FindAsync((long)1);
            var assignee = await Context.Users.FindAsync(TestHelper.User2Id);
            var assigner = await Context.Users.FindAsync(TestHelper.User1Id);
            var conversationManager = CreateManager();

            // Act
            await conversationManager.AssigntoUser(conversation, assignee, assigner);

            // Assert
            (await Context.Conversations.FindAsync((long)1)).AssignedToId.ShouldBe(TestHelper.User2Id);
            MockDomainEvents.Verify(d => d.RaiseAsync(It.Is<ConversationAssignedEvent>(m => m.Conversation == conversation
            && m.Assignee == assignee
            && m.Assigner == assigner)),
            Times.AtLeastOnce());
            
        }

        #region Private
        private void CreateTestData()
        {
            TestHelper.GenerateConversationsdataTest(ContextOptions, new Guid(TestHelper.Orgnization1Id), 1, 1, isArchived: false);
        }
        #endregion
    }

}
