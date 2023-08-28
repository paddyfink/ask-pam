using AskPam.Crm.Integrations.Dtos;
using AskPam.Crm.Users.Dtos;
using System;
using System.Collections.Generic;

namespace AskPam.Crm.Organizations.Dtos
{
    public class OrganizationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public bool FullContact { get; set; }
        public bool Klik { get; set; }
        public bool BrainDates { get; set; }
        public bool Stay22 { get; set; }
        public int UsersCount { get; set; }

        public DateTime? CreatedAt { get; set; }
        
    }
}
