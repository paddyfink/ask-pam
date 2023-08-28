using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.Conversations.Dtos
{
    public class RecipientDto
    {
        public string Name { get; set; }
        public string Recipient { get; set; }
        public string ChannelType { get; set; }
        public long ContactId { get; set; }
        public int ConversationsCount { get; set; }
    }
}
