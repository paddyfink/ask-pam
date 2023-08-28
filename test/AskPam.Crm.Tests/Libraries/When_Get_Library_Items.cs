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
    public class When_Get_Library_Items
    {
        readonly TestServerFixture _testServerFixture;


        public When_Get_Library_Items(TestServerFixture testServerFixture)
        {
            _testServerFixture = testServerFixture;
            _testServerFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _testServerFixture.user1IdToken);
            _testServerFixture.Client.DefaultRequestHeaders.Add("Organization", _testServerFixture.user1OrganizationId.ToString());
        }

        

        [Fact]
        public async Task Should_Get_LibraryItems_Filtered()
        {
           
            var requestData = new LibraryItemListRequestDto()
            {
                Filter = "Test",
                MaxResultCount = 5
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            var response = await _testServerFixture.Client.PostAsync("api/Library/GetLibraryItems", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Should_Get_LibraryItems_Sorted()
        {

            var requestData = new LibraryItemListRequestDto()
            {
                Sorting = "Name",
                MaxResultCount = 5
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            var response = await _testServerFixture.Client.PostAsync("api/Library/GetLibraryItems", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Should_Get_LibraryItems()
        {
            var requestData = new LibraryItemListRequestDto()
            {
                MaxResultCount = 5
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            var response = await _testServerFixture.Client.PostAsync("api/Library/GetLibraryItems", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
