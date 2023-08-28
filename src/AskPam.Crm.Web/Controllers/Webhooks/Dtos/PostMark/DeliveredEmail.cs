using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.Conversations.Dtos.PostMark
{
    public class DeliveredEmail
    {
        public int ServerId { get; set; }
        public string MessageID { get; set; }
        public string Recipient { get; set; }
        public string Tag { get; set; }
        public string DeliveredAt { get; set; }
        public string Details { get; set; }
    }
}
