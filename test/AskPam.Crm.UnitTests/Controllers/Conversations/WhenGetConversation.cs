using AskPam.Crm.Conversations.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using System.Linq;
using AskPam.Crm.Conversations.Events;
using Moq;
using AskPam.Exceptions;
using AskPam.Crm.EntityFramework;

namespace AskPam.Crm.UnitTests.Controllers.Conversations
{
    public class WhenGetConversation : ConversationsControllerTests
    {
        public WhenGetConversation()
        {
            CreateTestData();
            MockCrmSession = new MockCrmSession { UserId = TestHelper.User1Id, OrganizationId = new Guid(TestHelper.Orgnization1Id) };
            Context = new CrmDbContext(ContextOptions, MockCrmSession);
        }

        [Fact(DisplayName = "WhenGetConversation_ShouldReturnConversationWithMessages")]
        public async Task ShouldReturnConversationWithMessages()
        {
            var controller = CreateConversationsController();
            var conversation = await Context.Conversations.FindAsync((long)1);
            conversation.MarkAsUnSeen();
            Context.Conversations.Update(conversation);

            ObjectResult result = (ObjectResult)(await controller.GetConversation(1));
            var conversationDto = (ConversationDto)(result.Value);

            conversationDto.ShouldNotBeNull();
            conversationDto.Messages.Count().ShouldBe(50);
             conversation = Context.Conversations.Find((long)1);
            conversation.Seen.ShouldBe(true);
            MockDomainEvents.Verify(d => d.RaiseAsync(It.IsAny<ConversationReadEvent>()), Times.AtLeastOnce());
        }

        [Fact(DisplayName = "WhenGetConversation_ShouldThrowBadRequest")]
        public async Task ShouldThrowBadRequest()
        {
            MockCrmSession = new MockCrmSession { UserId = "userId", OrganizationId = new Guid?() };       


            var controller = CreateConversationsController();
            var exception = await Record.ExceptionAsync(() => controller.GetConversation(1));
            Assert.IsType<ApiException>(exception);
            ((ApiException)exception).StatusCode.ShouldBe(System.Net.HttpStatusCode.BadRequest);
        }

        #region private 
        private void CreateTestData()
        {
            TestHelper.GenerateConversationsdataTest(ContextOptions, new Guid(TestHelper.Orgnization1Id));
        }

        #endregion
    }
}
