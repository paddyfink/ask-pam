//using AskPam.Crm.Runtime.Session;
//using AskPam.Domain.Repositories;
//using AutoMapper;
//using Moq;
//using System;
//using System.Collections.Generic;
//using AskPam.Crm.Users.Dtos;
//using System.Threading.Tasks;
//using Xunit;
//using AskPam.Web.Dtos;
//using Shouldly;
//using AskPam.Crm.EntityFramework;
//using AskPam.EntityFramework.Repositories;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Mvc;
//using AskPam.Crm.Authorization;
//using AskPam.Crm.Users;
//using AskPam.Crm.Authorization.Users;
//using AskPam.Crm.Organizations;
//using System.Linq;

//namespace AskPam.Crm.Tests.UnitTests.Controllers
//{
//    public class UsersController_Tests
//    {
//        DbContextOptions<CrmDbContext> options;

//        private Mock<UserManager> mockUserManager;
//        private IRepository<User, string> userRepo;
//        private IRepository<UserRole> userRoleRepo;
//        private IRepository<Role> roleRepo;
//        private Mock<IMapper> mockMapper;
//        private ICrmSession mockSession;
//        private Guid orgId;
//        private CrmDbContext context;

//        public UsersController_Tests()
//        {
//            var builder = new DbContextOptionsBuilder<CrmDbContext>();
//            builder.UseInMemoryDatabase();
//            options = builder.Options;

//            orgId = Guid.NewGuid();
//            mockSession = new MockCrmSession { UserId = "userId", OrganizationId = orgId };

//            mockUserManager = new Mock<UserManager>();
//            context = new CrmDbContext(options, mockSession);
//            userRepo = new EfCoreRepositoryBase<User, string>(context);
//            userRoleRepo = new EfCoreRepositoryBase<UserRole>(context);
//            roleRepo = new EfCoreRepositoryBase<Role>(context);

//            mockMapper = new Mock<IMapper>();
//            mockMapper.Setup(x => x.Map<User, UserDto>(It.IsAny<User>()))
//              .Returns((User source) =>
//              {
//                  return new UserDto();
//              });

//            CreateTestData();
//        }

//        [Fact]
//        public async Task GetUsers_Should_return_pagedlists()
//        {
//            var input = new UserListRequestDto
//            {
//                MaxResultCount = 2,
//                SkipCount = 0,
//            };
//            Mapper.Initialize(m => m.AddProfile<UserAutomapperProfile>());
//            var controller = new UserController(mockSession, mockMapper.Object, userRepo, userRoleRepo, roleRepo, mockUserManager.Object);
//            ObjectResult result = (ObjectResult)(await controller.GetUsers(input));
//            var pagegUsers = (PagedResultDto<UserListDto>)(result.Value);


//            pagegUsers.Items.Count.ShouldBe(2);
//            pagegUsers.TotalCount.ShouldBe(4);
//            pagegUsers.HasNext.ShouldBe(true);

//            context.Database.EnsureDeleted();
//        }

//        [Fact]
//        public async Task GetUsers_Should_return_filtered_Lists()
//        {
//            var input = new UserListRequestDto
//            {
//                Filter = "Doe",
//                Sorting = "FirstName",
//                MaxResultCount = 2,
//                SkipCount = 1,
//            };
//            Mapper.Initialize(m => m.AddProfile<UserAutomapperProfile>());
//            var controller = new UserController(mockSession, mockMapper.Object, userRepo, userRoleRepo, roleRepo, mockUserManager.Object);
//            ObjectResult result = (ObjectResult)(await controller.GetUsers(input));
//            var pagegUsers = (PagedResultDto<UserListDto>)(result.Value);

//            pagegUsers.Items.Count.ShouldBe(2);
//            pagegUsers.TotalCount.ShouldBe(3);
//            pagegUsers.HasNext.ShouldBe(false);

//            context.Database.EnsureDeleted();
//        }


//        #region private 
//        private void CreateTestData()
//        {
//            using (var context = new CrmDbContext(options, new NullCrmSession()))
//            {
//                var roles = new List<Role>
//                {
//                    new Role(orgId, "role 1"),
//                    new Role(orgId, "role 2"),
//                };
//                context.Roles.AddRange(roles);
//                context.SaveChanges();

//                var role1 = context.Roles.First();

//                var users = new List<User>
//                {
//                    new User("Auth|0", "John", "Doe", "jd@test.com", "", "", ""),
//                    new User("Auth|1", "Peter", "Pan", "pp@test.com", "", "", ""),
//                    new User("Auth|2", "Alice", "Doe", "ad@test.com", "", "", ""),
//                    new User("Auth|3", "Lana", "Doe", "ld@test.com", "", "", ""),
//                };
//                foreach (var user in users)
//                {
//                    //user.AddToOrganization(orgId);
//                    //user.AssignToRole(role1);
//                }
//                context.Users.AddRange(users);
//                context.SaveChanges();

//                var role2 = context.Roles.Skip(1).First();
//                var users2 = new List<User>
//                {
//                    new User("Auth|4", "Lana", "Doe", "ld@test.com", "", "", ""),
//                    new User("Auth|5", "Lana", "Doe", "ld@test.com", "", "", "")
//                };
//                foreach (var user in users2)
//                {
//                    //user.AddToOrganization(orgId);
//                    //user.AssignToRole(role2);
//                }

//                context.Users.AddRange(users2);
//                context.SaveChanges();
//            }
//        }
//        #endregion
//    }
//}
