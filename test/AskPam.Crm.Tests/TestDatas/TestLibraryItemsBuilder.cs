using System.Linq;
using AskPam.Crm.EntityFramework;
using AskPam.Crm.Library;

namespace AskPam.Crm.IntegratedTests.TestDatas
{
    public class TestLibraryItemsBuilder
    {
        private readonly CrmDbContext _context;

        public TestLibraryItemsBuilder(CrmDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            //Add some organization
            var organization = _context.Organizations.First(o => o.Name == TestOrganizationsBuilder.organization1Name);

            _context.LibraryItems.AddRange(
                new LibraryItem[] {
                    new LibraryItem("Bicycle", organization.Id),
                    new LibraryItem("Kombucha", organization.Id),
               }
            );

            _context.SaveChanges();
        }
    }
}
