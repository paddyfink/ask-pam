using AskPam.Crm.Authorization;
using AskPam.Crm.Configuration;
using AskPam.Crm.Conversations;
using AskPam.Crm.Conversations.Events;
using AskPam.Crm.Helpers;
using AskPam.Events;
using AskPam.Crm.Notifications;
using AskPam.Crm.Organizations;
using AskPam.Crm.Presence;
using AskPam.Crm.Settings;
using AskPam.Domain.Repositories;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.Sendgrid
{
    public class EmailNotifier :
        IEventHandler<UserNotificationCreatedEvent>,
        IEventHandler<ConversationCreatedEvent>

    {
        private readonly ISettingManager _settingManager;
        private readonly IOrganizationManager _organizManager;
        private readonly IMailService _mailService;
        private readonly IRepository<ConnectedClient, long> _connectedClients;
        private readonly IUserManager _userManager;
        private readonly string _rootUrl;

        public EmailNotifier(
            ISettingManager settingManager,
            IMailService mailService,
            IRepository<ConnectedClient, long> connectedClients,
            IOrganizationManager organizManager,
            IUserManager userManager
        )
        {
            _settingManager = settingManager;
            _mailService = mailService;
            _connectedClients = connectedClients;
            _organizManager = organizManager;
            _userManager = userManager;
            _rootUrl = settingManager.GetSettingValueForApplicationAsync(AppSettingsNames.Application.RootUrl).Result;
        }

        //TODO Get Information From SignalR
        private bool IsOnline(User user)
        {
            return _connectedClients.GetAll().Any(c => c.UserId == user.Id);
        }

        public async Task Handle(UserNotificationCreatedEvent args)
        {
            var organization = await _organizManager.FindByIdAsync(args.Notification.OrganizationId);

            if (IsOnline(args.User))
            {
                return;
            }

            bool setting;

            dynamic data = null;
            if (!string.IsNullOrEmpty(args.Notification.Data))
            {
                data = JsonConvert.DeserializeObject(args.Notification.Data);
            }

            switch (args.Notification.NotificationType)
            {
                case NotificationTypes.LibraryItemCreated:

                    setting = bool.Parse(await _settingManager.GetSettingValueForUserAsync(AppSettingsNames.Notification.EmailLibraryItemCreated,
                        args.Notification.OrganizationId, args.UserNotification.UserId));
                    if (setting)
                    {
                        var link = getLibraryLink(args.Notification.EntityId);
                        var subject = $"{getSubjectPrefix(organization)}New library item";
                        var body = $"{data.CreatorName} added a new library item : <a href=\"{link}\">{data.Name}</a>";
                        var entity = "library item";
                        await _mailService.SendNotificationAsync(args.User.Email, subject, body, entity, link);
                    }
                    break;

                case NotificationTypes.ContactAssigned:

                    setting = bool.Parse(await _settingManager.GetSettingValueForUserAsync(AppSettingsNames.Notification.EmailContactAssigned,
                        args.Notification.OrganizationId, args.UserNotification.UserId));
                    if (setting)
                    {
                        var link = getContactLink(args.Notification.EntityId);
                        var subject = $"{getSubjectPrefix(organization)}A new contact was assigned to you";
                        var body = $"<a href=\"{link}\">{data.ContactName}</a> was assigned to you ";
                        var entity = "contact";
                        await _mailService.SendNotificationAsync(args.User.Email, subject, body, entity, link);
                    }
                    break;

                case NotificationTypes.ConversationFollowed:

                    setting = bool.Parse(await _settingManager.GetSettingValueForUserAsync(AppSettingsNames.Notification.EmailConversationFollowed,
                        args.Notification.OrganizationId, args.UserNotification.UserId));
                    if (setting)
                    {
                        var link = getContactLink(args.Notification.EntityId);
                        var subject = $"{getSubjectPrefix(organization)}You are now following a conversation with {data.ConversationName}";
                        var body = $"{data.AssignerName} added you as a follower to a <a href=\"{link}\">conversation</a> with <b>{data.ConversationName}</b> ";
                        var entity = "conversation";
                        await _mailService.SendNotificationAsync(args.User.Email, subject, body, entity, link);
                    }
                    break;

                case NotificationTypes.ConversationAssigned:

                    setting = bool.Parse(await _settingManager.GetSettingValueForUserAsync(AppSettingsNames.Notification.EmailConversationAssigned,
                        args.Notification.OrganizationId, args.UserNotification.UserId));
                    if (setting)
                    {
                        var link = getConversationLink(args.Notification.EntityId);
                        var subject = $"{getSubjectPrefix(organization)}A conversation with {data.ConversationName} was assigned to you";
                        var body = $"You have been assigned a <a href=\"{link}\">conversation</a> with <b>{data.ConversationName}</b> by {data.AssignerName}";
                        var entity = "conversation";
                        await _mailService.SendNotificationAsync(args.User.Email, subject, body, entity, link);
                    }
                    break;

                case NotificationTypes.ConversationFlagged:

                    setting = bool.Parse(await _settingManager.GetSettingValueForUserAsync(AppSettingsNames.Notification.EmailConversationflagged,
                        args.Notification.OrganizationId, args.UserNotification.UserId));
                    if (setting)
                    {
                        var link = getConversationLink(args.Notification.EntityId);
                        var subject = $"{getSubjectPrefix(organization)}{data.UserName} flagged a conversation with {data.ConversationName}";
                        var body = $"{data.UserName} flagged a <a href=\"{link}\">conversation</a> with <b>{data.ConversationName}</b>";
                        var entity = "conversation";
                        await _mailService.SendNotificationAsync(args.User.Email, subject, body, entity, link);
                    }
                    break;

                case NotificationTypes.MessageSent:

                    setting = bool.Parse(await _settingManager.GetSettingValueForUserAsync(AppSettingsNames.Notification.EmailMessageSent, args.Notification.OrganizationId,
                        args.UserNotification.UserId));
                    if (setting)
                    {
                        //                  public const string ReplytoConversationWith = "{0} replied to a conversation with {1}";
                        //public const string AddNoteToConveration = "{0} added a note to a conversation with {1}";
                        var isnote = false;
                        if (data.IsNote != null)
                            bool.TryParse(data.IsNote.ToString(), out isnote);
                        if (isnote)
                        {
                            var link = getConversationLink(args.Notification.EntityId);
                            var subject = $"{getSubjectPrefix(organization)}{data.From} added a note to a conversation with {data.ConversationName}";
                            var body = $"<p>{data.From} added a note to a <a href=\"{link}\">conversation</a> with <b>{data.ConversationName}</b> : </p><blockquote>{StringHelper.TextToHtml(data.MessageText.ToString())}</blockquote>";
                            var entity = "conversation";
                            await _mailService.SendNotificationAsync(args.User.Email, subject, body, entity, link);
                        }
                        else
                        {
                            var link = getConversationLink(args.Notification.EntityId);
                            var subject = $"{getSubjectPrefix(organization)}{data.From} replied to a conversation with {data.ConversationName}";
                            var body = $"<p>{data.From} replied to a <a href=\"{link}\">conversation</a> with <b>{data.ConversationName}</b> : </p><blockquote>{StringHelper.TextToHtml(data.MessageText.ToString())}</blockquote>";
                            var entity = "conversation";
                            await _mailService.SendNotificationAsync(args.User.Email, subject, body, entity, link);
                        }
                    }
                    break;

                case NotificationTypes.NewMessage:

                    setting = bool.Parse(await _settingManager.GetSettingValueForUserAsync(AppSettingsNames.Notification.EmailNewMessage, args.Notification.OrganizationId,
                        args.UserNotification.UserId));
                    if (setting)
                    {
                        var link = getConversationLink(args.Notification.EntityId);
                        var subject = $"{getSubjectPrefix(organization)}{data.ConversationName} replied to a conversation ";
                        var body = $"<b>{data.ConversationName}</b> replied to a <a href=\"{link}\">conversation</a> : </p><blockquote>{StringHelper.TextToHtml(data.MessageText.ToString())}</blockquote>";
                        const string entity = "conversation";

                        await _mailService.SendNotificationAsync(args.User.Email, subject, body, entity, link);
                    }
                    break;

                case NotificationTypes.BotAnswerNotFound:

                    setting = bool.Parse(await _settingManager.GetSettingValueForUserAsync(AppSettingsNames.Notification.EmailNewMessage,
                        args.Notification.OrganizationId, args.UserNotification.UserId));
                    if (setting)
                    {
                        var link = getConversationLink(args.Notification.EntityId);
                        var subject = $"The bot requested assistance for a conversation with {getSubjectPrefix(organization)}{data.ConversationName} ";
                        var body = $"The bot couldn't answer to a <a href=\"{link}\">message</a> : </p><blockquote>{StringHelper.TextToHtml(data.MessageText.ToString())}</blockquote>";
                        const string entity = "conversation";

                        await _mailService.SendNotificationAsync(args.User.Email, subject, body, entity, link);
                    }
                    break;

                default:
                    break;
            }
        }

        public async Task Handle(ConversationCreatedEvent args)
        {
            var organization = await _organizManager.FindByIdAsync(args.Conversation.OrganizationId);

            var users = await _userManager.GetUsersInRoleAsync(args.Conversation.OrganizationId, RolesName.Admin);

            foreach (var user in users)
            {
                if (IsOnline(user))
                {
                    break;
                }

                var setting = bool.Parse(await _settingManager.GetSettingValueForUserAsync(AppSettingsNames.Notification.EmailNewConversation, args.Conversation.OrganizationId, user.Id));
                if (setting)
                {
                    var link = getConversationLink(args.Conversation.Id.ToString());
                    var subject = $"{getSubjectPrefix(organization)}{args.Conversation.Contact.FullName} received a new conversation ";
                    var body = $"<b>{args.Conversation.Contact.FullName}</b> started a new <a href=\"{link}\">conversation</a> : </p><blockquote>{StringHelper.TextToHtml(args.Message.Text.ToString())}</blockquote>";
                    const string entity = "conversation";

                    await _mailService.SendNotificationAsync(user.Email, subject, body, entity, link);
                }
            }

        }

        private string getContactLink(string entityId)
        {
            return $"{_rootUrl}/contacts/view/{entityId}";
        }

        private string getConversationLink(string entityId)
        {
            return $"{_rootUrl}/conversations/{entityId}";
        }

        private string getLibraryLink(string entityId)
        {
            return $"{_rootUrl}/library/view/{entityId}";
        }

        private string getSubjectPrefix(Organization organization)
        {
            return organization == null ? "" : $"{organization.Name}: ";
        }
    }
}
