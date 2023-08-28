using AskPam.Crm.Tags.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AskPam.Crm.Library.Dtos
{
    public class LibraryItemDto
    {
        public long? Id { get; set; }
        [Required]
        public string Name { get; set; }            
        public string Subject { get; set; }            
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public string OpeningHours { get; set; }
        public string Menu { get; set; }
        public string Area { get; set; }
        public string Price { get; set; }
        public string Website { get; set; }
        public string Fax { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string Type { get; set; }
        public int? TypeValue { get; set; }

        //event
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsAllDay { get; set; }

        public IEnumerable<TagDto> Tags { get; set; }

    }
}
