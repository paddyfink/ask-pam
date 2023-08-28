using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AskPam.Domain.Entities;
using AskPam.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AskPam.EntityFramework.Repositories
{
    public class EfCoreRepositoryBase<TEntity> : EfCoreRepositoryBase<TEntity, long>, IRepository<TEntity>
        where TEntity : class, IEntity<long>
    {
        public EfCoreRepositoryBase(DbContext context)
            : base(context)
        {
        }
    }
}
