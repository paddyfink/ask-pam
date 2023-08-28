using AskPam.Crm.Runtime.Session;
using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.UnitTests
{
    public class MockCrmSession : ICrmSession
    {
        public string UserId { get; set; }

        public Guid? OrganizationId { get; set; }

    }
}
