using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AskPam.Crm.UnitTests.Controllers
{
    [CollectionDefinition(Constants.AutoMapperCollection)]
    public class AutomapperCollection : ICollectionFixture<AutoMapperFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
