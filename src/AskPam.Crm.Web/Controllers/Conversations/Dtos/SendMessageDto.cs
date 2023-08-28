using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.Conversations.Dtos
{
    public class SendMessageDto
    {
        public int? ConversationId { get; set; }

        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public string Type { get; set; }
        public bool IsBodyHtml { get; set; }
        public IEnumerable<AttachmentDto> Attachments { get; set; }
        public IEnumerable<RecipientDto> Recipients { get; set; }
    }
}
