using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AskPam.Crm.Authorization.Dtos;
using AskPam.Crm.IntegratedTests.TestDatas;
using Newtonsoft.Json;
using Xunit;

namespace AskPam.Crm.IntegratedTests.Authorization
{
    [Collection(Consts.TEST_COLLECTION)]
    public class Account_Tests 
    {
        TestServerFixture _testServerFixture;

        public Account_Tests(TestServerFixture testServerFixture)
        {
            _testServerFixture = testServerFixture;
        }

        [Fact]
        public async Task Should_Authenticate()
        {
            var requestData = new LoginDto { Email = TestUsersBuilder.adminEmail, Password = TestUsersBuilder .userPassword};
            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            var response = await _testServerFixture.Client.PostAsync("api/account/login", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Should_unAuthorized()
        {

            var requestData = new LoginDto { Email = TestUsersBuilder.user5Email, Password = TestUsersBuilder.userPassword };
            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            var response = await _testServerFixture.Client.PostAsync("api/account/login", content);
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Should_return_error()
        {
            var requestData = new LoginDto { Email = "admin@ask-pam.com", Password = "badpassword" };
            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            var response = await _testServerFixture.Client.PostAsync("api/account/login", content);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
