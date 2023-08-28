using AskPam.Application.Dto;

namespace AskPam.Crm.Users.Dtos
{
    public class UserListRequestDto : PagedAndSortedDto
    {
        public string Filter { get; set; }
    }
}
