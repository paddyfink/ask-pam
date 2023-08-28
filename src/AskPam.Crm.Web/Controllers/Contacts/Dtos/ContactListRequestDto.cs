using AskPam.Application.Dto;

namespace AskPam.Crm.Contacts.Dtos
{
    public class ContactListRequestDto : PagedAndSortedDto
    {
        public string Filter { get; set; }
        public int? GroupId { get; set; }
        public string UserId { get; set; }
        public bool? WithConversation { get; set; }
    }
}
