using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.Conversations.Dtos
{
    public class AttachmentDto
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public int ContentLength { get; set; }
        public string Content { get; set; }
    }
}
