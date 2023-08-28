using AskPam.Crm.Authorization;
using AskPam.Crm.Conversations;
using AskPam.Crm.Conversations.Dtos;
using AskPam.Crm.EntityFramework;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AskPam.Crm.Contacts;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AskPam.Crm.UnitTests.Controllers.Conversations
{
    public class WhenSendMessage : ConversationsControllerTests
    {
        public WhenSendMessage()
        {
            CreateTestData();
            MockCrmSession = new MockCrmSession { UserId = TestHelper.User1Id, OrganizationId = new Guid(TestHelper.Orgnization1Id) };
            Context = new CrmDbContext(ContextOptions, MockCrmSession);
        }

        [Fact(DisplayName = "WhenSendMessage_ShouldAddNoteToExistingConversation")]
        public async Task ShouldAddNote()
        {
            var orgId = new Guid(TestHelper.Orgnization1Id);
            var controller = CreateConversationsController();

            var conversation = new Conversation(orgId, "1234");
            var currentUser = TestHelper.Users.Find(u => u.Id == TestHelper.User1Id);

            MockUserManager.Setup(u => u.FindByIdAsync(TestHelper.User1Id)).ReturnsAsync(currentUser);


            var input = new SendMessageDto
            {
                Text = "this is the message text",
                Type = MessageType.Note.Value,
                ConversationId = 1
            };

            await controller.SendMessage(input);
            MockConversationManager.Verify(d => d.AddNote(
              It.Is<Conversation>(c => c.Id == 1),
              It.Is<User>(u => u.Id == TestHelper.User1Id),
              It.Is<Message>(m => m.MessageType == MessageType.Note
                  && m.Text == input.Text
                  && m.Status == MessageStatus.Sent)
              ), Times.AtLeastOnce());
        }

        [Fact(DisplayName = "WhenSendMessage_ShouldReplyToExistingConversation")]
        public async Task ShouldReplyToExistingConversation()
        {
            var orgId = new Guid(TestHelper.Orgnization1Id);
            var controller = CreateConversationsController();

            var currentUser = TestHelper.Users.Find(u => u.Id == TestHelper.User1Id);

            MockUserManager.Setup(u => u.FindByIdAsync(TestHelper.User1Id)).ReturnsAsync(currentUser);

            var input = new SendMessageDto
            {
                Text = "this is the message text",
                Type = MessageType.Text.Value,
                ConversationId = 1
            };

            await controller.SendMessage(input);
            MockConversationManager.Verify(d => d.SendMessage(
              It.Is<Conversation>(c => c.Id == 1),
              It.Is<Message>(m => m.MessageType == MessageType.Text
                  && m.Text == input.Text
                  && m.Status == MessageStatus.Sent),
              It.Is<User>(u => u.Id == TestHelper.User1Id)
              ), Times.AtLeastOnce());
        }

        [Fact(DisplayName = "WhenSendMessage_ShouldSendtoContactWithoutConversation")]
        public async Task ShouldSendtoContact()
        {
            TestHelper.GenerateContactDataTests(ContextOptions, new Guid(TestHelper.Orgnization1Id),1);
            var orgId = new Guid(TestHelper.Orgnization1Id);
            var contact = await Context.Contacts.FirstAsync();
            var controller = CreateConversationsController();

            var currentUser = TestHelper.Users.Find(u => u.Id == TestHelper.User1Id);

            MockUserManager.Setup(u => u.FindByIdAsync(TestHelper.User1Id)).ReturnsAsync(currentUser);

            var input = new SendMessageDto
            {
                Text = "this is the message text",
                Type = MessageType.Text.Value,
                Recipients = new List<RecipientDto>(new[] { new RecipientDto() { ChannelType = ChannelType.Email.Value, Name = "Full Name", ContactId = 1, Recipient = "contact@email.com" } })
            };

            await controller.SendMessage(input);
            MockConversationManager.Verify(d => d.SendMessage(
                It.Is<Conversation>(c => c.Name == "Full Name" && c.ContactId == 1),
                It.Is<Message>(m => m.MessageType == MessageType.Text
                                    && m.Text == input.Text
                                    && m.Status == MessageStatus.Sent),
                It.Is<User>(u => u.Id == TestHelper.User1Id)
            ), Times.AtLeastOnce());
        }

        #region private 
        private void CreateTestData()
        {
            TestHelper.GenerateConversationsdataTest(ContextOptions, new Guid(TestHelper.Orgnization1Id), 1, 5);
        }


        #endregion
    }
}
