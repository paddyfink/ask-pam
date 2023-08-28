using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AskPam.Module;
using Autofac;
using System.Reflection;
using AskPam.Domain.Services;
using AskPam.Events;

namespace AskPam.Crm
{
    public class CrmCoreModule : ModuleBase
    {
        protected override void Load(ContainerBuilder builder)
        {
            var core = typeof(CrmCoreModule).GetTypeInfo().Assembly;
            builder.RegisterAssemblyTypes(core)
                   .AssignableTo<IDomainService>()
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterType<DomainEvents>().As<IDomainEvents>();
        }
    }
}
