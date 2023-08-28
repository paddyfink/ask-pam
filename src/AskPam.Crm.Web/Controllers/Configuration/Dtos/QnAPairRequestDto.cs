using AskPam.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.Controllers.Configuration.Dtos
{
    public class QnAPairRequestDto : PagedAndSortedDto
    {
        public string Filter { get; set; }
    }
}
