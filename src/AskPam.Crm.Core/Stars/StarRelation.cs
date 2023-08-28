using AskPam.Crm.Contacts;
using AskPam.Crm.Conversations;
using AskPam.Domain.Entities;
using AskPam.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AskPam.Crm.Stars
{
    [Table("StarsRelations")]
    public class StarsRelation : Entity<long>, ICreationAudited
    {
        public string UserId { get; private set; }

        public virtual Conversation Conversation { get; set; }
        public long? ConversationId { get; private set; }

        public virtual Contact Contact { get; set; }
        public long? ContactId { get; private set; }

        public DateTime? CreatedAt { get; set; }
        public string CreatedById { get; set; }

        internal StarsRelation()
        {
        }

        public StarsRelation(string userId, long? conversationId = null, long? contactId = null)
        {
            UserId = userId;
            ConversationId = conversationId;
            ContactId = contactId;
        }
    }
}
