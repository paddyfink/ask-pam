using System;

namespace AskPam.Crm.Hubs
{
    public class HubTrack
    {
        public string HubName { get; set; }
        public string UserId { get; set; }
        public string OrganizationId { get; set; }
        public string ConnectionId { get; set; }
        public DateTime ConnectionTime { get; set; }
    }
}