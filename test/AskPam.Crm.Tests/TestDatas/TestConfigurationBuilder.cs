using System.Linq;
using AskPam.Crm.AI.Entities;
using AskPam.Crm.Configuration;
using AskPam.Crm.EntityFramework;

namespace AskPam.Crm.IntegratedTests.TestDatas
{
    public class TestConfigurationBuilder
    {
        private readonly CrmDbContext _context;

        public TestConfigurationBuilder(CrmDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            //Add some organization
            var organization = _context.Organizations.First(o => o.Name == TestOrganizationsBuilder.organization1Name);

            _context.QnAPairs.AddRange(
                new QnAPair[] {
                    new QnAPair("Question 1", "Answer 1", organization.Id),
                    new QnAPair("Question 2", "Answer 2", organization.Id),
               }
            );

            _context.Settings.AddRange(
                new Setting[] {
                    new Setting(){
                        Name ="App.AI.KnowledgeBaseId",
                        Value = "0b7b337b-35b1-49b4-a281-726d7397ed8a",
                        OrganizationId = organization.Id
                    }
               }
            );

            _context.SaveChanges();
        }
    }
}
