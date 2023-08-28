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
    public class When_Update_LibraryItem
    {
        readonly TestServerFixture _testServerFixture;

        public When_Update_LibraryItem(TestServerFixture testServerFixture)
        {
            _testServerFixture = testServerFixture;
            _testServerFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _testServerFixture.user1IdToken);
            _testServerFixture.Client.DefaultRequestHeaders.Add("Organization", _testServerFixture.user1OrganizationId.ToString());
        }

        

        [Fact]
        public async Task Should_Update_LibraryItem()
        {
            var libraryItem = new LibraryItemDto()
            {
                Id = 1,
                Name = "Name Test"
            };
            var content = new StringContent(JsonConvert.SerializeObject(libraryItem), Encoding.UTF8, "application/json");
            var response = await _testServerFixture.Client.PutAsync("api/Library/1", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

       
    }
}
