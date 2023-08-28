using AskPam.Crm.Contacts.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.Conversations.Dtos
{
    public class ChannelDto
    {
        public long? Id { get; set; }
        public string AvatarUrl { get; set; }

        public string SmoochId { get; set; }
        public string DisplayName { get; set; }
        public string Type { get; set; }
		public long? ConversationId { get; set; }
	}
}
