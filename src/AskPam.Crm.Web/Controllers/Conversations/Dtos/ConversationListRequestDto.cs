using AskPam.Application.Dto;
using System.ComponentModel.DataAnnotations;

namespace AskPam.Crm.Controllers.Conversations.Dtos
{
    public class ConversationListRequestDto : PagedResultRequestDto
    {
        [Required]
        public string Filter { get; set; }
        public int? ContactId { get; set; }
        public string Search { get; set; }
    }
}
