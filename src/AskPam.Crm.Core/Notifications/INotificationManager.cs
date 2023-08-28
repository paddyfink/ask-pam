using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AskPam.Crm.Authorization;
using AskPam.Crm.Organizations;
using AskPam.Domain.Services;

namespace AskPam.Crm.Notifications
{
    public interface INotificationManager: IDomainService
    {
        Task DeleteNotificationAsync(Notification notification);
        IQueryable<Notification> GetNotifications(Guid organizationId);
        Task<List<UserNotificationWithNotification>> GetUserNotifications(User user, Organization organization, bool? read, int? skipCount, int? maxResultCount);
        Task<int> GetUserUnreadNotificationCountAsync(User user, Organization organization);
        Task<int> GetUserUnSeenNotificationCountAsync(User user, Organization organization);
        Task<Notification> InsertNotificationAsync(Notification notification);
        Task<UserNotification> InsertUserNotificationAsync(Notification notification, UserNotification userNotification);
        Task MarkAllNotificationsAsSeen(string userId, Guid organizationId);
        Task UpdateNotificationStateAsync(long notificationId, bool state, string userId, Guid organizationId);
    }
}