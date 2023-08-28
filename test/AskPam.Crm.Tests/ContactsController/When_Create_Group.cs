using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AskPam.Crm.Authorization.Dtos;
using AskPam.Crm.Contacts.Dtos;
using AskPam.Crm.IntegratedTests.TestDatas;
using Newtonsoft.Json;
using Xunit;

namespace AskPam.Crm.IntegratedTests.ContactsController
{
    [Collection(Consts.TEST_COLLECTION)]
    public class When_Create_Group
    {
        readonly TestServerFixture _testServerFixture;


        public When_Create_Group(TestServerFixture testServerFixture) 
        {
            _testServerFixture = testServerFixture;
        }

        [Fact]
        public async Task Should_Return_Ok()
        {
            _testServerFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _testServerFixture.user1IdToken);
            _testServerFixture.Client.DefaultRequestHeaders.Add("Organization", _testServerFixture.user1OrganizationId.ToString());
            var requestData = new ContactGroupDto()
            {
                Name = "New Group"
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            var response = await _testServerFixture.Client.PostAsync("api/contacts/groups", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

       
    }
}
