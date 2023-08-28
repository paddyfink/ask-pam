using AskPam.Application.Dto;

namespace AskPam.Crm.Tags.Dtos
{
    public class TagListRequestDto : PagedAndSortedDto
    {
        public string Filter { get; set; }
    }
}
