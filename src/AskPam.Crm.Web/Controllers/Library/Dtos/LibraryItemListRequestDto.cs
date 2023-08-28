using AskPam.Application.Dto;

namespace AskPam.Crm.Controllers.Libraries.Dtos
{
    public class LibraryItemListRequestDto : PagedAndSortedDto
    {
        public string Filter { get; set; }
        public int? LibraryTypeId { get; set; }
    }
}
