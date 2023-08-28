using AskPam.Domain.Entities;
using AskPam.Domain.Repositories;
using AskPam.EntityFramework.Repositories;
using AskPam.Module;
using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.EntityFramework
{
    public class AskPamEntitityFrameworkModule : ModuleBase
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(EfCoreRepositoryBase<>)).As(typeof(IRepository<>));
            builder.RegisterGeneric(typeof(EfCoreRepositoryBase<,>)).As(typeof(IRepository<,>));
        }

    }
}
