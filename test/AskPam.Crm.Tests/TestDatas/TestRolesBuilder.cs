using System.Linq;
using AskPam.Crm.Authorization;
using AskPam.Crm.EntityFramework;

namespace AskPam.Crm.IntegratedTests.TestDatas
{
    public class TestRolesBuilder
    {
       

        private readonly CrmDbContext _context;

        
        public TestRolesBuilder(CrmDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            //Add some organization
            var organization = _context.Organizations.First(o => o.Name == TestOrganizationsBuilder.organization1Name);
            var organization2 = _context.Organizations.First(o => o.Name == TestOrganizationsBuilder.organization2Name);

            //Add some Users
            _context.Roles.AddRange(
                new Role[] {
                    new Role(organization.Id,  RolesName.Admin,  RolesName.Admin),
                    new Role(organization.Id,  RolesName.User,  RolesName.User),
                    new Role(organization.Id,  RolesName.Reader,  RolesName.Reader),

                    new Role(organization2.Id,  RolesName.Admin,  RolesName.Admin)
                }
            );

            _context.SaveChanges();
        }
    }
}
