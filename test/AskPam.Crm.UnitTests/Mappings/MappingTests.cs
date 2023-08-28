
using Xunit;
using AutoMapper;
using AskPam.Crm.Contacts;
using AskPam.Crm.Authorization.Users;
using AskPam.Crm.Controllers.Common;
using AskPam.Crm.Conversations;
using AskPam.Crm.Library;
using AskPam.Crm.Organizations;
using AutoMapper.Configuration;

namespace AskPam.Crm.UnitTests.Mappings
{
    [Collection(Constants.AutoMapperCollection)]
    public class MappingTests
    {
        [Fact]
        public void AutoMapper_Configuration_IsValid()
        {
    
            Mapper.AssertConfigurationIsValid();

        }

       
    }


}


