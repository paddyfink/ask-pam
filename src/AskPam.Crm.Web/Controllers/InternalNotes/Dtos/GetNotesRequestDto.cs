using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.InternalNotes.Dtos
{
    public class GetNotesRequestDto
    {
        public int? ContactId { get; set; }
        public int? PostId { get; set; }
    }
}
