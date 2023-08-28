using AskPam.Crm.Conversations;
using AskPam.Crm.Posts;
using AskPam.Domain.Entities;
using AskPam.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AskPam.Crm.Authorization.Followers
{
    [Table("FollowersRelations")]
    public class FollowersRelation : Entity, ICreationAudited
    {
        public string UserId { get; private set; }

        public virtual Conversation Conversation { get; set; }
        public long? ConversationId { get; private set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedById { get; set; }

        public long? PostId { get; private set; }
        public virtual Post Post { get; set; }

        internal FollowersRelation()
        {

        }
        public FollowersRelation(string userId, long conversationId)
        {
            UserId = userId;
            ConversationId = conversationId;
        }
    }
}