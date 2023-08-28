using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AskPam.Crm.Runtime.Session;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using AskPam.Crm.Notifications;
using AskPam.Crm.Controllers.Notifications.Dtos;
using AskPam.Crm.Authorization;
using AskPam.Crm.Organizations;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Serialization;
using AskPam.Application.Dto;
using AskPam.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AskPam.Crm.Controllers.Notifications
{
    [Authorize]
    [Route("api/[controller]")]
    public class NotificationsController : BaseController
    {
        private INotificationManager _notificationManager;
        private readonly IRepository<Notification, long> _notificationRepository;
        private readonly IRepository<UserNotification, long> _userNotificationRepository;
        private IUserManager _userManager;
        private IOrganizationManager _organizationManager;

        public NotificationsController(
            ICrmSession session,
            IMapper mapper,
            INotificationManager notificationManager,
            IUserManager userManager,
            IOrganizationManager organizationManager, 
            IRepository<Notification, long> notificationRepository,
            IRepository<UserNotification, long> userNotificationRepository
        ) : base(session, mapper)
        {
            _notificationManager = notificationManager;
            _userManager = userManager;
            _organizationManager = organizationManager;
            _userNotificationRepository = userNotificationRepository;
            _notificationRepository = notificationRepository;
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(PagedResultDto<NotificationDto>), 200)]
        public async Task<IActionResult> GetNotifications([FromBody]GetNotificationsRequestDto input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EnsureOrganization();
            

            var result =  _notificationRepository.GetAll()
               .Join(
                   _userNotificationRepository.GetAll(),
                   (n) => n.Id,
                   (un) => un.NotificationId,
                   (n, un) => new { UserNotification = un, Notification = n }
               )
               .Where(r => r.UserNotification.UserId.Equals(Session.UserId))
               .Where(r => r.Notification.OrganizationId.Equals(Session.OrganizationId.Value))
               .Where(r => (input.Read == null || r.UserNotification.Read == input.Read))
               .OrderByDescending(r => r.Notification.CreatedAt)
               .Select(r => Mapper.Map<UserNotificationWithNotification, NotificationDto>(new UserNotificationWithNotification(r.UserNotification, r.Notification)));


            var totalCount = await result.CountAsync();
            var hasNext = (input.SkipCount + input.MaxResultCount) < totalCount;

            var notifications = await result
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)

                    .ToListAsync();

            return new ObjectResult(
               new PagedResultDto<NotificationDto>(
                   totalCount,
                   notifications,
                   hasNext
               )
           );
        }


        [HttpGet("unreadcount")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> GetUnreadNotificationsCount()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EnsureOrganization();

            var user = await _userManager.FindByIdAsync(Session.UserId);
            var organization = await _organizationManager.FindByIdAsync(Session.OrganizationId.Value);

            var result = await _notificationManager.GetUserUnSeenNotificationCountAsync(user, organization);

            return new ObjectResult(result);
        }


        [HttpPost("{id}/read")]
        public async Task<IActionResult> Read(long id)
        {
            EnsureOrganization();

            await _notificationManager.UpdateNotificationStateAsync(id, true, Session.UserId, Session.OrganizationId.Value);

            return Ok();
        }

        [HttpPost("markAllAsSeen")]
        public async Task<IActionResult> MarkAllNotificationAsSeen()
        {
            EnsureOrganization();

            await _notificationManager.MarkAllNotificationsAsSeen(Session.UserId, Session.OrganizationId.Value);

            return Ok();
        }
    }
}
