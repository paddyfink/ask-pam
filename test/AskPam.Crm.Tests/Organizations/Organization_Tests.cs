using System.Net.Http;
using System.Text;
using AskPam.Crm.Authorization.Dtos;
using Newtonsoft.Json;
using Xunit;

namespace AskPam.Crm.IntegratedTests.Organizations
{
    [Collection(Consts.TEST_COLLECTION)]
    public class Organization_Tests 
    {
        TestServerFixture _testServerFixture;

        private string _idToken;
        public Organization_Tests(TestServerFixture testServerFixture)
        {
            _testServerFixture = testServerFixture;

            var requestData = new LoginDto { Email = "admin@ask-pam.com", Password = "123qwe" };
            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            var response = (_testServerFixture.Client.PostAsync("api/account/login", content)).Result;
            var authInfoDto = JsonConvert.DeserializeObject<AuthInfoDto>(response.Content.ReadAsStringAsync().Result);
            _idToken = authInfoDto.IdToken;
        }

        //[Fact]
        //public async Task Should_Create_Organization()
        //{
        //    _testServerFixture.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _idToken);
        //    var organizationName = (new Guid()).ToString();
        //    var requestData = new CreateOrganizationDto { Name= organizationName };
        //    var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
        //    var response = await _testServerFixture.client.PostAsync("api/organization/createOrganization", content);
        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
        //}
    }
}
