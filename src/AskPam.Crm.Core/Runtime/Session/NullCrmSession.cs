using System;
using System.Collections.Generic;
using System.Text;
using AskPam.Crm.Authorization;
using AskPam.Crm.Organizations;

namespace AskPam.Crm.Runtime.Session
{
    public class NullCrmSession : ICrmSession
    {
        public string UserId => string.Empty;

        public Guid? OrganizationId => null;

        public User User => null;

        public Organization Organization => null;
    }
}
