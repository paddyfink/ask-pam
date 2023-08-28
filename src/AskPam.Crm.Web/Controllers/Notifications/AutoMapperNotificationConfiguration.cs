using AskPam.Crm.Controllers.Notifications.Dtos;
using AskPam.Crm.Library;
using AskPam.Crm.Library.Dtos;
using AskPam.Crm.Notifications;
using AutoMapper;

namespace AskPam.Crm.Authorization.Library
{
    public class NotificationsAutomapperProfile : Profile
    {
        public NotificationsAutomapperProfile()
        {
            CreateMap<UserNotificationWithNotification, NotificationDto>()
                .ForMember(
                    dest => dest.Id,
                    opts => opts.MapFrom(src => src.Notification.Id)
                )
                .ForMember(
                    dest => dest.CreatedAt,
                    opts => opts.MapFrom(src => src.Notification.CreatedAt.Value.ToLocalTime())
                )
                .ForMember(
                    dest => dest.Data,
                    opts => opts.MapFrom(src => src.Notification.Data)
                )
                .ForMember(
                    dest => dest.EntityId,
                    opts => opts.MapFrom(src => src.Notification.EntityId)
                )
                .ForMember(
                    dest => dest.EntityType,
                    opts => opts.MapFrom(src => src.Notification.EntityType)
                )
                .ForMember(
                    dest => dest.NotificationType,
                    opts => opts.MapFrom(src => src.Notification.NotificationType)
                )
                .ForMember(
                    dest => dest.Seen,
                    opts => opts.MapFrom(src => src.UserNotification.Seen)
                )
                .ForMember(
                dest => dest.Read,
                opts => opts.MapFrom(src => src.UserNotification.Read)
                );
        }
    }
}
