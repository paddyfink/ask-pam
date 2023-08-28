using AskPam.Crm.Tags.Dtos;
using System;
using System.Collections.Generic;

namespace AskPam.Crm.Controllers.Libraries.Dtos
{
    public class LibraryItemListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string NationalPhone { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public IEnumerable<TagDto> Tags { get; set; }
    }
}
