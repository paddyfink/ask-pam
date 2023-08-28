using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.Controllers.Notifications.Dtos
{
    public static class NotificationTypeDescriptions
    {
        public const string ReplytoConversation = "{0} replied to a conversation";
        public const string BotAnswerNotFound = "The bot couldn't answered to a converaion with {0}";        
        public const string ReplytoConversationWith = "{0} replied to a conversation with {1}";
        public const string AddNoteToConveration = "{0} added a note to a conversation with {1}";
        public const string ConversationAssigned = "You have been assigned a conversation with <strong>{0}</strong> by {1}.";
        public const string ConversationFlagged = "{0} flagged a conversation with <strong>{1}</strong>.";
        public const string ContactAssigned = "Contact <strong>{0}</strong> was assigned to you";
        public const string ContactUnAssigned = "Contact <strong>{0}</strong>  was reassigned";
        public const string ConversationFollowed = "{0} added you as a follower to a conversation with <strong>{1}</strong>";
        public const string LibraryItemCreated = "A new library item <strong>{0}</strong> has been added by {1}.";
        public const string PostAdded = "A new post <strong>{0}</strong> has been added by {1}.";
        public const string PostCommented = "{0} commented on your post <strong>{0}</strong>.";
    }
}
