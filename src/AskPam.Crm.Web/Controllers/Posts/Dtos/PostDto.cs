using AskPam.Application.Dto;
using AskPam.Crm.Tags.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.Posts.Dtos
{
    public class PostDto : FullAuditedDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CreatedUserFullName { get; set; }
        public string CreatedUserPicture { get; set; }
        public int NotesCount { get; set; }
        public IEnumerable<TagDto> Tags { get; set; }
    }
}
