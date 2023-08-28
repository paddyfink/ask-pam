using AskPam.Crm.Authorization;
using AskPam.Crm.Common.Interfaces;
using AskPam.Domain.Entities;
using AskPam.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.Notifications
{
    public class Notification : Entity<long>, IMustHaveOrganization, ICreationAudited
    {
        public Guid OrganizationId { get; set; }
        public string EntityType { get; set; }
        //See Notification list types in AskPam.Crm.Notifications.NotificationTypes
        public string NotificationType { get; set; }

        public string EntityId { get; set; }
        public string Data { get; set; }
        public string CreatedById { get; set; }
        public virtual User CreatedUser { get; set; }
        public DateTime? CreatedAt { get; set; }

        public ICollection<UserNotification> Users { get; set; }
    }
}
