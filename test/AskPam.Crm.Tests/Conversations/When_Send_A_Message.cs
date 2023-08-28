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
    public class When_Send_A_Message
    {
        readonly TestServerFixture _testServerFixture;
        
        public When_Send_A_Message(TestServerFixture testServerFixture)
        {
            _testServerFixture = testServerFixture;

            _testServerFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _testServerFixture.user1IdToken);
            _testServerFixture.Client.DefaultRequestHeaders.Add("Organization", _testServerFixture.user1OrganizationId.ToString());
        }

      
        //Todo
        //[Fact]
        //public async Task Should_Return_Ok()
        //{
        //    var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
        //    var response = await _testServerFixture.Client.PostAsync("api/conversations/1/Archive", content);
        //    response.StatusCode.ShouldBe(HttpStatusCode.OK);
        //}

       
    }
}
