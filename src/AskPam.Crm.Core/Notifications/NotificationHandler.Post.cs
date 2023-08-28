using AskPam.Events;
using Newtonsoft.Json;
using AskPam.Crm.Library;
using System.Linq;
using System.Threading.Tasks;
using AskPam.Crm.Posts.Events;
using AskPam.Crm.Posts;
using Microsoft.EntityFrameworkCore;

namespace AskPam.Crm.Notifications
{
    public partial class NotificationHandler :
        IEventHandler<PostCreated>,
        IEventHandler<PostCommented>
    {

        public async Task Handle(PostCreated @event)
        {
            var creator = await _userManager.FindByIdAsync(@event.Post.CreatedById);

            var notification = new Notification
            {
                EntityId = @event.Post.Id.ToString(),
                NotificationType = NotificationTypes.PostAdded,
                OrganizationId = @event.Post.OrganizationId,
                EntityType = typeof(Post).Name,
                Data = JsonConvert.SerializeObject(new
                {
                    CreatorId = creator.Id,
                    CreatorName = creator.FullName,
                    Title = @event.Post.Title
                })
            };
            notification = await _notificationManager.InsertNotificationAsync(notification);

            var users = await _userManager.GetAllUsers(@event.Post.OrganizationId)
                .Where(u => u.Id != creator.Id)
                .ToListAsync();

            foreach (var user in users)
            {
                var userNotification = new UserNotification
                {
                    UserId = user.Id,
                    NotificationId = notification.Id,
                    OrganizationId = @event.Post.OrganizationId
                };

                userNotification = await _notificationManager.InsertUserNotificationAsync(notification, userNotification);
                               
            }
        }

        public async Task Handle(PostCommented @event)
        {
            var commentor = await _userManager.FindByIdAsync(@event.Note.CreatedById);
            var user = await _userManager.FindByIdAsync(@event.Post.CreatedById);

            var notification = new Notification
            {
                EntityId = @event.Post.Id.ToString(),
                NotificationType = NotificationTypes.PostCommented,
                OrganizationId = @event.Post.OrganizationId,
                EntityType = typeof(Post).Name,
                Data = JsonConvert.SerializeObject(new
                {
                    CommentorId = @event.Note.CreatedById,
                    CommentorName = commentor.FullName,
                    PostTitle = @event.Post.Title,
                    Comment = @event.Note.Comment
                })
            };
            notification = await _notificationManager.InsertNotificationAsync(notification);

            var userNotification = new UserNotification
            {
                UserId = @event.Post.CreatedById,
                NotificationId = notification.Id,
                OrganizationId = @event.Post.OrganizationId
            };

            userNotification = await _notificationManager.InsertUserNotificationAsync(notification, userNotification);

        }
    }
}
