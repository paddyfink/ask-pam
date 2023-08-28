//using AskPam.Crm.Runtime.Session;
//using AskPam.Domain.Repositories;
//using AutoMapper;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Xunit;
//using AskPam.Web.Dtos;
//using Shouldly;
//using AskPam.Crm.EntityFramework;
//using AskPam.EntityFramework.Repositories;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Mvc;
//using AskPam.Crm.Library;
//using AskPam.Crm.Controllers.Libraries.Dtos;
//using AskPam.Crm.Authorization.Library;
//using System.Linq;

//namespace AskPam.Crm.Tests.UnitTests.Controllers
//{
//    public class LibrariesController_Tests
//    {
//        DbContextOptions<CrmDbContext> options;

//        private IRepository<LibraryItem> repo;
//        private Mock<LibraryManager> mockLibraryManager;
//        private Mock<IMapper> mockMapper;
//        private ICrmSession mockSession;
//        private Guid orgId;
//        public LibrariesController_Tests()
//        {
//            var builder = new DbContextOptionsBuilder<CrmDbContext>();
//            builder.UseInMemoryDatabase();
//            options = builder.Options;

//            orgId = Guid.NewGuid();
//            mockSession = new MockCrmSession { UserId = "userId", OrganizationId = orgId };

//            var _context = new CrmDbContext(options, mockSession);
//            repo = new EfCoreRepositoryBase<LibraryItem>(_context);
//            mockLibraryManager = new Mock<LibraryManager>(repo);

//            mockMapper = new Mock<IMapper>();
//            mockMapper.Setup(x => x.Map<LibraryItem, LibraryItemDto>(It.IsAny<LibraryItem>()))
//              .Returns((LibraryItem source) =>
//              {
//                  return new LibraryItemDto();
//              });

//            CreateTestData();
//        }

//        [Fact]
//        public async Task GetLibraryItems_Should_return_pagedlists()
//        {
//            var _context = new CrmDbContext(options, mockSession);
//            repo = new EfCoreRepositoryBase<LibraryItem>(_context);

//            var input = new LibraryItemListRequestDto
//            {
//                MaxResultCount = 2,
//                SkipCount = 0,
//            };
//            Mapper.Initialize(cfg => cfg.CreateMap<LibraryItem, LibraryItemDto>());
//            Mapper.Initialize(cfg => cfg.CreateMap<LibraryItem, LibraryItemListDto>());
//            var controller = new LibraryController(mockSession, mockMapper.Object, repo, mockLibraryManager.Object);
//            ObjectResult result = (ObjectResult)(await controller.GetLibraryItems(input));
//            var pagegLibraryItems = (PagedResultDto<LibraryItemListDto>)(result.Value);

//            pagegLibraryItems.Items.Count.ShouldBe(2);
//            pagegLibraryItems.TotalCount.ShouldBe(4);
//            pagegLibraryItems.HasNext.ShouldBe(true);

//            _context.Database.EnsureDeleted();
//        }

//        [Fact]
//        public async Task GetLibraryItems_Should_return_filtered_Lists()
//        {
//            var _context = new CrmDbContext(options, mockSession);
//            repo = new EfCoreRepositoryBase<LibraryItem>(_context);

//            var input = new LibraryItemListRequestDto
//            {
//                Filter = "Lana",
//                Sorting = "Name",
//                MaxResultCount = 2,
//                SkipCount = 0,
//            };
//            Mapper.Initialize(m => m.AddProfile<LibrariesAutomapperProfile>());
//            var controller = new LibraryController(mockSession, mockMapper.Object, repo, mockLibraryManager.Object);
//            ObjectResult result = (ObjectResult)(await controller.GetLibraryItems(input));
//            var pagegLibraryItems = (PagedResultDto<LibraryItemListDto>)(result.Value);

//            pagegLibraryItems.Items.Count.ShouldBe(1);
//            pagegLibraryItems.TotalCount.ShouldBe(1);
//            pagegLibraryItems.HasNext.ShouldBe(false);

//            _context.Database.EnsureDeleted();
//        }

//        [Fact]
//        public async Task GetLibraryItemTypes_Should_return_Lists()
//        {
//            var _context = new CrmDbContext(options, mockSession);
//            repo = new EfCoreRepositoryBase<LibraryItem>(_context);
            
//            Mapper.Initialize(m => m.AddProfile<LibrariesAutomapperProfile>());
//            var controller = new LibraryController(mockSession, mockMapper.Object, repo, mockLibraryManager.Object);
//            ObjectResult result = (ObjectResult)(controller.GetLibraryItemTypes());
//            var pageLibraryItemTypes = (IDictionary<int, string>)(result.Value);
//            pageLibraryItemTypes.First(i => i.Key == 1).Value.ShouldBe("Supplier");
//            pageLibraryItemTypes.First(i => i.Value == "Event").Key.ShouldBe(3);
//            pageLibraryItemTypes.Count.ShouldBe(4);
            
//            _context.Database.EnsureDeleted();
//        }


//        #region private 
//        private void CreateTestData()
//        {
//            var orgs = new List<LibraryItem>
//            {
//                new LibraryItem("John", orgId),
//                new LibraryItem("Peter", orgId),
//                new LibraryItem("Alice", orgId),
//                new LibraryItem("Lana", orgId),
//                new LibraryItem("Mike", Guid.NewGuid()),
//                new LibraryItem("Lana", Guid.NewGuid())
//            };

//            using (var context = new CrmDbContext(options, new NullCrmSession()))
//            {
//                context.LibraryItems.AddRange(orgs);
//                context.SaveChanges();
//            }
//        }
//    }
//    #endregion
//}
