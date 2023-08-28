using AskPam.Crm.Conversations;
using AskPam.Crm.Conversations.Events;
using AskPam.Events;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using System.Linq;
using AskPam.Extensions;
using System.Collections.Generic;
using AskPam.Crm.Configuration;

namespace AskPam.Crm.Notifications
{
    public partial class NotificationHandler :
        IEventHandler<ConversationAssignedEvent>,
        IEventHandler<ConversationFollowedEvent>,
        IEventHandler<MessageReceivedEvent>,
        IEventHandler<MessageSentEvent>,
        IEventHandler<ConversationBotAnswerNotFoundEvent>,
        IEventHandler<ConversationFlaggedEvent>
    {

        public async Task Handle(ConversationAssignedEvent conversationAssigned)
        {
            if (conversationAssigned.Assigner != null && (conversationAssigned.Assignee.Id != conversationAssigned.Assigner.Id))
            {
                var assignee = conversationAssigned.Assignee;
                var assigner = conversationAssigned.Assigner;
                var contact = conversationAssigned.Conversation.Contact;

                var notification = new Notification
                {
                    EntityId = conversationAssigned.Conversation.Id.ToString(),
                    NotificationType = NotificationTypes.ConversationAssigned,
                    OrganizationId = conversationAssigned.Conversation.OrganizationId,
                    EntityType = typeof(Conversation).Name,
                    Data = JsonConvert.SerializeObject(new
                    {
                        AssignerId = assigner.Id,
                        AssignerName = assigner.FullName,
                        ConversationName = contact != null ? contact.FullName : conversationAssigned.Conversation.Name,
                    })
                };

                notification = await _notificationManager.InsertNotificationAsync(notification);

                var userNotification = new UserNotification
                {
                    UserId = assignee.Id,
                    NotificationId = notification.Id,
                    OrganizationId = conversationAssigned.Conversation.OrganizationId
                };

                await _notificationManager.InsertUserNotificationAsync(notification, userNotification);
            }
        }

        public async Task Handle(ConversationFollowedEvent @event)
        {
            var creator = await _userManager.FindByIdAsync(@event.FollowersRelation.CreatedById);
            var contact = @event.Conversation.Contact;

            var notification = new Notification
            {
                EntityId = @event.FollowersRelation.ConversationId.ToString(),
                NotificationType = NotificationTypes.ConversationFollowed,
                OrganizationId = @event.Conversation.OrganizationId,
                EntityType = typeof(Conversation).Name,
                Data = JsonConvert.SerializeObject(new
                {
                    AssignerId = creator.Id,
                    AssignerName = creator.FullName,
                    ConversationName = contact != null ? contact.FullName : @event.Conversation.Name
                })
            };

            notification = await _notificationManager.InsertNotificationAsync(notification);

            await _notificationManager.InsertUserNotificationAsync(notification, new UserNotification
            {
                UserId = @event.FollowersRelation.UserId,
                NotificationId = notification.Id,
                OrganizationId = @event.Conversation.OrganizationId
            });
        }

        public async Task Handle(MessageSentEvent @event)
        {
            var contact = @event.Conversation.Contact;

            var notification = new Notification
            {
                EntityId = @event.Conversation.Id.ToString(),
                NotificationType = NotificationTypes.MessageSent,
                OrganizationId = @event.Conversation.OrganizationId,
                EntityType = typeof(Conversation).Name,
                Data = JsonConvert.SerializeObject(new
                {
                    From = @event.Message.Author,
                    ConversationName = contact != null ? contact.FullName : @event.Conversation.Name,
                    MessageId = @event.Message.Id,
                    MessageText = @event.Message.Text,

                })
            };
            notification = await _notificationManager.InsertNotificationAsync(notification);

            if (@event.Message.AuthorId != null)
            {
                var sender = await _userManager.FindByIdAsync(@event.Message.AuthorId);

                var userIds = (await _followerManager.GetFollowers(@event.Conversation.Id, @event.Conversation.OrganizationId))
                    .Where(u => u.Id != sender.Id)
                    .Select(u => u.Id)
                    .ToList();

                if (userIds != null)
                {
                    if (!@event.Conversation.AssignedToId.IsNullOrEmpty() && sender.Id != @event.Conversation.AssignedToId && !userIds.Contains(@event.Conversation.AssignedToId))
                    {
                        if (userIds == null)
                            userIds = new List<string>();

                        userIds.Add(@event.Conversation.AssignedToId);
                    }

                    foreach (var userId in userIds)
                    {
                        await _notificationManager.InsertUserNotificationAsync(notification, new UserNotification
                        {
                            UserId = userId,
                            NotificationId = notification.Id,
                            OrganizationId = @event.Conversation.OrganizationId
                        });
                    }
                }
            }
        }

        public async Task Handle(MessageReceivedEvent @event)
        {
            var contact = @event.Conversation.Contact;
            var notification = new Notification
            {
                EntityId = @event.Conversation.Id.ToString(),
                NotificationType = NotificationTypes.NewMessage,
                OrganizationId = @event.Conversation.OrganizationId,
                EntityType = typeof(Conversation).Name,
                //todo : duplicate form and conversationame
                Data = JsonConvert.SerializeObject(new
                {
                    From = contact != null ? contact.FullName : @event.Conversation.Name,
                    ConversationName = contact != null ? contact.FullName : @event.Conversation.Name,
                    MessageId = @event.Message.Id,
                    MessageText = @event.Message.Text
                })
            };
            notification = await _notificationManager.InsertNotificationAsync(notification);

            var userIds = (await _followerManager.GetFollowers(@event.Conversation.Id, @event.Conversation.OrganizationId))
                .Select(u => u.Id)
                .ToList();

            if (!@event.Conversation.AssignedToId.IsNullOrEmpty() && !userIds.Contains(@event.Conversation.AssignedToId))
            {
                userIds.Add(@event.Conversation.AssignedToId);
            }

            foreach (var userId in userIds)
            {
                await _notificationManager.InsertUserNotificationAsync(notification, new UserNotification
                {
                    UserId = userId,
                    NotificationId = notification.Id,
                    OrganizationId = @event.Conversation.OrganizationId
                });
            }
        }

        public async Task Handle(ConversationBotAnswerNotFoundEvent @event)
        {
            var conversation = @event.Conversation;
            var contact = @event.Conversation.Contact;
            var notification = new Notification
            {
                EntityId = conversation.Id.ToString(),
                NotificationType = NotificationTypes.BotAnswerNotFound,
                OrganizationId = conversation.OrganizationId,
                EntityType = typeof(Conversation).Name,
                //todo: duplivate from and conversationName
                Data = JsonConvert.SerializeObject(new
                {
                    From = contact != null ? contact.FullName : @event.Conversation.Name,
                    ConversationName = contact != null ? contact.FullName : @event.Conversation.Name,
                    MessageId = @event.Message.Id,
                    MessageText = @event.Message.Text
                })
            };

            notification = await _notificationManager.InsertNotificationAsync(notification);

            var followers = await _followerManager.GetFollowers(conversation.Id, conversation.OrganizationId);
            if (followers != null)
            {
                foreach (var follower in followers)
                {
                    await CreateUserNotification(follower.Id);
                }
            }

            if (followers != null || !followers.Any(f => f.Id == conversation.AssignedToId))
            {
                await CreateUserNotification(conversation.AssignedToId);
            }

            async Task CreateUserNotification(string userId)
            {
                await _notificationManager.InsertUserNotificationAsync(notification,
                    new UserNotification()
                    {
                        NotificationId = notification.Id,
                        UserId = userId,
                        OrganizationId = conversation.OrganizationId
                    }
                );
            }
        }

        public async Task Handle(ConversationFlaggedEvent args)
        {
            if (args.Conversation.IsFlagged)
            {
                var flagger = args.User;

                var notification = new Notification
                {
                    EntityId = args.Conversation.Id.ToString(),
                    NotificationType = NotificationTypes.ConversationFlagged,
                    OrganizationId = args.Conversation.OrganizationId,
                    EntityType = typeof(Conversation).Name,
                    Data = JsonConvert.SerializeObject(new
                    {
                        UserId = flagger.Id,
                        UserName = flagger.FullName,
                        ConversationName = args.Conversation.Contact != null ? args.Conversation.Contact.FullName : args.Conversation.Name,
                    })
                };

                notification = await _notificationManager.InsertNotificationAsync(notification);

                var usersToNotify = (await _followerManager.GetFollowers(args.Conversation.Id, args.Conversation.OrganizationId))
                               .Where(a => a.Id != flagger.Id)
                               .Select(u => u.Id)
                               .ToList();

                if (!args.Conversation.AssignedToId.IsNullOrEmpty() && !usersToNotify.Contains(args.Conversation.AssignedToId) && args.Conversation.AssignedToId != flagger.Id)
                    usersToNotify.Add(args.Conversation.AssignedToId);

                foreach (var user in usersToNotify)
                {
                    await _notificationManager.InsertUserNotificationAsync(notification, new UserNotification
                    {
                        UserId = user,
                        NotificationId = notification.Id,
                        OrganizationId = args.Conversation.OrganizationId
                    });
                }

            }

        }
    }
}
