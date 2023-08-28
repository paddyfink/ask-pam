using System.Linq;
using AskPam.Crm.EntityFramework;
using AskPam.Module;
using Autofac;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using AskPam.Crm.Auth0;
using AskPam.Crm.Authorization;
using AskPam.Crm.Conversations;
using AskPam.Crm.Smooch;
using AskPam.Crm.Twilio;
using AskPam.Crm.Storage;
using AskPam.Crm.Postmark;
using AskPam.Crm.AI;
using AskPam.Crm.AzureStorage;
using AskPam.Crm.FullContact;
using AskPam.Crm.Contacts;
using AskPam.Crm.Sendgrid;
using AskPam.EntityFramework.Repositories;
using AskPam.Crm.Broid;
using AskPam.Crm.Common;

namespace AskPam.Crm
{
    public class CrmInfrastructureModule : ModuleBase
    {

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new AskPam.EntityFramework.AskPamEntitityFrameworkModule());
            var dataAccess = Assembly.GetEntryAssembly();

            builder.RegisterAssemblyTypes(dataAccess)
                   .Where(t => t.Name.EndsWith("Service"))
                   .AsImplementedInterfaces();

            builder.RegisterType<CrmDbContext>().As<DbContext>().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork<DbContext>>().As<IUnitOfWork>();
            builder.RegisterType<Auth0UserService>().As<IExternalUserService>();
            builder.RegisterType<SmoochConversationService>().As<ISmoochConversationService>();
            builder.RegisterType<TwilioPhoneNumberLookupService>().As<IPhoneNumberLookupService>();
            builder.RegisterType<AzureStorageService>().As<IStorageService>();
            builder.RegisterType<PostmarkService>().As<IPostmarkService>();
            builder.RegisterType<PostmarkService>().As<IPostmarkService>();
            builder.RegisterType<QnAMakerService>().As<IQnAMakerService>();
            builder.RegisterType<FulllContactService>().As<IFullContactService>();
            builder.RegisterType<SendgridEmailService>().As<IMailService>();
            builder.RegisterType<EmailNotifier>().AsSelf();
            //builder.RegisterType<e180Service>().AsSelf();
            //builder.RegisterType<Auth0UserService>().As<IBotService>();
            //builder.RegisterType<Auth0UserService>().As<ITextAnalysisService>();

        }
    }
}
