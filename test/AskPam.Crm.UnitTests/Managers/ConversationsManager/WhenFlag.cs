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
   public  class WhenFlag : BaseConversationManagerTests
    {
        public WhenFlag()
        {
            MockCrmSession = new MockCrmSession { UserId = TestHelper.User1Id, OrganizationId = new Guid(TestHelper.Orgnization1Id) };
            Context = new CrmDbContext(ContextOptions, MockCrmSession);
        }

        [Fact(DisplayName = "WhenFlag_ShouldFlag")]
        public async Task ShouldFlag()
        {
            // Arrange
            TestHelper.GenerateConversationsdataTest(ContextOptions, new Guid(TestHelper.Orgnization1Id), 1, 1, isFlagged: false);
            var conversation = await Context.Conversations.FindAsync((long)1);
            var conversationManager = CreateManager();

            // Act
            await conversationManager.Flag(conversation);

            // Assert
            (await Context.Conversations.FindAsync((long)1)).IsFlagged.ShouldBe(true);
            MockDomainEvents.Verify(d => d.RaiseAsync(It.Is<ConversationFlaggedEvent>(m => m.Conversation.Id == 1 && m.Conversation.IsFlagged == true)), Times.AtLeastOnce());
        }

        [Fact(DisplayName = "WhenFlag_ShouldUnFlag()")]
        public async Task ShouldUnFlag()
        {
            // Arrange
            TestHelper.GenerateConversationsdataTest(ContextOptions, new Guid(TestHelper.Orgnization1Id), 1, 1, isFlagged: true);
            var conversation = await Context.Conversations.FindAsync((long)1);
            var conversationManager = CreateManager();

            // Act
            await conversationManager.Flag(conversation);

            // Assert
            (await Context.Conversations.FindAsync((long)1)).IsFlagged.ShouldBe(false);
            MockDomainEvents.Verify(d => d.RaiseAsync(It.Is<ConversationFlaggedEvent>(m => m.Conversation.Id == 1 && m.Conversation.IsFlagged == false)), Times.AtLeastOnce());
        }
    }
}
