using AskPam.Crm.Authorization;
using AskPam.Events;
using AskPam.Crm.Library;
using System.Collections.Generic;

namespace AskPam.Crm.Library.Events
{
    public class LibraryEvent: IEvent
    {
        public LibraryItem LibraryItem { get; set; }
    }

    public class LibraryCreatedEvent : LibraryEvent
    {
    }

}
