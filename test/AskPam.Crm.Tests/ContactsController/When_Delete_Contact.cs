﻿using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AskPam.Crm.Contacts.Dtos;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace AskPam.Crm.IntegratedTests.ContactsController
{
    [Collection(Consts.TEST_COLLECTION)]
    public class When_Delete_Contact
    {
        readonly TestServerFixture _testServerFixture;
        
        public When_Delete_Contact(TestServerFixture testServerFixture)
        {
            _testServerFixture = testServerFixture;

            _testServerFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _testServerFixture.user1IdToken);
            _testServerFixture.Client.DefaultRequestHeaders.Add("Organization", _testServerFixture.user1OrganizationId.ToString());
        }


        [Fact]
        public async Task Should_Return_NoContent()
        {
            var response = await _testServerFixture.Client.DeleteAsync($"api/contacts/2");
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Should_Return_Notfound()
        {
            var response = await _testServerFixture.Client.DeleteAsync($"api/contacts/12");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
