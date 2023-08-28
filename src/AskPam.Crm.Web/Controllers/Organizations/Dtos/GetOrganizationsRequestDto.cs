using AskPam.Application.Dto;

namespace AskPam.Crm.Organizations.Dtos
{
    public class GetOrganizationsRequestDto : PagedAndSortedDto
    {
        public string Filter { get; set; }
    }
}
