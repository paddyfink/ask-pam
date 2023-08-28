using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.Postmark.Entities
{
    public class AttachmentRequest
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public string ContentType { get; set; }
    }
}
