using AskPam.Crm.Notifications;
using System;

namespace AskPam.Crm.Controllers.Notifications.Dtos
{
    public class NotificationDto
    {
        public long Id { get; set; }

        public bool Seen { get; set; }
        public bool Read { get; set; }
        public string Data { get; set; }

        //See Notification list types in AskPam.Crm.Notifications.NotificationTypes
        public string NotificationType { get; set; }

        public string EntityType { get; set; }
        public string EntityId { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string Description
        {
            get
            {
                if (Data == null)
                {
                    return null;
                }

                dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(Data);

                switch (NotificationType)
                {
                    case NotificationTypes.NewMessage:
                        return string.Format(
                           NotificationTypeDescriptions.ReplytoConversation,
                           data.ConversationName
                       );

                    case NotificationTypes.BotAnswerNotFound:
                        return string.Format(
                           NotificationTypeDescriptions.BotAnswerNotFound,
                           data.ConversationName
                       );

                    case NotificationTypes.MessageSent:
                        var isnote = false;
                        if (data.IsNote != null)
                            bool.TryParse(data.IsNote.ToString(), out isnote);
                        if (isnote)
                            return string.Format(
                              NotificationTypeDescriptions.AddNoteToConveration,
                              data.From, data.ConversationName
                          );
                        else
                            return string.Format(
                          NotificationTypeDescriptions.ReplytoConversationWith,
                          data.From, data.ConversationName
                      );

                    case NotificationTypes.ConversationFollowed:
                        return string.Format(
                            NotificationTypeDescriptions.ConversationFollowed,
                            data.AssignerName,
                            data.ConversationName
                        );
                    case NotificationTypes.ConversationAssigned:
                        return string.Format(
                            NotificationTypeDescriptions.ConversationAssigned,
                            data.ConversationName,
                            data.AssignerName
                        );
                    case NotificationTypes.ConversationFlagged:
                        return string.Format(
                            NotificationTypeDescriptions.ConversationFlagged,
                            data.UserName,
                            data.ConversationName
                        );
                    case NotificationTypes.LibraryItemCreated:
                        return string.Format(
                            NotificationTypeDescriptions.LibraryItemCreated,
                            data.Name,
                            data.CreatorName
                        );
                    case NotificationTypes.PostAdded:
                        return string.Format(
                            NotificationTypeDescriptions.PostAdded,
                            data.Name,
                            data.CreatorName
                        );
                    case NotificationTypes.PostCommented:
                        return string.Format(
                            NotificationTypeDescriptions.PostCommented,
                            data.CommentorName,
                            data.PostTitle
                        );
                    case NotificationTypes.ContactAssigned:
                        return string.Format(
                            NotificationTypeDescriptions.ContactAssigned,
                            data.ContactName
                        );
                    case NotificationTypes.ContactUnAssigned:
                        return string.Format(
                            NotificationTypeDescriptions.ContactUnAssigned,
                            data.ContactName
                        );
                    default:
                        return "";
                }
            }
        }
    }
}