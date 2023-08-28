using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.Conversations.Dtos
{
    public class DeliveryStatusDto
    {
        public bool Success { get; set; }
        public bool Open { get; set; }
        public string ChannelType { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
