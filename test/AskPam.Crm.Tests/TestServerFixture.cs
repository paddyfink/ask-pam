using System;
using System.Net.Http;
using System.Text;
using AskPam.Crm.Authorization.Dtos;
using AskPam.Crm.IntegratedTests.TestDatas;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;

namespace AskPam.Crm.IntegratedTests
{
    public class TestServerFixture : IDisposable
    {
        public TestServer Server { get; }
        public HttpClient Client { get; }

        public string user1IdToken;
        public Guid? user1OrganizationId;

        public TestServerFixture()
        {
            var builder = new WebHostBuilder()
             //.UseEnvironment("Development")
             .UseStartup<TestStartup>();

            Server = new TestServer(builder);
            Client = Server.CreateClient();
            Client.BaseAddress = new Uri("https://localhost");

            var requestData = new LoginDto { Email = TestUsersBuilder.user1Email, Password = TestUsersBuilder.userPassword };
            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            var response = (Client.PostAsync("api/account/login", content)).Result;
            var authInfoDto = JsonConvert.DeserializeObject<AuthInfoDto>(response.Content.ReadAsStringAsync().Result);
            user1IdToken = authInfoDto.IdToken;
            user1OrganizationId = authInfoDto.OrganizationId;
        }
        public void Dispose()
        {
            Server.Dispose();
            Client.Dispose();
        }
    }
}
