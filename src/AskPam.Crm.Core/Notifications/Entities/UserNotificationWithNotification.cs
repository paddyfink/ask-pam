using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.Notifications
{
   public class UserNotificationWithNotification
    {
        public UserNotification UserNotification { get; set; }
        public Notification Notification { get; set; }

        public UserNotificationWithNotification(UserNotification userNotification, Notification notification)
        {
            UserNotification = userNotification;
            Notification = notification;
        }
    }
}
