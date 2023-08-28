using System;
using System.Threading.Tasks;
using AskPam.Crm.Conversations;
using AskPam.Crm.Conversations.Dtos;
using AskPam.Crm.Organizations;
using AskPam.Crm.UnitTests.TestData;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace AskPam.Crm.UnitTests.Controllers.Webhooks.Postmark
{
    public class WhenIncomeMessage : BasePostMarkController
    {
       


        [Fact(DisplayName = "WhenIncomeMessage_ShouldCreateNewClient")]
        public async Task ShouldProcessNewMessage()
        {
            await _controller.Income(new Guid(TestHelper.Orgnization1Id), JsonConvert.DeserializeObject<IncomeMail>(MessageContent.IncomeMessageJson));

            var conversation = await Context.Conversations.FirstAsync();

            conversation.Email.ShouldBe("john.doe@email.com");
            conversation.Channels.Count.ShouldBe(1);
            _mockConversationManager.Verify(d => d.ProcessNewMessage(It.IsAny<Message>(),
                It.Is<Conversation>(c => c.Email == "john.doe@email.com" && c.Name == "John Doe"),
                It.IsAny<Organization>()), Times.AtLeastOnce());
        }

        [Fact(DisplayName = "WhenIncomeMessage_ShouldProcessNewMessageForExistingConversation")]
        public async Task ShouldProcessNewMessageForExistingConversation()
        {
            var json = MessageContent.SmoochJson.Replace("{0}", TestHelper.Orgnization1Id);
            await Context.Conversations.AddAsync(new Conversation(new Guid(TestHelper.Orgnization1Id), "John Doe", email: "john.doe@email.com"));
            await Context.SaveChangesAsync();

            await _controller.Income(new Guid(TestHelper.Orgnization1Id), JsonConvert.DeserializeObject<IncomeMail>(MessageContent.IncomeMessageJson));

            var conversation = await Context.Conversations.FirstAsync();
            (await Context.Conversations.CountAsync()).ShouldBe(1);
            conversation.Email.ShouldBe("john.doe@email.com");
            _mockConversationManager.Verify(d => d.ProcessNewMessage(It.IsAny<Message>(),
                It.Is<Conversation>(c => c.Email == "john.doe@email.com"),
                It.IsAny<Organization>()), Times.AtLeastOnce());
        }

        [Fact(DisplayName = "WhenIncomeMessage_WhenSameEmailConversationInDifferentOrganization_ShouldProcessNewMessageForExistingConversation")]
        public async Task ShouldProcessNewMessageForExistingConversation2()
        {
            await Context.Conversations.AddAsync(new Conversation(new Guid(TestHelper.Orgnization1Id), "John Doe", email: "john.doe@email.com"));
            await Context.Conversations.AddAsync(new Conversation(new Guid(TestHelper.Orgnization2Id), "John Doe", email: "john.doe@email.com"));
            await Context.SaveChangesAsync();

            await _controller.Income(new Guid(TestHelper.Orgnization2Id), JsonConvert.DeserializeObject<IncomeMail>(MessageContent.IncomeMessageJson));
            
            _mockConversationManager.Verify(d => d.ProcessNewMessage(It.IsAny<Message>(),
                It.Is<Conversation>(c => c.Email == "john.doe@email.com" && c.OrganizationId== new Guid(TestHelper.Orgnization2Id)),
                It.Is<Organization>(c=> c.Id== new Guid(TestHelper.Orgnization2Id))), Times.AtLeastOnce());
        }


    }
}
