using AskPam.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.Configuration.Dtos
{
    public class SettingsRequestDto
    {
        public Guid? OrganizationId { get; set; }
        public string UserId { get; set; }
    }
}
