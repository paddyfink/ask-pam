using AskPam.Crm.Authorization;
using AskPam.Crm.EntityFramework;
using AutoMapper;
using Xunit;

namespace AskPam.Crm.UnitTests.Controllers
{
    [Collection(Constants.AutoMapperCollection)]
    public class BaseControllerTest : BaseTest
    {

        protected IMapper MockMapper;
        protected User CurrentUser;
        protected CrmDbContext Context { get; set; }


        public BaseControllerTest() 
        {
            MockMapper = Mapper.Instance;
        }

        public override void Dispose()
        {
            Context.Dispose();
            base.Dispose();
        }
    }
}
