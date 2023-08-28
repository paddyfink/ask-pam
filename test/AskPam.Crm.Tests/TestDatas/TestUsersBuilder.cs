using System.Linq;
using AskPam.Crm.Authorization;
using AskPam.Crm.EntityFramework;

namespace AskPam.Crm.IntegratedTests.TestDatas
{
    public class TestUsersBuilder
    {
        public const string adminId = "auth0|588b7ec5aa03fb78b0e55ca5";
        public const string user1Id = "auth0|588b828504a2bd7e716dcdcf";
        public const string user2Id = "auth0|588b82b8b422f97d3cbbbeb4";
        public const string user3Id = "auth0|588b82d204a2bd7e716dcde3";
        public const string user4Id = "auth0|588b82f6aa03fb78b0e55e10";
        public const string user5Id = "auth0|588b8325aa03fb78b0e55e26";

        public const string adminEmail = "admin@ask-pam.com";
        public const string user1Email = "user1@ask-pam.com";
        public const string user2Email = "user2@ask-pam.com";
        public const string user3Email = "user3@ask-pam.com";
        public const string user4Email = "user4@ask-pam.com";
        public const string user5Email = "user5@ask-pam.com";

        public const string userPassword = "123qwe";

        private readonly CrmDbContext _context;


        public TestUsersBuilder(CrmDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            //Add some Users
            _context.Users.AddRange(
                new User[] {
                    new User(adminId, "John", "Doe", adminEmail, "", "", ""),
                    new User(user1Id, "Peter", "Pan", user1Email, "", "", ""),
                    new User(user2Id, "Alice", "Doe", user2Email, "", "", ""),
                    new User(user3Id, "Lana", "Doe", user3Email, "", "", ""),
                    new User(user4Id, "Lana", "Doe", user4Email, "", "", ""),
                    //new User(user5Id, "Lana", "Doe", user5Email, "", "", "")
                }
            );

            //Add users to organization
            var organization1 = _context.Organizations.Where(o => o.Name == TestOrganizationsBuilder.organization1Name).First();
            var organization2 = _context.Organizations.Where(o => o.Name == TestOrganizationsBuilder.organization2Name).First();

            _context.UserOrganizations.AddRange(
                new UserOrganization[] {
                    new UserOrganization { OrganizationId = organization1.Id, UserId = user1Id },
                    new UserOrganization { OrganizationId = organization1.Id, UserId = user2Id },
                    new UserOrganization { OrganizationId = organization1.Id, UserId = user3Id },
                    new UserOrganization { OrganizationId = organization2.Id, UserId = user4Id },
                    //new UserOrganization { OrganizationId = organization2.Id, UserId = user5Id },
                }
            );

            var adminrole = _context.Roles.First(o => o.Name == RolesName.Admin && o.OrganizationId== organization1.Id);
            var userRole = _context.Roles.First(o => o.Name == RolesName.User && o.OrganizationId == organization1.Id);
            var readerRole = _context.Roles.First(o => o.Name == RolesName.Reader && o.OrganizationId == organization1.Id);

            var adminRole2 = _context.Roles.First(o => o.Name == RolesName.Admin && o.OrganizationId == organization2.Id);

            //Add some UserRoles
            _context.UserRoles.AddRange(
                new UserRole[] {
                    new UserRole(){ OrganizationId = organization1.Id , RoleId = adminrole.Id, UserId = user1Id },
                    new UserRole(){ OrganizationId = organization1.Id , RoleId = userRole.Id, UserId = user2Id },
                    new UserRole(){ OrganizationId = organization1.Id , RoleId = readerRole.Id, UserId = user3Id },

                     new UserRole(){ OrganizationId = organization2.Id , RoleId = adminRole2.Id, UserId = user4Id },
                }
            );
            _context.SaveChanges();
        }
    }
}
