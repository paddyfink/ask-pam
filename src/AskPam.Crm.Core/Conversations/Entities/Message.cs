using AskPam.Crm.Organizations;
using AskPam.Crm.Common.Interfaces;
using AskPam.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using AskPam.Domain.Entities.Auditing;
using AskPam.Crm.Authorization;
using System.ComponentModel.DataAnnotations.Schema;
using AskPam.Crm.Tags;

namespace AskPam.Crm.Conversations
{
    [Table("Messages")]
    public class Message : Entity, ISoftDelete
    {
        public long ConversationId { get; set; }
        public virtual Conversation Conversation { get; set; }

        public string Author { get; set; }
        public string AuthorId { get; set; }
        public string SmoochMessageId { get; set; }
        public Guid? PostmarkId { get; set; }
        public string Avatar { get; set; }

        public string Text { get; set; }
        public bool? Seen { get; set; }
        public DateTime Date { get; set; }
        public bool IsAutomaticReply { get; set; }
        public long? ReplyTo { get; set; }
        public virtual ICollection<TagsRelation> TagsRelations { get; set; }

        [NotMapped]
        public MessageStatus Status
        {
            get => Enumeration<MessageStatus, string>.FromValue(MessageStatusId);
            set => MessageStatusId = value.Value;
        }
        [Column("Status")]
        public string MessageStatusId { get; set; }
        [NotMapped]

        public MessageType MessageType
        {
            get => Enumeration<MessageType, string>.FromValue(MessageTypeId);
            set => MessageTypeId = value.Value;
        }
        [Column("Type")]
        public string MessageTypeId{ get; set; }

        [NotMapped]
        public ChannelType ChannelType
        {
            get => Enumeration<ChannelType, string>.FromValue(ChannelTypeId);
            set => ChannelTypeId = value.Value;
        }
        [Column("Channel")]
        public string ChannelTypeId { get; set; }
        public string Subject { get; set; }
        public bool IsDeleted { get; set; }

        //Foreign Keys + Tables
        public virtual ICollection<Attachment> Attachments { get; set; }
        public virtual ICollection<DeliveryStatus> DeliveryStatus { get; set; }


        public virtual User CreatedUser { get; set; }

        public void AddAttachment(string name, string contentString, string contentType, int length)
        {
            Attachment attachment = new Attachment(Id, name, contentString, contentType, length);
            if (Attachments == null)
                Attachments = new List<Attachment>();
            Attachments.Add(attachment);
        }

    }
}
