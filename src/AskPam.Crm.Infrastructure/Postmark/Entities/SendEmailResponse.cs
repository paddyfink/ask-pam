using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.Postmark.Entities
{
    public class SendEmailResponse
    {
        public string To { get; set; }
        public string SubmittedAt { get; set; }
        public string MessageID { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }
}
