using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.Integrations.Dtos
{
    public class IntegrationDto
    {
        public Guid OrganizationId { get; set; }

        public string Name { get; set; }
        public string ChannelType { get; set; }
        public int ChannelTypeId { get; set; }

        public string Username { get; set; }
        public string Token { get; set; }
        public string Secret { get; set; }
        public string State { get; set; }

        public int PostmarkSenderId { get; set; }
        public bool PostmarkIsSpfVerified { get; set; }
        public bool PostmarkIsDkimVerified { get; set; }
        public bool IsActive { get; set; }
    }
}
