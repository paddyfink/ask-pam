using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.Configuration.Dtos
{
    public class SettingDto
    {
        public Guid? OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string UserId { get; set; }
        public string UserFullName { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
