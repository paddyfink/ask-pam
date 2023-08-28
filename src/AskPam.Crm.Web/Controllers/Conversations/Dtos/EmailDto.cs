using AskPam.Crm.Conversations.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.Controllers.Conversations.Dtos
{
    public class EmailDto :MessageDto
    {
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Header { get; set; }

        public string HtmlBody { get; set; }
        public string TextBody { get; set; }
        public string Thread { get; set; }
        public bool IsReplied { get; set; }
        public bool IsBodyHtml { get; set; }


    }
}
