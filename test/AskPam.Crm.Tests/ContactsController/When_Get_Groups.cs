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
    public class When_Get_Groups
    {
        readonly TestServerFixture _testServerFixture;


        public When_Get_Groups(TestServerFixture testServerFixture) 
        {
            _testServerFixture = testServerFixture;
        }
        

        [Fact]
        public async Task Should_Return_Ok()
        {
            _testServerFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _testServerFixture.user1IdToken);
            _testServerFixture.Client.DefaultRequestHeaders.Add("Organization", _testServerFixture.user1OrganizationId.ToString());
            var response = await _testServerFixture.Client.GetAsync("api/contacts/groups");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
