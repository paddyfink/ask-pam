using System.Linq;
using AskPam.Crm.Contacts;
using AskPam.Crm.EntityFramework;

namespace AskPam.Crm.IntegratedTests.TestDatas
{
    public class TestContactsBuilder
    {
        private readonly CrmDbContext _context;

        public TestContactsBuilder(CrmDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            //Add some organization
            var organization = _context.Organizations.First(o => o.Name == TestOrganizationsBuilder.organization1Name);

            _context.ContactGroups.AddRange(
                new ContactGroup[] {
                    new ContactGroup("Group 2", organization.Id),
                    new ContactGroup("Group 1", organization.Id),
                    new ContactGroup("Group 3", organization.Id),
               }
            );

            _context.Contacts.AddRange(
                new Contact[] {
                    new Contact("Contact", "Un", organization.Id),
                    new Contact("Contact", "Deux", organization.Id),
                    new Contact("Contact", "Trois", organization.Id),
                    new Contact("Contact", "Quatre", organization.Id),
                    new Contact("Contact", "Cinq", organization.Id),
               }
            );

            _context.SaveChanges();
        }
    }
}
