using AskPam.Domain.Entities;
using System;

namespace AskPam.Crm.Notifications
{
    public class UserNotification : Entity<long>
    {
        public string UserId { get; set; }
        public long NotificationId { get; set; }
        public bool Seen { get; set; }
        public bool Read { get; set; }
        public Guid OrganizationId { get; set; }
    }
}
