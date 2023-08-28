using AskPam.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.Posts.Dtos
{
    public class PostListRequestDto : PagedAndSortedDto
    {
        public string Filter { get; set; }
    }
}
