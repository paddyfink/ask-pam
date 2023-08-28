using AskPam.Crm.Conversations.Events;
using AskPam.Crm.EntityFramework;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AskPam.Crm.UnitTests.Managers.Conversations
{
    public class WhenArchive : BaseConversationManagerTests
    {
        public WhenArchive()
        {
            MockCrmSession = new MockCrmSession { UserId = TestHelper.User1Id, OrganizationId = new Guid(TestHelper.Orgnization1Id) };
            Context = new CrmDbContext(ContextOptions, MockCrmSession);
        }


        [Fact(DisplayName = "WhenArchive_ShouldArchive")]
        public async Task ShouldArchive()
        {
            // Arrange
            TestHelper.GenerateConversationsdataTest(ContextOptions, new Guid(TestHelper.Orgnization1Id), 1, 1, isArchived: false);
            var user = TestHelper.Users.First();
            var conversation = await Context.Conversations.FindAsync((long)1);
            var conversationManager = CreateManager();

            // Act
            await conversationManager.Archive(conversation, user);

            // Assert
            (await Context.Conversations.FindAsync((long)1)).IsActive.ShouldBe(false);
            MockDomainEvents.Verify(d => d.RaiseAsync(It.Is<ConversationArchivedEvent>(m => m.Conversation.Id == 1 && m.Conversation.IsActive==false)), Times.AtLeastOnce());
        }

        [Fact(DisplayName = "WhenArchive_ShouldUnArchive()")]
        public async Task ShouldUnArchive()
        {
            // Arrange
            TestHelper.GenerateConversationsdataTest(ContextOptions, new Guid(TestHelper.Orgnization1Id), 1, 1, isArchived: true);
            var conversation = await Context.Conversations.FindAsync((long)1);
            var conversationManager = CreateManager();
            var user = TestHelper.Users.First();

            // Act
            await conversationManager.Archive(conversation, user);

            // Assert
            (await Context.Conversations.FindAsync((long)1)).IsActive.ShouldBe(true);
            MockDomainEvents.Verify(d => d.RaiseAsync(It.Is<ConversationUnarchivedEvent>(m => m.Conversation.Id == 1 && m.Conversation.IsActive==true)), Times.AtLeastOnce());
        }

    }
}
