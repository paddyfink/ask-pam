using AskPam.Events;
using Newtonsoft.Json;
using AskPam.Crm.Library;
using System.Linq;
using System.Threading.Tasks;
using AskPam.Crm.Library.Events;
using Microsoft.EntityFrameworkCore;

namespace AskPam.Crm.Notifications
{
    public partial class NotificationHandler : 
        IEventHandler<LibraryCreatedEvent>
    {

        public async Task Handle(LibraryCreatedEvent libraryCreated)
        {
            var creator = await _userManager.FindByIdAsync(libraryCreated.LibraryItem.CreatedById);

            var notification = new Notification
            {
                EntityId = libraryCreated.LibraryItem.Id.ToString(),
                NotificationType = NotificationTypes.LibraryItemCreated,
                OrganizationId = libraryCreated.LibraryItem.OrganizationId,
                EntityType = typeof(LibraryItem).Name,
                Data = JsonConvert.SerializeObject(new {
                    CreatorId = creator.Id,
                    CreatorName = creator.FullName,
                    Name = libraryCreated.LibraryItem.Name
                })
            };
            notification = await _notificationManager.InsertNotificationAsync(notification);

            var users = await _userManager.GetAllUsers(libraryCreated.LibraryItem.OrganizationId)
                .Where(u => u.Id != creator.Id)
                .ToListAsync();

            foreach (var user in users)
            {
                var userNotification = new UserNotification
                {
                    UserId = user.Id,
                    NotificationId = notification.Id,
                    OrganizationId = libraryCreated.LibraryItem.OrganizationId
                };

                userNotification = await _notificationManager.InsertUserNotificationAsync(notification, userNotification);
                
            }
        }
    }
}
