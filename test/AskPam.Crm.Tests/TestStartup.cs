using AskPam.Crm.EntityFramework;
using AskPam.Crm.IntegratedTests.TestDatas;
using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AskPam.Crm.IntegratedTests
{
    class TestStartup : Startup
    {
        public new IConfigurationRoot Configuration { get; }
        public new IContainer ApplicationContainer { get; private set; }

        public TestStartup(IHostingEnvironment env) : base(env)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public override void SetUpDataBase(IServiceCollection services)
        {
            services.AddDbContext<CrmDbContext>(options => options.UseInMemoryDatabase());
        }

        public override void EnsureDatabaseCreated(CrmDbContext dbContext)
        {
            var testDataBuilder = new TestDataBuilder(dbContext);
            testDataBuilder.Create();
        }
    }
}
