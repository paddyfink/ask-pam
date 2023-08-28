using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AskPam.Crm.EntityFramework;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AskPam.Crm.Settings;
using AutoMapper;
using AskPam.Crm.Authorization.Organizations;
using AskPam.Crm.Runtime;
using AskPam.Crm.Runtime.Session;
using Microsoft.AspNetCore.Http;
using AskPam.Crm.Authorization.Library;
using AskPam.Crm.Helpers.Filters;
using Serilog;
using AskPam.Crm.Conversations;
using System.Reflection;
using AskPam.Crm.Helpers.Extensions;
using System.Collections.Generic;
using AskPam.Crm.Authorization.Users;
using AskPam.Events;
using Microsoft.Extensions.DependencyModel;
using System.Linq;
using Elmah.Io.AspNetCore;
using AskPam.Crm.Posts;
using AskPam.Crm.InternalNotes;
using Elmah.Io.Extensions.Logging;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AskPam.Crm.Configuration;
using Newtonsoft.Json.Serialization;
using AskPam.Crm.Contacts;
using AskPam.Crm.Library;
using AskPam.Crm.Organizations;
using System.IO;
using AskPam.Crm.Hubs;

namespace AskPam.Crm
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            if (env.IsDevelopment())
            {
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            Log.Logger = new LoggerConfiguration()
                                .WriteTo.RollingFile(pathFormat: "logs\\log-{Date}.log")
                                .CreateLogger();
        }

        public IConfigurationRoot Configuration { get; }

        public IContainer ApplicationContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //Set up Database
            SetUpDataBase(services);

            //App settings
            services.Configure<Auth0Settings>(Configuration.GetSection("Auth0"));
            services.Configure<ElmahSettings>(Configuration.GetSection("Elmah"));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Audience = Configuration["auth0:clientId"];
                    options.Authority = $"https://{Configuration["auth0:domain"]}/";

                    //Events = new JwtBearerEvents
                    //{
                    //    OnTokenValidated = context =>
                    //    {
                    //        // If you need the user's information for any reason at this point, you can get it by looking at the Claims property
                    //        // of context.Ticket.Principal.Identity
                    //        if (context.Ticket.Principal.Identity is ClaimsIdentity claimsIdentity)
                    //        {
                    //            // Get the user's ID
                    //            string userId = claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                    //            string email = claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;

                    //            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, email));
                    //            //identity.AddClaim(new Claim("sub", context.UserName));
                    //        }

                    //        return Task.FromResult(0);
                    //    }
                    //}
                });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //aplication insights
            services.AddApplicationInsightsTelemetry(Configuration);

            #region Cors Policy
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    config => config
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
            #endregion

            //SignalR
            services.AddSignalR(o =>
            {
                o.JsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            //Hangfire
            services.AddHangfire(config => config.UseSqlServerStorage(Configuration.GetConnectionString("Default")));

            // Add framework services.
            services.AddMvc(options =>
            {
                options.Filters.AddService(typeof(ApiExceptionFilter));
            });

           

            #region Autofac

            // Create the container builder.
            var builder = new ContainerBuilder();

            // Register dependencies, populate the services from
            // the collection, and build the container. If you want
            // to dispose of the container at the end of the app,
            // be sure to keep a reference to it as a property or field.

            builder.RegisterModule(new CrmCoreModule());
            builder.RegisterModule(new CrmInfrastructureModule());
            builder.RegisterType<CrmSession>().As<ICrmSession>().InstancePerLifetimeScope();
            builder.RegisterType<ApiExceptionFilter>();
            //automapper
            //register your profiles, or skip this if you don't want them in your container
            builder.RegisterAssemblyTypes().AssignableTo(typeof(Profile));


            //Domain event
            RegisterHandlers(builder);

            #region Automapper
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile(new OrganizationsAutomapperProfile());
                cfg.AddProfile(new ContactsAutomapperProfile());
                cfg.AddProfile(new LibraryAutomapperProfile());
                cfg.AddProfile(new ConversationsAutomapperProfile());
                cfg.AddProfile(new UserAutomapperProfile());
                cfg.AddProfile(new AccountAutomapperProfile());
                cfg.AddProfile(new TagsAutomapperProfile());
                cfg.AddProfile(new InternalNotesAutomapperProfile());
                cfg.AddProfile(new NotificationsAutomapperProfile());
                cfg.AddProfile(new SettingsAutomapperProfile());
                cfg.AddProfile(new PostAutomapperProfile());
            });
            #endregion



            builder.Populate(services);
            this.ApplicationContainer = builder.Build();

            #endregion

            #region  Audit

            //Audit.Core.Configuration.DataProvider = new SqlDataProvider()
            //{
            //    ConnectionString = Configuration.GetConnectionString("Default"),
            //    Schema = "Audit",
            //    TableName = "Events",
            //    IdColumnName = "EventId",
            //    JsonColumnName = "Data",
            //    LastUpdatedDateColumnName = "LastUpdatedDate"
            //};

            #endregion


            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();

            // Ensure any buffered events are sent at shutdown
            appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                loggerFactory.WithFilter(new FilterLoggerSettings
                    {
                        {"Trace",LogLevel.Trace },
                        {"Default", LogLevel.Trace},
                        {"Microsoft", LogLevel.Warning}, // very verbose
                        {"System", LogLevel.Warning}
                    })
                   .AddConsole()
                   .AddSerilog();

            }
            else
            {
                app.UseHttpsEnforcement();

                if (!string.IsNullOrEmpty(Configuration["Elmah:AppId"]) && !string.IsNullOrEmpty(Configuration["Elmah:LogId"]))
                    loggerFactory.AddElmahIo(Configuration["Elmah:AppId"], new Guid(Configuration["Elmah:LogId"]));
            }

         
            //Elmah.io
            if (!string.IsNullOrEmpty(Configuration["Elmah:AppId"]) && !string.IsNullOrEmpty(Configuration["Elmah:LogId"]))
                app.UseElmahIo(Configuration["Elmah:AppId"], new Guid(Configuration["Elmah:LogId"]));

            //jwt
            app.UseAuthentication();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());

            //SignalR
            app.UseSignalR(routes =>
            {
                routes.MapHub<NotificationHub>("notification");
                routes.MapHub<ConversationHub>("conversation");
            });

            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new MyRestrictiveAuthorizationFilter() }
            });

            //RecurringJob.AddOrUpdate<e180Service>(
            //    x => x.Sync(), Cron.MinuteInterval(15));

            // Redirect any non-API calls to the Angular application
            // so our application can handle the routing
            app.Use(async (context, next) => {
                await next();
                if (context.Response.StatusCode == 404 &&
                    !Path.HasExtension(context.Request.Path.Value) &&
                    !context.Request.Path.Value.StartsWith("/api/"))
                {
                    context.Request.Path = "/index.html";
                    await next();
                }
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseMvc();

        }

        public virtual void SetUpDataBase(IServiceCollection services)
        {
            services.AddDbContext<CrmDbContext>(options =>
                 options.UseSqlServer(Configuration.GetConnectionString("Default"),
                 opt => opt.EnableRetryOnFailure()
                 ));
        }



        private void RegisterHandlers(ContainerBuilder builder)
        {
            IEnumerable<Assembly> assemblies = GetAssemblies();

            builder.RegisterAssemblyTypes(assemblies.ToArray())
                .AsClosedTypesOf(typeof(IEventHandler<>))
                .InstancePerLifetimeScope();
        }

        private static Assembly[] GetAssemblies()
        {
            var assemblies = new List<Assembly>();
            var dependencies = DependencyContext.Default.RuntimeLibraries;
            foreach (var library in dependencies)
            {
                if (IsCandidateCompilationLibrary(library))
                {
                    var assembly = Assembly.Load(new AssemblyName(library.Name));
                    assemblies.Add(assembly);
                }
            }
            return assemblies.ToArray();
        }

        private static bool IsCandidateCompilationLibrary(RuntimeLibrary compilationLibrary)
        {
            return compilationLibrary.Name.StartsWith("askpam.crm", StringComparison.OrdinalIgnoreCase);
        }
    }

    public class MyRestrictiveAuthorizationFilter : IDashboardAuthorizationFilter
    {

        public bool Authorize(DashboardContext context)
        {
            //var owinEnvironment = context.GetOwinEnvironment();

            //if (owinEnvironment.ContainsKey("server.User"))
            //{
            //    if (owinEnvironment["server.User"] is ClaimsPrincipal)
            //    {
            //        return (owinEnvironment["server.User"] as ClaimsPrincipal).Identity.IsAuthenticated;
            //    }
            //    else if (owinEnvironment["server.User"] is GenericPrincipal)
            //    {
            //        return (owinEnvironment["server.User"] as GenericPrincipal).Identity.IsAuthenticated;
            //    }
            //}

            //if (HttpContext.Current.User.IsInRole("Admin"))
            //{
            //    return true;
            //}

            return true;
        }
    }
}

