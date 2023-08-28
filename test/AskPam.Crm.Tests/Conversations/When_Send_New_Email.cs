using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AskPam.Application.Dto;
using AskPam.Crm.Authorization.Dtos;
using AskPam.Crm.Controllers.Conversations;
using AskPam.Crm.Controllers.Conversations.Dtos;
using AskPam.Crm.Conversations.Dtos;
using AskPam.Crm.IntegratedTests.TestDatas;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace AskPam.Crm.IntegratedTests.ConversationsController
{
    [Collection(Consts.TEST_COLLECTION)]
    public class When_Send_New_Email
    {
        readonly TestServerFixture _testServerFixture;

        public When_Send_New_Email(TestServerFixture testServerFixture)
        {
            _testServerFixture = testServerFixture;
            _testServerFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _testServerFixture.user1IdToken);
            _testServerFixture.Client.DefaultRequestHeaders.Add("Organization", _testServerFixture.user1OrganizationId.ToString());
        }


        //Todo : write test case
        //[Fact]
        // public async Task Should_SendNewEmail()
        // {
        //     _testServerFixture.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _idToken);
        //     _testServerFixture.client.DefaultRequestHeaders.Add("Organization", _organizationId.ToString());
        //     var requestData = new EmailDto()
        //     {
        //         To = "John Doe <test@test.com>,Peter Jackson <test2@test.com>",
        //         Subject = "Subject",
        //         Text = "Text"
        //     };
        //     var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
        //     var response = await _testServerFixture.client.PostAsync("api/conversations/sendNewEmail", content);
        //     Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        // }

       
    }
}
