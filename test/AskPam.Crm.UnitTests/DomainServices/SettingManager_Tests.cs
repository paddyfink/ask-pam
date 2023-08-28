//using System;
//using System.Threading.Tasks;
//using Xunit;
//using Shouldly;
//using System.Linq;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Caching.Memory;
//using AskPam.Crm.Organizations;
//using AskPam.Crm.Configuration;
//using AskPam.Crm.Runtime.Session;
//using AskPam.Crm.EntityFramework;
//using AskPam.Domain.Repositories;
//using AskPam.EntityFramework.Repositories;

//namespace AskPam.Crm.UnitTests.DomainServices
//{
//    public class SettingManager_Tests
//    {
//        DbContextOptions<CrmDbContext> options;

//        private IRepository<Organization, Guid> organizationRepository;
//        private IRepository<Setting, long> settingRepository;
//        private ICrmSession mockSession;
//        private CrmDbContext context;
//        private Guid orgId;

//        public SettingManager_Tests()
//        {
//            var builder = new DbContextOptionsBuilder<CrmDbContext>();
//            builder.UseInMemoryDatabase();
//            options = builder.Options;

//            orgId = Guid.NewGuid();
//            mockSession = new MockCrmSession { UserId = "userId", OrganizationId = orgId };

//            context = new CrmDbContext(options, mockSession);
//            organizationRepository = new EfCoreRepositoryBase<Organization, Guid>(context);
//            settingRepository = new EfCoreRepositoryBase<Setting, long>(context);

//            //CreateTestData();
//        }

//        [Fact]
//        public async Task Should_Get_All_OrganizationSettings()
//        {

//            var settingManager = new SettingManager(settingRepository, mockSession, new SettingDefinitionManager(), new MemoryCache(new MemoryCacheOptions()));

//            var setttings = await settingManager.GetAllSettingValuesAsync();

//            setttings.Count.ShouldBeGreaterThan(0);

//            context.Database.EnsureDeleted();
//        }

//        [Fact]
//        public async Task Should_Set_OrganizationSettings()
//        {

//            var settingManager = new SettingManager(settingRepository, mockSession, new SettingDefinitionManager(), new MemoryCache(new MemoryCacheOptions()));

//            await settingManager.ChangeSettingForOrganizationAsync(AppSettingsNames.AI.QnABotName, "value", orgId);

//            var dbvalue = context.Settings.Where(s => s.OrganizationId == orgId && s.Name == AppSettingsNames.AI.QnABotName).Select(s => s.Value).First();
//            dbvalue.ShouldBe("value");

//            var setttingsValue = await settingManager.GetSettingValueForOrganizationAsync(AppSettingsNames.AI.QnABotName, orgId);
//            setttingsValue.ShouldBe("value");

//            var defaultValue = await settingManager.GetSettingValueForOrganizationAsync(AppSettingsNames.AI.QnABotName, new Guid());
//            defaultValue.ShouldBe("Gustav");

//            context.Database.EnsureDeleted();
//        }
//    }
//}