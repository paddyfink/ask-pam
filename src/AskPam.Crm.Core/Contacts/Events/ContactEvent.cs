using AskPam.Crm.Authorization;
using AskPam.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.Contacts.Events
{
    public class ContactEvent : IEvent
    {
        public Contact Contact { get; set; }
    }

    public class ContactCreated : ContactEvent
    { }

    public class ContactAssigned : ContactEvent
    {
        public User Assigner { get; set; }
        public User Assignee { get; set; }
    }

    public class ContactUnAssigned : ContactEvent
    {
        public User Assigner { get; set; }
        public User Assignee { get; set; }
    }

    public class ContactDeleted : ContactEvent
    { }
}
