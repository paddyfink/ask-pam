using AskPam.Domain.Entities;
using AskPam.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.Conversations
{
    [Table("Attachments")]
    public class Attachment : Entity, ICreationAudited
    {
        public string Url { get; private set; }
        public string Name { get; private set; }
        public byte[] Content { get; set; }
        [NotMapped]
        public string ContentString { get; set; }
        public string ContentType { get; set; }
        public int ContentLength { get; set; }

        //Settings
        public DateTime? CreatedAt { get; set; }
        public string CreatedById { get; set; }

        //Foreign Key + Table
        public virtual Message Message { get; set; }
        public long MessageId { get; private set; }


        internal Attachment()
        {

        }

        public Attachment(long messageId, string name, string contentString, string contentType, int length)
        {
            MessageId = messageId;
            Name = name;
            ContentString = contentString;
            ContentType = contentType;
            ContentLength = length;
        }
        public void UpdateUrl(string url)
        {
            Url = url;
        }
    }
}
