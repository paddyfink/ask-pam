using AskPam.Crm.Organizations.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.Authorization.Dtos
{
    public class AccountInfo
    {
        public ProfileDto Profile { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public IEnumerable<OrganizationDto> Organizations { get; set; }
    }
}
