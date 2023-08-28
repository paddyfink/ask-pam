using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AskPam.Crm.Authorization.Dtos;
using AskPam.Crm.IntegratedTests.TestDatas;
using AskPam.Crm.InternalNotes.Dtos;
using Newtonsoft.Json;
using Xunit;

namespace AskPam.Crm.IntegratedTests.InternalNotes
{
    [Collection(Consts.TEST_COLLECTION)]
    public class InternalNote_Tests
    {
        TestServerFixture _testServerFixture;
        private string _idToken;
        private Guid _organizationId;

        public InternalNote_Tests(TestServerFixture testServerFixture)
        {
            _testServerFixture = testServerFixture;

            var requestData = new LoginDto { Email = TestUsersBuilder.user1Email, Password = TestUsersBuilder.userPassword };
            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            var response = (_testServerFixture.Client.PostAsync("api/account/login", content)).Result;
            var authInfoDto = JsonConvert.DeserializeObject<AuthInfoDto>(response.Content.ReadAsStringAsync().Result);
            _idToken = authInfoDto.IdToken;
            _organizationId = authInfoDto.OrganizationId.Value;
        }

        [Fact]
        public async Task Should_Create_InternalNote()
        {
            _testServerFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _idToken);
            _testServerFixture.Client.DefaultRequestHeaders.Add("Organization", _organizationId.ToString());
            var requestData = new NoteDto()
            {
                Comment = "Name Test",
                ContactId = 1
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            var response = await _testServerFixture.Client.PostAsync("api/Notes", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        //[Fact]
        //public async Task Should_Delete_InternalNote()
        //{
        //    _testServerFixture.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _idToken);
        //    _testServerFixture.client.DefaultRequestHeaders.Add("Organization", _organizationId.ToString());
        //    var requestData = 2;
        //    var response = await _testServerFixture.client.DeleteAsync($"api/InternalNotes/deleteInternalNote?id={requestData}");
        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //}

        [Fact]
        public async Task Should_Get_InternalNotes()
        {
            _testServerFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _idToken);
            _testServerFixture.Client.DefaultRequestHeaders.Add("Organization", _organizationId.ToString());
            var requestData = new GetNotesRequestDto { ContactId = 1 };
            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            var response = await _testServerFixture.Client.PostAsync("api/Notes/GetNotes", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
