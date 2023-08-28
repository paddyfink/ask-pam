using System;
using System.Collections.Generic;
using System.Net;
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
    public class When_Create_Contact
    {
        readonly TestServerFixture _testServerFixture;

        public static IEnumerable<object[]> BadRequestData()
        {
            yield return new object[] { new ContactDto { FirstName = "Test" } };
            yield return new object[] { new ContactDto { LastName = "Test"} };
            
        }

        public static IEnumerable<object[]> ShouldReturnOkData()
        {
            yield return new object[] { new ContactDto { FirstName = "FirstName", LastName= "LastName",} };
            yield return new object[] { new ContactDto { FirstName = "FirstName", LastName = "LastName" } };
        }


        public When_Create_Contact(TestServerFixture testServerFixture)
        {
            _testServerFixture = testServerFixture;

            _testServerFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _testServerFixture.user1IdToken);
            _testServerFixture.Client.DefaultRequestHeaders.Add("Organization", _testServerFixture.user1OrganizationId.ToString());
        }

        [Theory]
        [MemberData(nameof(ShouldReturnOkData))]
        public async Task Should_Return_Ok(ContactDto contact)
        {
            
            var content = new StringContent(JsonConvert.SerializeObject(contact), Encoding.UTF8, "application/json");
            var response = await _testServerFixture.Client.PostAsync("api/contacts", content);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Theory]
        [MemberData(nameof(BadRequestData))]
        public async Task Should_Return_Bad_Request(ContactDto contact)
        {
            
            var content = new StringContent(JsonConvert.SerializeObject(contact), Encoding.UTF8, "application/json");
            var response = await _testServerFixture.Client.PostAsync("api/contacts", content);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        }


    }
}
