using AskPam.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.Organizations.events
{
    public class OrganizationEvent : IEvent
    {
        public Organization Orgnization { get; set; }
    }

    public class OrganizationCreated : OrganizationEvent
    {
       
    }

    public class SmoochAppTokenGenerated : OrganizationEvent
    {
        public string Token { get; set; }
    }
}
