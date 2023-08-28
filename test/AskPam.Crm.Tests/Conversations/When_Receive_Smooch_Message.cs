//using System;
//using System.Net;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Text;
//using System.Threading.Tasks;
//using AskPam.Application.Dto;
//using AskPam.Crm.Authorization.Dtos;
//using AskPam.Crm.Controllers.Conversations;
//using AskPam.Crm.Controllers.Conversations.Dtos;
//using AskPam.Crm.Conversations.Dtos;
//using AskPam.Crm.IntegratedTests.Conversations;
//using AskPam.Crm.IntegratedTests.TestDatas;
//using Newtonsoft.Json;
//using Shouldly;
//using Xunit;

//namespace AskPam.Crm.IntegratedTests.ConversationsController
//{
//    [Collection(Consts.TEST_COLLECTION)]
//    public class When_Receive_Smooch_Message
//    {
//        readonly TestServerFixture _testServerFixture;

//        public When_Receive_Smooch_Message(TestServerFixture testServerFixture)
//        {
//            _testServerFixture = testServerFixture;
//            _testServerFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _testServerFixture.user1IdToken);
//            _testServerFixture.Client.DefaultRequestHeaders.Add("Organization", _testServerFixture.user1OrganizationId.ToString());
//        }
        

//        [Fact]
//        public async Task Should_Process_It()
//        {
//            //Test message reception
//            var data = MessageContent.smoochJson.Replace("{0}", TestOrganizationsBuilder.organization1SmoochAppID);
//            var content = new StringContent(data, Encoding.UTF8, "application/json");
//            var response = await _testServerFixture.Client.PostAsync("api/smooch/income", content);
//            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            

//            var requestConversationData = new ConversationListRequestDto { Filter = ConversationFilter.All, MaxResultCount = 10, SkipCount = 0, Search = "New Message" };
//            content = new StringContent(JsonConvert.SerializeObject(requestConversationData), Encoding.UTF8, "application/json");
//            response = await _testServerFixture.Client.PostAsync("api/conversations/GetConversationsList", content);
//            response.StatusCode.ShouldBe(HttpStatusCode.OK);
//            var conversationList = JsonConvert.DeserializeObject<PagedResultDto<ConversationListDto>>(response.Content.ReadAsStringAsync().Result);
//            conversationList.Items.Count.ShouldBe(1);
//        }
//    }
//}
