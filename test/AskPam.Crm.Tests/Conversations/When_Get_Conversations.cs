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
    public class When_Get_Conversations
    {
        readonly TestServerFixture _testServerFixture;
        
        public When_Get_Conversations(TestServerFixture testServerFixture)
        {
            _testServerFixture = testServerFixture;

            _testServerFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _testServerFixture.user1IdToken);
            _testServerFixture.Client.DefaultRequestHeaders.Add("Organization", _testServerFixture.user1OrganizationId.ToString());
        }

      

        [Fact]
        public async Task Should_Return_SortedList_By_Date()
        {
            var requestData = new ContactListRequestDto()
            {
                Sorting = "EmailAddress DESC",
                SkipCount = 0,
                MaxResultCount = 5
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            var response = await _testServerFixture.Client.PostAsync("api/conversations/GetConversations", content);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var list = JsonConvert.DeserializeObject<PagedResultDto<ConversationListDto>> (await response.Content.ReadAsStringAsync());
            list.TotalCount.ShouldBe(2);
        }

       
    }
}
