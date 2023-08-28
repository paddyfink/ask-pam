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
    public class When_Create_Library_Item
    {
        readonly TestServerFixture _testServerFixture;

        public When_Create_Library_Item(TestServerFixture testServerFixture)
        {
            _testServerFixture = testServerFixture;
        }

        [Fact]
        public async Task Should_Return_Ok()
        {
            var requestData = new LibraryItemDto()
            {
                Name = "Name Test",
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            var response = await _testServerFixture.Client.PostAsync("api/Library", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        
    }
}
