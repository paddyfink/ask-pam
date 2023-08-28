using AskPam.Crm.Authorization;
using AskPam.Crm.Authorization.Followers;
using AskPam.Events;
using System.Collections.Generic;

namespace AskPam.Crm.Conversations.Events
{
    public class ConversationEvent: IEvent
    {
        public Conversation Conversation { get; set; }
        public User User { get; set; }
    }

    public class ConversationCreatedEvent : ConversationEvent
    {
        public Message Message { get; set; }
    }

    public class ConversationFlaggedEvent : ConversationEvent
    {
       
    }

    public class ConversationBotActivatedEvent : ConversationEvent
    {
    }


    public class ConversationArchivedEvent : ConversationEvent
    {

    }


    public class ConversationUnarchivedEvent : ConversationEvent
    {

    }

    public class ConversationDeletedEvent : ConversationEvent
    {

    }

    public class ConversationAssignedEvent : ConversationEvent
    {
         public User Assigner { get; set; }
         public User Assignee { get; set; }
    }

    public class ConversationBotAnswerNotFoundEvent : ConversationEvent
    {
        public Message Message { get; set; }
    }

    


    public class ConversationReadEvent : ConversationEvent
    {

    }
    public class ConversationFollowedEvent : ConversationEvent
    {
        public FollowersRelation FollowersRelation { get; set; }
    }

    public class MessageReceivedEvent : ConversationEvent
    {
        public Message Message { get; set; }
    }

    public class MessageSentEvent : ConversationEvent
    {
        public Message Message { get; set; }
        public User User { get; set; }
    }

}
