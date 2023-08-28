using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AskPam.Crm.Authorization.Dtos;
using AskPam.Crm.Controllers.Libraries.Dtos;
using AskPam.Crm.IntegratedTests.TestDatas;
using AskPam.Crm.Library.Dtos;
using Newtonsoft.Json;
using Xunit;

namespace AskPam.Crm.IntegratedTests.LibrariesController
{
    [Collection(Consts.TEST_COLLECTION)]
    public class When_Delete_LibraryItem
    {
        readonly TestServerFixture _testServerFixture;

        public When_Delete_LibraryItem(TestServerFixture testServerFixture)
        {
            _testServerFixture = testServerFixture;
            _testServerFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _testServerFixture.user1IdToken);
            _testServerFixture.Client.DefaultRequestHeaders.Add("Organization", _testServerFixture.user1OrganizationId.ToString());
        }

        

        [Fact]
        public async Task Should_Return_Ok()
        {
            var response = await _testServerFixture.Client.DeleteAsync("api/Library/2");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

       
    }
}
