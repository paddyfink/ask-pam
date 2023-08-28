using AskPam.Crm.Authorization;
using AskPam.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.Notifications
{
    public class NotificationEvent : IEvent
    {
        public Notification Notification { get; set; }
    }

    public class UserNotificationCreatedEvent : NotificationEvent
    {
        public UserNotification UserNotification { get; set; }
        public User User { get; set; }
    }
}
