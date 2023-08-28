using AskPam.Crm.Authorization;
using AskPam.Crm.EntityFramework;
using AskPam.EntityFramework.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.UnitTests.Managers
{
    public class BaseManagerTest : BaseTest
    {
        protected User CurrentUser;
        protected CrmDbContext Context { get; set; }
        protected IUnitOfWork mockUnitOfWork;

        public override void Dispose()
        {
            Context.Dispose();
            base.Dispose();
        }
    }

}
