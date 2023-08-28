using AskPam.Crm.Conversations;
using AskPam.Crm.Conversations.Events;
using AskPam.Crm.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AskPam.Crm.UnitTests.Managers.Conversations
{
    public class WhenAddNote : BaseConversationManagerTests
    {
        public WhenAddNote()
        {

            CreateTestData();
            MockCrmSession = new MockCrmSession { UserId = TestHelper.User1Id, OrganizationId = new Guid(TestHelper.Orgnization1Id) };
            Context = new CrmDbContext(ContextOptions, MockCrmSession);
        }

        [Fact(DisplayName = "WhenAddNote_ShouldAddNoteToConversation")]
        public async Task ShouldAddNoteToConversation()
        {
            long conversationId = 1;
            var conversation = Context.Conversations.Find((long)1);
            var user = Context.Users.Find(TestHelper.User1Id);

            var conversationManager = CreateManager();
            var note = new Message
            {
                Text = "this is a note",
                Status = MessageStatus.Received,
                Date = DateTime.UtcNow,
                Author = "John Doe",
                MessageType = MessageType.Note,
                Seen = false
            };

            var msg = await conversationManager.AddNote(conversation, user, note);

            (await Context.Conversations.CountAsync()).ShouldBe(1);
            Context.Conversations.Find(conversationId).Messages.Count.ShouldBe(1);

            MockDomainEvents.Verify(d => d.RaiseAsync(It.Is<MessageSentEvent>(m => m.Message == msg
            && m.Conversation.Id == msg.ConversationId)), Times.AtLeastOnce());
        }

        #region private

        private void CreateTestData()
        {
            TestHelper.GenerateConversationsdataTest(ContextOptions, new Guid(TestHelper.Orgnization1Id), 1, 1);
        }
        #endregion
    }
}
