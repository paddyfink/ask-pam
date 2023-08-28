using AskPam.Crm.Authorization;
using AskPam.Crm.Organizations;
using AskPam.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AskPam.Domain.Services;
using AskPam.Events;
using AskPam.Exceptions;
using Z.EntityFramework.Plus;
using AskPam.EntityFramework.Repositories;

namespace AskPam.Crm.Notifications
{
    public class NotificationManager : INotificationManager
    {
        private readonly IRepository<Notification, long> _notificationRepository;
        private readonly IRepository<UserNotification, long> _userNotificationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDomainEvents _domainEvents;
        private readonly IUserManager _userManager;

        public NotificationManager(
            IUserManager userManager,
             IUnitOfWork unitOfWork,
            IDomainEvents domainEvents, IRepository<Notification, long> notificationRepository, IRepository<UserNotification, long> userNotificationRepository)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
          
            _domainEvents = domainEvents;
            _notificationRepository = notificationRepository;
            _userNotificationRepository = userNotificationRepository;
        }

        public async Task<List<UserNotificationWithNotification>> GetUserNotifications(User user, Organization organization, bool? read, int? skipCount, int? maxResultCount)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (organization == null)
            {
                throw new ArgumentNullException(nameof(organization));
            }

            var result = await _notificationRepository.GetAll()
                .Join(
                    _userNotificationRepository.GetAll(),
                    (n) => n.Id,
                    (un) => un.NotificationId,
                    (n, un) => new { UserNotification = un, Notification = n }
                )
                .Where(r => r.UserNotification.UserId.Equals(user.Id))
                .Where(r => r.Notification.OrganizationId.Equals(organization.Id))
                .Where(r => (read == null || r.UserNotification.Read == read))
                .Skip(skipCount ?? 0)
                .Take(maxResultCount ?? int.MaxValue)
                .OrderByDescending(r => r.Notification.CreatedAt)
                .ToListAsync();

            return result.Select(a => new UserNotificationWithNotification(a.UserNotification, a.Notification)).ToList();
        }

        public async Task<int> GetUserUnreadNotificationCountAsync(User user, Organization organization)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (organization == null)
            {
                throw new ArgumentNullException(nameof(organization));
            }

            return await _userNotificationRepository.GetAll()
                .CountAsync(n => n.UserId == user.Id 
                    && n.OrganizationId == organization.Id 
                    &&  n.Read == false
                );
        }

        public async Task<int> GetUserUnSeenNotificationCountAsync(User user, Organization organization)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (organization == null)
            {
                throw new ArgumentNullException(nameof(organization));
            }

            return await _userNotificationRepository.GetAll()
                .CountAsync(n => n.UserId == user.Id
                    && n.OrganizationId == organization.Id
                    &&  n.Seen == false
                );
        }

        public async Task DeleteNotificationAsync(Notification notification)
        {
            await _notificationRepository.DeleteAsync(notification);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Notification> InsertNotificationAsync(Notification notification)
        {
            notification = await _notificationRepository.InsertAsync(notification);
            await _unitOfWork.SaveChangesAsync();
            return notification;
        }

        public async Task<UserNotification> InsertUserNotificationAsync(Notification notification, UserNotification userNotification)
        {
            userNotification = await _userNotificationRepository.InsertAsync(userNotification);
            await _unitOfWork.SaveChangesAsync();
            
            var user = await _userManager.FindByIdAsync(userNotification.UserId);

            await _domainEvents.RaiseAsync(
                new UserNotificationCreatedEvent()
                {
                    Notification = notification,
                    UserNotification = userNotification,
                    User = user
                }
            );

            return userNotification;
        }

        public async Task UpdateNotificationStateAsync(long notificationId, bool state, string userId, Guid organizationId)
        {
            var notification = await _userNotificationRepository.GetAll()
                .Where(n => n.OrganizationId == organizationId)
                .Where(n => n.NotificationId == notificationId)
                .Where(n => n.UserId == userId)
                .Join(
                    _notificationRepository.GetAll(),
                    (un) => un.NotificationId,
                    (n) => n.Id,
                    (un, n) => new { Notification = n, UserNotification = un }
                )
                .FirstOrDefaultAsync();

            if (notification == null)
            {
                return;
            }

            notification.UserNotification.Read = state;
            await _userNotificationRepository.UpdateAsync(notification.UserNotification);
            await _unitOfWork.SaveChangesAsync();

            if (notification.Notification.NotificationType == NotificationTypes.NewMessage)
            {
                var notifications = await _userNotificationRepository.GetAll()
               .Where(n => n.OrganizationId == organizationId)
               .Where(n => n.UserId == userId)
               .Join(
                   _notificationRepository.GetAll()
                   .Where(n => n.NotificationType == NotificationTypes.NewMessage && n.EntityId == notification.Notification.EntityId && n.EntityType == notification.Notification.EntityType),
                   (un) => un.NotificationId,
                   (n) => n.Id,
                   (un, n) => un
               ).ToListAsync();

                foreach (var un in notifications)
                {
                    un.Read = true;
                    await _userNotificationRepository.UpdateAsync(un);
                }
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task MarkAllNotificationsAsSeen(string userId, Guid organizationId)
        {           
           await _userNotificationRepository.GetAll()
                .Where(n => n.OrganizationId == organizationId)
                .Where(n => n.UserId == userId)
                .Where(n => n.Seen == false)
               .UpdateAsync(x => new UserNotification() { Seen = true });
            
            await _unitOfWork.SaveChangesAsync();
        }
            public IQueryable<Notification> GetNotifications(Guid organizationId)
        {
            return _notificationRepository.GetAll()
                .Where(n => n.OrganizationId == organizationId);
        }

        //public async Task<Notification> FindNotificationById(long id, Guid organizationId)
        //{
        //    var result = await GetNotifications(organizationId)
        //        .Where(c => c.Id == id)
        //        .FirstOrDefaultAsync();

        //    if (result == null)
        //    {
        //        throw new ApiException("Notification not found.", 404);
        //    }

        //    return result;
        //}
    }
}
