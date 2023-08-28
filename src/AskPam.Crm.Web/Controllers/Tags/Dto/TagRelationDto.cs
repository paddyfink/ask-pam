using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.Tags.Dto
{
    public class TagRelationDto
    {
        public long TagId { get; set; }
        public long? ContactId { get; set; }
        public long? LibraryId { get; set; }
        public long? ConversationId { get; set; }
        public int? MessageId { get; set; }
    }
}
