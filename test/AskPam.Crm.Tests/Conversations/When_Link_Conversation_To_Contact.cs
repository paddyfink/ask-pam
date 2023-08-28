using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AskPam.Application.Dto;
using AskPam.Crm.Contacts.Dtos;
using AskPam.Crm.Conversations.Dtos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shouldly;
using Xunit;

namespace AskPam.Crm.IntegratedTests.ConversationsController
{
    [Collection(Consts.TEST_COLLECTION)]
    public class When_Link_Conversation_To_Contact
    {
        readonly TestServerFixture _testServerFixture;
        
        public When_Link_Conversation_To_Contact(TestServerFixture testServerFixture)
        {
            _testServerFixture = testServerFixture;

            _testServerFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _testServerFixture.user1IdToken);
            _testServerFixture.Client.DefaultRequestHeaders.Add("Organization", _testServerFixture.user1OrganizationId.ToString());
        }

      

        [Fact]
        public async Task Should_Return_Ok()
        {
            var response = await _testServerFixture.Client.GetAsync("api/conversations/1");
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

        }

       
    }
}
