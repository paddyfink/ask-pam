using AskPam.Crm.Conversations;
using AskPam.Crm.Conversations.Events;
using AskPam.Crm.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AskPam.Crm.Contacts;
using Xunit;

namespace AskPam.Crm.UnitTests.Managers.Conversations
{
    public class WhenProcessNewMessage : BaseConversationManagerTests
    {
        public WhenProcessNewMessage()
        {
            CreateTestData();
            MockCrmSession = new MockCrmSession { UserId = TestHelper.User1Id, OrganizationId = new Guid(TestHelper.Orgnization1Id) };
            Context = new CrmDbContext(ContextOptions, MockCrmSession);
        }

        [Fact(DisplayName = "WhenProcessNewMessage_ShouldCreateNewChannelAndNewConversation")]
        public async Task ShouldCreateNewConversation()
        {
            var organizaton = TestHelper.Organizations.Find(o => o.Id == new Guid(TestHelper.Orgnization1Id));

            var conversationManager = CreateManager();
            var smoochUserId = (new Guid()).ToString();
           await Context.Conversations.AddAsync(new Conversation(new Guid(TestHelper.Orgnization1Id), "John Doe",
                smoochUserId));
           await Context.SaveChangesAsync();
            var conversation = await Context.Conversations.FirstAsync(c => c.SmoochUserId == smoochUserId);
            var message = new Message
            {

                Text = "this is message",
                Status = MessageStatus.Received,
                Date = DateTime.UtcNow,
                Author = "John Doe",
                MessageType = MessageType.Text,
                Seen = false
            };

            var msg = await conversationManager.ProcessNewMessage(message, conversation, organizaton);

            (await Context.Conversations.CountAsync()).ShouldBe(2);
            Context.Conversations.Find((long)2).Messages.Count().ShouldBe(1);

            AssertMessageReceivedEventTriggered(msg);
            //Todo: 
            //AssertConversationCreatedEventTriggered(msg);
        }

        //[Fact(DisplayName = "WhenProcessNewMessage_ShouldCreateNewConversationForExistingChannel")]
        //public async Task ShouldCreateNewConversationForExistingChannel()
        //{
        //    var organizaton = TestHelper.Organizations.Find(o => o.Id == new Guid(TestHelper.Orgnization1Id));

        //    var channel = new Channel
        //    {
        //        Type = ChannelType.Sms,
        //        Recipient = "+1514000000000",
        //        DisplayName = "+1514000000000",
        //        Active = true,
        //        OrganizationId = organizaton.Id,
        //        IntegrationId = 1,
        //    };
        //    await Context.Channels.AddAsync(channel);
        //    await Context.SaveChangesAsync();

        //    var conversationManager = CreateManager();
        //    var message = new Message
        //    {

        //        Text = "this is message",
        //        Status = MessageStatus.Received,
        //        Date = DateTime.UtcNow,
        //        Name = "John Doe",
        //        Channel = ChannelType.Sms,
        //        Type = MessageType.Text,
        //        Seen = false
        //    };


        //    var msg = await conversationManager.ProcessNewMessage(message, channel, organizaton);

        //    Context.Conversations.Count().ShouldBe(2);
        //    (await Context.Channels.CountAsync()).ShouldBe(2);
        //    Context.Conversations.Find(2).Messages.Count().ShouldBe(1);

        //    AssertMessageReceivedEventTriggered(msg);
        //    AssertConversationCreatedEventTriggered(msg);

        //}

        [Fact(DisplayName = "WhenProcessNewMessage_ShouldAddMessageToExistingConversation")]
        public async Task ShouldAddMessageToExistingConversation()
        {
            var organizaton = TestHelper.Organizations.Find(o => o.Id == new Guid(TestHelper.Orgnization1Id));
            var conversation = Context.Conversations.Find((long)1);

            var conversationManager = CreateManager();
            var message = new Message
            {
                Text = "this is message",
                Status = MessageStatus.Received,
                Date = DateTime.UtcNow,
                Author = "John Doe",
                ChannelType = ChannelType.Sms,
                MessageType = MessageType.Text,
                Seen = false
            };

            var msg = await conversationManager.ProcessNewMessage(message, conversation, organizaton);

            Context.Conversations.Count().ShouldBe(1);
            Context.Conversations.Find((long)1).Messages.Count().ShouldBe(1);

            AssertMessageReceivedEventTriggered(msg);
        }

        [Fact(DisplayName = "WhenProcessNewMessage_ShouldAttachConversationToExistingContact")]
        public async Task ShouldAttachConversationToExistingContact()
        {
            var organization = TestHelper.Organizations.Find(o => o.Id == new Guid(TestHelper.Orgnization1Id));
            await Context.Conversations.AddAsync(new Conversation
            (
                organization.Id,
                "James Doe",
                email:"test@email.com"
            ));

            await Context.Contacts.AddAsync(new Contact
            (
                "James",
                "Doe",
                organization.Id,
                "test@email.com"
            ));
            await Context.SaveChangesAsync();

            var conversation = Context.Conversations.First(c=>c.Email== "test@email.com");

            var conversationManager = CreateManager();
            var message = new Message
            {
                Text = "this is message",
                Status = MessageStatus.Received,
                Date = DateTime.UtcNow,
                Author = "John Doe",
                ChannelType = ChannelType.Sms,
                MessageType = MessageType.Text,
                Seen = false
            };

            await conversationManager.ProcessNewMessage(message, conversation, organization);
            
            Context.Conversations.First(c => c.Email == "test@email.com").ContactId.ShouldNotBeNull();
            
        }

        [Fact(DisplayName = "WhenProcessNewMessage_ShouldAttachConversationToExistingContact2")]
        public async Task ShouldAttachConversationToExistingContact2()
        {
            var organization = TestHelper.Organizations.Find(o => o.Id == new Guid(TestHelper.Orgnization1Id));
            await Context.Conversations.AddAsync(new Conversation
            (
                organization.Id,
                "James Doe",
                email: "test2@email.com"
            ));

            await Context.Contacts.AddAsync(new Contact
            (
                "James",
                "Doe",
                organization.Id,
                emailAddress2:"test2@email.com"
            ));
            await Context.SaveChangesAsync();

            var conversation = Context.Conversations.First(c => c.Email == "test2@email.com");

            var conversationManager = CreateManager();
            var message = new Message
            {
                Text = "this is message",
                Status = MessageStatus.Received,
                Date = DateTime.UtcNow,
                Author = "John Doe",
                ChannelType = ChannelType.Sms,
                MessageType = MessageType.Text,
                Seen = false
            };

            await conversationManager.ProcessNewMessage(message, conversation, organization);

            Context.Conversations.First(c => c.Email == "test2@email.com").ContactId.ShouldNotBeNull();

        }

        #region private
        private void AssertMessageReceivedEventTriggered(Message msg)
        {
            MockDomainEvents.Verify(d => d.RaiseAsync(It.Is<MessageReceivedEvent>(m => m.Message == msg
            && m.Conversation.Id == msg.ConversationId)), Times.AtLeastOnce());
        }

        private void AssertConversationCreatedEventTriggered(Message msg)
        {

            MockDomainEvents.Verify(d => d.RaiseAsync(It.Is<ConversationCreatedEvent>(c => c.Conversation.Id == msg.ConversationId)), Times.AtLeastOnce());
        }
        private void CreateTestData()
        {
            TestHelper.GenerateConversationsdataTest(ContextOptions, new Guid(TestHelper.Orgnization1Id), 1, 1);
        }
        #endregion
    }
}
