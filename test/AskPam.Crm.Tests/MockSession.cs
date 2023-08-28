using System;
using AskPam.Crm.Runtime.Session;

namespace AskPam.Crm.IntegratedTests
{
    public class MockCrmSession : ICrmSession
    {
        public string UserId { get; set; }

        public Guid? OrganizationId { get; set; }

    }
}
