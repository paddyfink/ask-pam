using AskPam.Crm.Contacts;
using System.Collections.Generic;
using AskPam.Crm.Contacts.Dtos;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using System.Linq;
using AskPam.Crm.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using AskPam.Application.Dto;
using System;
using AskPam.Crm.Tags;
using Moq;
using Bogus;

namespace AskPam.Crm.UnitTests.Controllers.Contacts
{

    public class WhenGetContacts : BaseContactsControllerTestsController
    {

        public WhenGetContacts() : base()
        {
            CreateTestData();
            MockCrmSession = new MockCrmSession { UserId = TestHelper.User1Id, OrganizationId = new Guid(TestHelper.Orgnization1Id)};
            Context = new CrmDbContext(ContextOptions, MockCrmSession);
        }

        [Fact(DisplayName = "WhenGetContacts_ShouldReturnList")]
        public async Task ShouldReturnLists()
        {
            
            var controller = CreateContactsController();
            var input = new ContactListRequestDto
            {
                MaxResultCount = 20,
                SkipCount = 0,
            };


            ObjectResult result = (ObjectResult)(await controller.GetContacts(input));
            var pagegContacts = (PagedResultDto<ContactListDto>)(result.Value);


            pagegContacts.Items.Count.ShouldBe(20);
            pagegContacts.TotalCount.ShouldBe(50);
            pagegContacts.HasNext.ShouldBe(true);

            pagegContacts.Items.All(i => i.OrganizationId == new Guid(TestHelper.Orgnization1Id)).ShouldBe(true);

            Context.Database.EnsureDeleted();
        }

        [Fact(DisplayName = "WhenGetContacts_ShouldReturnFilteredLists")]
        public async Task ShouldReturnFilteredLists()
        {
            MockCrmSession = new MockCrmSession { UserId = "userId", OrganizationId = new Guid(TestHelper.Orgnization1Id) };
            var controller = CreateContactsController();

            var input = new ContactListRequestDto
            {
                GroupId=1,
                MaxResultCount = 50,
                SkipCount = 0,
            };

            MockTagsManager.Setup(t => t.Search(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(new List<Tag>());
            ObjectResult result = (ObjectResult)(await controller.GetContacts(input));
            var pagegContacts = (PagedResultDto<ContactListDto>)(result.Value);

            pagegContacts.Items.All(c=>c.GroupId==1).ShouldBe(true, "Should filter by Group Id");
            pagegContacts.HasNext.ShouldBe(false);

            Context.Database.EnsureDeleted();
        }

        //[Fact(DisplayName = "WhenGetContacts_ShouldReturnSortedLists")]
        //public async Task ShouldReturnSortedLists()
        //{
        //    var input = new ContactListRequestDto
        //    {
        //        Sorting = "FullName",
        //        MaxResultCount = 5,
        //        SkipCount = 0,
        //    };

        //    mockTagsManager.Setup(t => t.Search("", Guid.Empty)).ReturnsAsync(new List<Tag>());

        //    ObjectResult result = (ObjectResult)(await controller.GetContacts(input));
        //    var pagegContacts = (PagedResultDto<ContactListDto>)(result.Value);

        //    pagegContacts.Items.Count.ShouldBe(5);
        //    pagegContacts.Items[0].FullName.ShouldBe("1 user1");

        //    Context.Database.EnsureDeleted();
        //}


        #region private 
        private void CreateTestData()
        {

            foreach (var org in TestHelper.Organizations)
            {
                TestHelper.GenerateContactDataTests(ContextOptions, org.Id,50);
            }
        }

       
        #endregion
    }
}
