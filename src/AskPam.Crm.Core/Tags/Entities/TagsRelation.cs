using AskPam.Crm.Contacts;
using AskPam.Crm.Conversations;
using AskPam.Crm.Library;
using AskPam.Crm.Posts;
using AskPam.Domain.Entities;
using AskPam.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AskPam.Crm.Tags
{
    [Table("TagsRelations")]
    public class TagsRelation : Entity, ICreationAudited
    {
        public long TagId { get; private set; }
        public virtual Tag Tag { get; set; }

        public virtual Contact Contact { get; set; }
        public long? ContactId { get; private set; }

        public virtual LibraryItem Library { get; set; }
        public long? LibraryItemId { get; private set; }

        public virtual Conversation Conversation { get; set; }
        public  long? ConversationId { get; private set; }

        public virtual Message Message { get; set; }
        public long? MessageId { get; private set; }

        public virtual Post Post { get; set; }
        public long? PostId { get; private set; }


        public DateTime? CreatedAt { get; set; }
        public string CreatedById { get; set; }

        internal TagsRelation()
        {

        }
        public TagsRelation(long tagId, long? contactId = null, long? libraryItemId = null, long? conversationId = null, long? postId =null, long? messageId = null)
        {
            TagId = tagId;

            if (contactId.HasValue)
                ContactId = contactId;

            if (libraryItemId.HasValue)
                LibraryItemId = libraryItemId;

            if (postId.HasValue)
                PostId = postId;

            if (conversationId.HasValue)
                ConversationId = conversationId;

            if (messageId.HasValue)
                MessageId = messageId;
        }


    }
}