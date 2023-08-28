using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AskPam.Application.Dto;
using AskPam.Crm.Contacts.Dtos;
using AskPam.Crm.Conversations.Dtos;
using AskPam.Crm.IntegratedTests.TestDatas;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shouldly;
using Xunit;

namespace AskPam.Crm.IntegratedTests.ConversationsController
{
    [Collection(Consts.TEST_COLLECTION)]
    public class When_Star_A_Conversation
    {
        readonly TestServerFixture _testServerFixture;
        
        public When_Star_A_Conversation(TestServerFixture testServerFixture)
        {
            _testServerFixture = testServerFixture;
            _testServerFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _testServerFixture.user1IdToken);
            _testServerFixture.Client.DefaultRequestHeaders.Add("Organization", _testServerFixture.user1OrganizationId.ToString());
        }

      

        [Fact]
        public async Task Should_Return_Ok()
        {
            var content = new StringContent(JsonConvert.SerializeObject(true), Encoding.UTF8, "application/json");
            var response = await _testServerFixture.Client.PostAsync("api/conversations/1/flag", content);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_Return_BadRequest()
        {
            var content = new StringContent(JsonConvert.SerializeObject("text"), Encoding.UTF8, "application/json");
            var response = await _testServerFixture.Client.PostAsync("api/conversations/1/flag", content);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }


    }
}
