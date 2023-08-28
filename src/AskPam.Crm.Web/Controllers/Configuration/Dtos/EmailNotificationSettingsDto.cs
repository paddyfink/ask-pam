using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.Controllers.Configuration.Dtos
{
    public class EmailNotificationSettingsDto
    {
        [Required]
        public bool NewMessage { get; set; }
        [Required]
        public bool NewConversation { get; set; }        
        [Required]
        public bool MessageSent { get; set; }
        [Required]
        public bool ConversationAssigned { get; set; }
        [Required]
        public bool ContactAssigned { get; set; }
        [Required]
        public bool ConversationFollowed { get; set; }
        [Required]
        public bool ConversationFlagged { get; set; }
        [Required]
        public bool LibraryItemCreated { get; set; }
    }
}
