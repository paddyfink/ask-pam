using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.Configuration.Dtos
{
    public class EmailOrganizationSettingsDto
    {
        public string SenderSignatureId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsDkimVerified { get; set; }
        public string ForwardEmail { get; set; }
        public bool ShowPoweredByAskPam { get; set; }
    }
}
