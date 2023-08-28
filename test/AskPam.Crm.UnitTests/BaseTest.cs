using AskPam.Crm.EntityFramework;
using AskPam.Crm.Organizations;
using AskPam.Crm.Runtime.Session;
using Bogus;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AskPam.Crm.UnitTests
{
    public class BaseTest : IDisposable
    {
        private SqliteConnection _connection;
        protected DbContextOptions<CrmDbContext> ContextOptions;
        protected ICrmSession MockCrmSession;

        public BaseTest()
        {
            CreateConnection();
            this._connection.Open();
            ContextOptions = new DbContextOptionsBuilder<CrmDbContext>().UseSqlite(_connection)
               .Options;
            EnsureDatabaseCreated();
            TestHelper.GenerateGlobalDataTests(ContextOptions);
        }

        public void CreateConnection()
        {
            // In-memory database only exists while the connection is open
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            _connection = new SqliteConnection(connectionStringBuilder.ToString());
        }

        private void EnsureDatabaseCreated()
        {

            // Create the schema in the database if not available
            using (var context = new CrmDbContext(ContextOptions, new NullCrmSession()))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();
                context.Database.CloseConnection();
            };

        }

        public virtual void Dispose()
        {
            this._connection.Close();
        }

    }
}
