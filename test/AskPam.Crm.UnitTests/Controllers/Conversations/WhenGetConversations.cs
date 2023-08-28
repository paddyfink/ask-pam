using AskPam.Crm.Controllers.Conversations.Dtos;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AskPam.Crm.Conversations.Dtos;
using AskPam.Application.Dto;
using AskPam.Crm.Controllers.Conversations;
using Shouldly;
using System.Linq;
using System;
using AskPam.Crm.Conversations;
using AskPam.Crm.EntityFramework;
using AutoMapper;
using AutoMapper.Configuration;

namespace AskPam.Crm.UnitTests.Controllers.Conversations
{
    public class WhenGetConversations : ConversationsControllerTests
    {

        public WhenGetConversations()
        {

            MockCrmSession = new MockCrmSession { UserId = TestHelper.User1Id, OrganizationId = new Guid(TestHelper.Orgnization1Id) };
            Context = new CrmDbContext(ContextOptions, MockCrmSession);

            // Since wer are using ProjectTo, we need to initialize with the static

        }

        [Fact(DisplayName = "WhenGetConversations_ShouldReturnList")]
        public async Task ShouldReturnList()
        {
            CreateTestData();
            var controller = CreateConversationsController();
            var request = new ConversationListRequestDto { Filter = ConversationFilter.All, MaxResultCount = 20, SkipCount = 0 };
            ObjectResult result = (ObjectResult)(await controller.GetConversations(request));
            var pagegConversations = (PagedResultDto<ConversationListDto>)(result.Value);

            pagegConversations.Items.Count.ShouldBe(20);
            pagegConversations.TotalCount.ShouldBe(50);
            pagegConversations.HasNext.ShouldBe(true);

            Context.Database.EnsureDeleted();
        }

        [Fact(DisplayName = "WhenGetConversationsWithoutMessage_ShouldReturnList")]
        public async Task ShouldReturnListWithoutMessages()
        {
            TestHelper.GenerateConversationsdataTest(ContextOptions, new Guid(TestHelper.Orgnization1Id), 1, 0);
            var controller = CreateConversationsController();
            var request = new ConversationListRequestDto { Filter = ConversationFilter.All, MaxResultCount = 20, SkipCount = 0 };
            ObjectResult result = (ObjectResult)(await controller.GetConversations(request));
            var pagegConversations = (PagedResultDto<ConversationListDto>)(result.Value);

            pagegConversations.Items.Count.ShouldBe(1);
            pagegConversations.Items[0].LastMessage.ShouldBeNull();

            Context.Database.EnsureDeleted();
        }


        [Fact(DisplayName = "WhenGetConversations_ShouldNotIncludeConversationFromOtherOrganizations")]
        public async Task ShouldNotIncludeConversationFromOtherOrganizations()
        {
            CreateTestData();
            var controller = CreateConversationsController();
            var request = new ConversationListRequestDto { Filter = ConversationFilter.All, MaxResultCount = 100, SkipCount = 0 };
            ObjectResult result = (ObjectResult)(await controller.GetConversations(request));
            var pagegConversations = (PagedResultDto<ConversationListDto>)(result.Value);

            pagegConversations.Items.Any(c => c.OrganizationId != new Guid(TestHelper.Orgnization1Id)).ShouldBe(false);

            Context.Database.EnsureDeleted();
        }

        #region private 
        private void CreateTestData()
        {
            TestHelper.GenerateConversationsdataTest(ContextOptions, new Guid(TestHelper.Orgnization1Id));
            TestHelper.GenerateConversationsdataTest(ContextOptions, new Guid(TestHelper.Orgnization2Id));
        }


        #endregion
    }
}
