using AskPam.Events;
using AskPam.Crm.Hubs;
using AutoMapper;
using AskPam.Crm.Notifications;
using AskPam.Crm.Controllers.Notifications.Dtos;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace AskPam.Crm.Conversations
{
    //[Authorize]
    public class NotificationHub :
        BaseHub,
        IEventHandler<UserNotificationCreatedEvent>
    {
        private readonly IHubContext<NotificationHub> connectionManager;
        private readonly IMapper mapper;
        private readonly IDomainEvents _domainEvents;

        public NotificationHub(
            IHubContext<NotificationHub> connectionManager,
            IMapper mapper,
            IDomainEvents domainEvents
        ) : base()
        {
            this.connectionManager = connectionManager;
            this.mapper = mapper;
            _domainEvents = domainEvents;
        }

        //public override async Task OnConnected()
        //{
        //    var userId = Context.QueryString["userId"];

        //    var organizationId = Context.QueryString["organizationId"];
        //    await Groups.Add(Context.ConnectionId, organizationId);

        //    var client = new ConnectedClient
        //    {
        //        ConnectionId = Context.ConnectionId,
        //        UserAgent = Context.Request.Headers["User-Agent"],
        //        LastActivity = DateTime.UtcNow,
        //        UserId = userId
        //    };

        //    await _domainEvents.RaiseAsync(new UserConnected { Client = client });
        //    await base.OnConnected();
        //}

        //public override async Task OnDisconnected(bool stopCalled)
        //{
        //    var connectionID = Context.ConnectionId;

        //    await _domainEvents.RaiseAsync(new UserDisconnected { connectionID = connectionID });

        //    await base.OnDisconnected(stopCalled);
        //}

        //public override async Task OnReconnected()
        //{
        //    var userId = Context.QueryString["userId"];

        //     var client = new ConnectedClient
        //        {
        //            ConnectionId = Context.ConnectionId,
        //            UserAgent = Context.Request.Headers["User-Agent"],
        //            LastActivity = DateTime.UtcNow,
        //            UserId = userId
        //        };


        //    await _domainEvents.RaiseAsync(new UserConnected { Client = client });

        //    await base.OnReconnected();
        //}

        public async Task Handle(UserNotificationCreatedEvent @event)
        {
            var result = new UserNotificationWithNotification(
                @event.UserNotification,
                @event.Notification
            );

            var notificationDto = Mapper.Map<UserNotificationWithNotification, NotificationDto>(result);

            await connectionManager
                .Clients
                .Group(@event.UserNotification.UserId).InvokeAsync("newNotification", notificationDto);
        }
    }
}
