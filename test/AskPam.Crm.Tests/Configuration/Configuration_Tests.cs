using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AskPam.Crm.Authorization.Dtos;
using AskPam.Crm.Controllers.Configuration.Dtos;
using AskPam.Crm.IntegratedTests.TestDatas;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace AskPam.Crm.IntegratedTests.Configuration
{
    [Collection(Consts.TEST_COLLECTION)]
    public class Configuration_Tests
    {
        TestServerFixture _testServerFixture;
        private string _idToken;
        private Guid _organizationId;

        public Configuration_Tests(TestServerFixture testServerFixture)
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
        public async Task Should_Get_BotSettings()
        {
            _testServerFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _idToken);
            _testServerFixture.Client.DefaultRequestHeaders.Add("Organization", _organizationId.ToString());
            var response = await _testServerFixture.Client.GetAsync($"/api/OrganizationSettings/GetBotSettings");
            Assert.True(response.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_Not_Update_BotSettings()
        {
            _testServerFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _idToken);
            _testServerFixture.Client.DefaultRequestHeaders.Add("Organization", _organizationId.ToString());
            var requestData = new BotSettingsDto(); //Test with no parameter
            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            var response = await _testServerFixture.Client.PostAsync("/api/OrganizationSettings/UpdateBotSettings", content);
            Assert.Equal(response.StatusCode, HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_Update_BotSettings()
        {
            _testServerFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _idToken);
            _testServerFixture.Client.DefaultRequestHeaders.Add("Organization", _organizationId.ToString());
            var requestData = new BotSettingsDto()
            {
                BotAvatar = "BotAvatar",
                BotName = "BotName",
                BotEnabled = true,
                DesactivationEnabled = false,
                Intro = "Intro",
                Outro = "Outro",
                Treshold = 10,
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            var response = await _testServerFixture.Client.PostAsync("/api/OrganizationSettings/UpdateBotSettings", content);
            Assert.Equal(response.StatusCode, HttpStatusCode.OK);


            _testServerFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _idToken);
            _testServerFixture.Client.DefaultRequestHeaders.Add("Organization", _organizationId.ToString());
            var response2 = await _testServerFixture.Client.GetAsync($"/api/OrganizationSettings/GetBotSettings");
            Assert.True(response2.StatusCode == HttpStatusCode.OK);

            var botSettings = JsonConvert.DeserializeObject<BotSettingsDto>(response2.Content.ReadAsStringAsync().Result);
            botSettings.BotAvatar.ShouldBe("BotAvatar");
            botSettings.BotName.ShouldBe("BotName");
            botSettings.BotEnabled.ShouldBe(true);
            botSettings.DesactivationEnabled.ShouldBe(false);
            botSettings.Intro.ShouldBe("Intro");
            botSettings.Outro.ShouldBe("Outro");
            botSettings.Treshold.ShouldBe(10);
        }

        [Fact]
        public async Task Should_Get_FilteredQnAs()
        {
            _testServerFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _idToken);
            _testServerFixture.Client.DefaultRequestHeaders.Add("Organization", _organizationId.ToString());
            var requestData = new QnAPairRequestDto()
            {
                Filter = "Question",
                Sorting = "Question",
                MaxResultCount = 20
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            var response = await _testServerFixture.Client.PostAsync("api/OrganizationSettings/GetQnas", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Should_Create_KnowledgeBase()
        {
            _testServerFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _idToken);
            _testServerFixture.Client.DefaultRequestHeaders.Add("Organization", _organizationId.ToString());
            var requestData = new List<QnAPairDto>()
            {
                new QnAPairDto()
                {
                    Question = "New Question" + DateTime.Now.ToString(CultureInfo.InvariantCulture),
                    Answer = "New Answer",
                },
                new QnAPairDto()
                {
                    Id = 1,
                    Question = "Question 1",
                    Answer = "Edit Answer 1",
                },
                new QnAPairDto()
                {
                    Id = 2,
                    Question = "Question 2",
                    Answer = "Answer 2",
                    IsDeleted = true
                }
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            var response = await _testServerFixture.Client.PostAsync("/api/OrganizationSettings/SaveQnAs", content);
            Assert.Equal(response.StatusCode, HttpStatusCode.OK);
        }

        //[Fact]
        //public async Task Should_Get_Answer()
        //{
        //    _testServerFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _idToken);
        //    _testServerFixture.Client.DefaultRequestHeaders.Add("Organization", _organizationId.ToString());
        //    var requestData = "question";
        //    var response = await _testServerFixture.Client.GetAsync($"api/OrganizationSettings/Ask?question={requestData}");
        //    Assert.Equal(response.StatusCode, HttpStatusCode.OK);

        //    var result = JsonConvert.DeserializeObject<QnAMakerResultDto>(await response.Content.ReadAsStringAsync());
        //    Assert.True(result != null);
        //}
    }
}
