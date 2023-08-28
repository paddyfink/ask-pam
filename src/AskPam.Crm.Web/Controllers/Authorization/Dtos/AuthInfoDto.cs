using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.Authorization.Dtos
{
    public class AuthInfoDto
    {
        public string IdToken { get; set; }
        public Guid? OrganizationId { get; set; }
       
    }
}
