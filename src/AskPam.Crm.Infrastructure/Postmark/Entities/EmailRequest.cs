using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.Postmark.Entities
{

    public class EmailRequest
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string Tag { get; set; }
        public string HtmlBody { get; set; }
        public string TextBody { get; set; }
        public string ReplyTo { get; set; }
        public List<HeaderRequest> Headers { get; set; }
        public bool TrackOpens { get; set; }
        public string TrackLinks { get; set; }
        public List<AttachmentRequest> Attachments { get; set; }
    }
}
