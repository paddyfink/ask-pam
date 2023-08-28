using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AskPam.Crm.Authorization.Dtos;
using AskPam.Crm.IntegratedTests.TestDatas;
using Newtonsoft.Json;
using Xunit;

namespace AskPam.Crm.IntegratedTests.Users
{
    [Collection(Consts.TEST_COLLECTION)]
    public class User_Tests 
    {
        TestServerFixture _testServerFixture;

        private string _idToken;
        private Guid? _organizationId;

        public User_Tests(TestServerFixture testServerFixture) 
        {
            _testServerFixture = testServerFixture;

            var requestData = new LoginDto { Email = TestUsersBuilder.user1Email, Password = TestUsersBuilder.userPassword };
            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            var response = (_testServerFixture.Client.PostAsync("api/account/login", content)).Result;
            var authInfoDto = JsonConvert.DeserializeObject<AuthInfoDto>(response.Content.ReadAsStringAsync().Result);
            _idToken = authInfoDto.IdToken;
            _organizationId = authInfoDto.OrganizationId;
        }

        //[Fact]
        //public async Task Should_Get_FilteredUsers()
        //{
        //    _testServerFixture.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _idToken);
        //    _testServerFixture.client.DefaultRequestHeaders.Add("Organization", _organizationId.ToString());
        //    var requestData = new UserListRequestDto()
        //    {
        //        //Filter = "Test",
        //        Sorting = "FirstName"
        //    };
        //    var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
        //    var response = await _testServerFixture.client.PostAsync("api/User/GetUsers", content);
        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //}

        [Fact]
        public async Task Should_Get_Roles()
        {
            _testServerFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _idToken);
            _testServerFixture.Client.DefaultRequestHeaders.Add("Organization", _organizationId.ToString());
            var response = await _testServerFixture.Client.GetAsync("api/User/GetRoles");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        //[Fact]
        //public async Task Should_Get_Users()
        //{
        //    _testServerFixture.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _idToken);
        //    _testServerFixture.client.DefaultRequestHeaders.Add("Organization", _organizationId.ToString());
        //    var response = await _testServerFixture.client.GetAsync("api/User/GetAllUsers");
        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //    var users = JsonConvert.DeserializeObject<IList<UserListDto>>(response.Content.ReadAsStringAsync().Result);

        //    users.Count.ShouldBe(3);
        //}

        //[Fact]
        //public async Task Should_AssignToRole()
        //{
        //    _testServerFixture.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _idToken);
        //    _testServerFixture.client.DefaultRequestHeaders.Add("Organization", _organizationId.ToString());
        //    var requestData = new UserRoleAssignationDto()
        //    {
        //        UserId = TestUsersBuilder.user1Email,
        //        RoleName = RolesName.Reader
        //    };
        //    var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
        //    var response = await _testServerFixture.client.PostAsync("api/User/assignToRole", content);
        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //}
    }
}
