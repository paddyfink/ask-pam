//using AskPam.Crm.Organizations;
//using AskPam.Crm.Runtime.Session;
//using AskPam.Domain.Repositories;
//using AutoMapper;
//using Moq;
//using System;
//using System.Collections.Generic;
//using AskPam.Crm.Organizations.Dtos;
//using System.Threading.Tasks;
//using Xunit;
//using AskPam.Web.Dtos;
//using Shouldly;
//using System.Linq;
//using AskPam.Crm.EntityFramework;
//using AskPam.EntityFramework.Repositories;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Mvc;

//namespace AskPam.Crm.Tests.UnitTests.Controllers
//{
//    public class OrganizationsController_Tests
//    {
//        DbContextOptions<CrmDbContext> options;

//        private IRepository<Organization, Guid> repo;
//        private Mock<OrganizationManager> mockOrgManager;
//        private Mock<IMapper> mockMapper;
//        private ICrmSession mockSession;
//        private Guid orgId;
//        public OrganizationsController_Tests()
//        {
//            var builder = new DbContextOptionsBuilder<CrmDbContext>();
//            builder.UseInMemoryDatabase();
//            options = builder.Options;

//            orgId = Guid.NewGuid();
//            mockSession = new MockCrmSession { UserId = "userId", OrganizationId = orgId };
                        

//            mockOrgManager = new Mock<OrganizationManager>();

//            mockMapper = new Mock<IMapper>();
//            mockMapper.Setup(x => x.Map<Organization, OrganizationDto>(It.IsAny<Organization>()))
//              .Returns((Organization source) =>
//              {
//                  return new OrganizationDto();
//              });

//            CreateTestData();
//        }


//        [Fact]
//        public async Task GetOrganizations_Should_return_pagedlists()
//        {
           

//            var _context = new CrmDbContext(options, mockSession);
//            repo = new EfCoreRepositoryBase<Organization, Guid>(_context);

//            var input = new GetOrganizationsRequestDto
//            {
//                MaxResultCount = 2,
//                SkipCount = 0,
//            };
//            Mapper.Initialize(cfg => cfg.CreateMap<Organization, OrganizationDto>()); ;
//            var controller = new OrganizationController(mockSession, mockMapper.Object, repo, mockOrgManager.Object);
//            ObjectResult result = (ObjectResult)(await  controller.GetOrganizations(input));
//            var pagegOrganizations = (PagedResultDto<OrganizationDto>)(result.Value);


//            pagegOrganizations.Items.Count.ShouldBe(2);
//            pagegOrganizations.TotalCount.ShouldBe(5);
//            pagegOrganizations.HasNext.ShouldBe(true);

//            _context.Database.EnsureDeleted();
//        }

//        [Fact]
//        public async Task GetOrganizations_Should_return_filtered_Lists()
//        {
           
//            var _context = new CrmDbContext(options, mockSession);
//            repo = new EfCoreRepositoryBase<Organization, Guid>(_context);

//            var input = new GetOrganizationsRequestDto
//            {
//                Filter = "Ask",
//                MaxResultCount = 20,
//                SkipCount = 0,
//            };
//            Mapper.Initialize(cfg => cfg.CreateMap<Organization, OrganizationDto>()); ;
//            var controller = new OrganizationController(mockSession, mockMapper.Object, repo, mockOrgManager.Object);
//            ObjectResult result = (ObjectResult)(await controller.GetOrganizations(input));
//            var pagegOrganizations = (PagedResultDto<OrganizationDto>)(result.Value);


//            pagegOrganizations.Items.Count.ShouldBe(1);

//            _context.Database.EnsureDeleted();
//        }


//        #region private 
//        private void CreateTestData()
//        {

//            var orgs = new List<Organization>
//            {
//                new Organization{Name="John Paul",Type=OrganizationType.Company },
//                new Organization{Name="C2 Montreal",Type=OrganizationType.Company },
//                new Organization{Name="Ask Pam",Type=OrganizationType.Company },
//                new Organization{Name="Wework",Type=OrganizationType.Individual },
//                new Organization{Name="Lavallée",Type=OrganizationType.Individual },
//            };

//            using (var context = new CrmDbContext(options, new NullCrmSession()))
//            {
//                context.Organizations.AddRange(orgs);
//                context.SaveChanges();
//            }
//        }

//        //return orgs.ToArray();
//    }
//    #endregion
//}
