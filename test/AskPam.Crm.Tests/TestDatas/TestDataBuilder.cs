using AskPam.Crm.EntityFramework;

namespace AskPam.Crm.IntegratedTests.TestDatas
{
    class TestDataBuilder
    {
        private readonly CrmDbContext _context;

        public TestDataBuilder(CrmDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new TestOrganizationsBuilder(_context).Create();
            new TestRolesBuilder(_context).Create();
            new TestUsersBuilder(_context).Create();
            new TestContactsBuilder(_context).Create();
            new TestConversationsBuilder(_context).Create();
            new TestLibraryItemsBuilder(_context).Create();
            new TestConfigurationBuilder(_context).Create();
            _context.SaveChanges();
        }
    }
}
