using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.Conversations.Dtos.PostMark
{
    public class BouncedMail
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public int TypeCode { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public string MessageID { get; set; }
        public int ServerId { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public string Email { get; set; }
        public string From { get; set; }
        public string BouncedAt { get; set; }
        public bool DumpAvailable { get; set; }
        public bool Inactive { get; set; }
        public bool CanActivate { get; set; }
        public string Subject { get; set; }
    }
}
