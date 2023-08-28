using AskPam.Crm.Authorization;
using AskPam.Events;
using AskPam.Crm.InternalNotes;
using AskPam.Crm.Library;
using System.Collections.Generic;

namespace AskPam.Crm.Posts.Events
{
    public class PostEvent: IEvent
    {
        public Post Post { get; set; }
    }

    public class PostCreated : PostEvent
    {
    }

    public class PostUpdated : PostEvent
    {
    }

    public class PostCommented : PostEvent
    {
        public InternalNote Note { get; set; }
    }

}
