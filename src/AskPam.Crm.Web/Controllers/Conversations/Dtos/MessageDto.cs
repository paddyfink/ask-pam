using AskPam.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AskPam.Crm.Tags.Dtos;

namespace AskPam.Crm.Conversations.Dtos
{
    public class MessageDto 
    {
        public int? Id { get; set; }
        public int ConversationId { get; set; }
        public string Text { get; set; }
        public bool Seen { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public int AttachmentsCount { get; set; }
        
        public string ChannelType { get; set; }
        public string Type { get; set; }
        public string Author { get; set; }
        public string AuthorId { get; set; }
        public string Avatar { get; set; }
        public IEnumerable<AttachmentDto> Attachments { get; set; }
        public IEnumerable<DeliveryStatusDto> DeliveryStatus { get; set; }
        public IEnumerable<TagDto> Tags { get; set; }
    }
}
