using AskPam.Crm.Common.Interfaces;
using AskPam.Crm.Contacts;
using AskPam.Crm.Organizations;
using AskPam.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AskPam.Crm.Conversations
{
    [Table("Channels")]
    public class Channel : Entity
    {
        public Channel()
        {

        }

        public string AvatarUrl { get; set; }
        public string DisplayName { get; set; }
        public DateTime? LastSeen { get; set; }
        public bool Active { get; set; }
        [NotMapped]
        public ChannelType Type
        {
            get => Enumeration<ChannelType, string>.FromValue(TypeId);
            set => TypeId = value.Value;
        }
        [Column("Type")]
        public string TypeId { get; set; }

        public long? ConversationId{ get; set; }
        public virtual Conversation Conversation{ get; set; }
      
        public string SmoochId { get; set; }
        public bool Primary { get; set; }
    }
}
