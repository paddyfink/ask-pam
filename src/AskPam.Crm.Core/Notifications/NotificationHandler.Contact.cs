using AskPam.Crm.Conversations;
using AskPam.Crm.Conversations.Events;
using AskPam.Events;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using AskPam.Crm.Library;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AskPam.Crm.Contacts.Events;
using AskPam.Crm.Contacts;

namespace AskPam.Crm.Notifications
{
    public partial class NotificationHandler :
        IEventHandler<ContactAssigned>,
        IEventHandler<ContactUnAssigned>
    {

        public async Task Handle(ContactAssigned contactAssigned)
        {

            var assignee = contactAssigned.Assignee;

            var notification = new Notification
            {
                EntityId = contactAssigned.Contact.Id.ToString(),
                NotificationType = NotificationTypes.ContactAssigned,
                OrganizationId = contactAssigned.Contact.OrganizationId,
                EntityType = typeof(Contact).Name,
                Data = JsonConvert.SerializeObject(new
                {
                    ContactId = contactAssigned.Contact.Id,
                    ContactName = contactAssigned.Contact.FullName
                })
            };

            notification = await _notificationManager.InsertNotificationAsync(notification);

            var userNotification = new UserNotification
            {
                UserId = assignee.Id,
                NotificationId = notification.Id,
                OrganizationId = contactAssigned.Contact.OrganizationId
            };

            await _notificationManager.InsertUserNotificationAsync(notification, userNotification);
        }

        public async Task Handle(ContactUnAssigned contactAssigned)
        {

            var assignee = contactAssigned.Assignee;

            var notification = new Notification
            {
                EntityId = contactAssigned.Contact.Id.ToString(),
                NotificationType = NotificationTypes.ContactUnAssigned,
                OrganizationId = contactAssigned.Contact.OrganizationId,
                EntityType = typeof(Contact).Name,
                Data = JsonConvert.SerializeObject(new { ContactId = contactAssigned.Contact.Id, ContactName = contactAssigned.Contact.FullName })
            };


            await _notificationManager.InsertNotificationAsync(notification);

            var userNotification = new UserNotification
            {
                UserId = assignee.Id,
                NotificationId = notification.Id,
                OrganizationId = contactAssigned.Contact.OrganizationId
            };

            await _notificationManager.InsertUserNotificationAsync(notification, userNotification);
        }
    }
}
