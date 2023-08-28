using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.Controllers.Configuration.Dtos
{
    public class QnAPairDto
    {
        public long Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
