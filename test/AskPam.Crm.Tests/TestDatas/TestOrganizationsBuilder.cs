using AskPam.Crm.EntityFramework;
using AskPam.Crm.Organizations;

namespace AskPam.Crm.IntegratedTests.TestDatas
{
    public class TestOrganizationsBuilder
    {

        public const string organization1Name = "Organization1";
        public const string organization2Name = "Organization2";
        public const string organization3Name = "Organization2";

        private readonly CrmDbContext _context;

        public TestOrganizationsBuilder(CrmDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            //Add some organization
            _context.Organizations.AddRange(
                new Organization[] {
                    new Organization(organization1Name),
                    new Organization(organization2Name),
                    new Organization(organization3Name )
                }
            );

            _context.SaveChanges();
        }
    }
}
