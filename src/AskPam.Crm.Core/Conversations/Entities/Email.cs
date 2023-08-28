using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.Conversations
{
   public class Email : Message
    {
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string From { get; set; }
       
        public string Header { get; set; }

        public string HtmlBody { get; set; }
        public string TextBody { get; set; }
        public bool IsBodyHtml { get; set; }
        public string Thread { get; set; }
        public bool IsReplied { get; set; }
        public Email() : base()
        {

        }
        //public virtual ICollection<Attachment> Attachments { get; set; }
    }
}
