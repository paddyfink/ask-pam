using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.Configuration
{
    public static class AppSettingsNames
    {          
        public static class Application
        {
            public const string RootUrl = "App.RootUrl";
            public const string IntercomAppId = "App.IntercomAppId";
            public const string EmailsToExclude = "App.EmailsToExclude";
            public const string NoReplyEmail = "App.NoReplyEmail";
            public const string AvatarColors = "App.AvatarColors";
        }

        public static class Conversation
        {
            public const string ShowPoweredByAskPam = "App.Conversation.ShowPoweredByAskPam";
            public const string EmailSignature = "App.Conversation.EmailSignature";
        }

        public static class Twilio
        {
            public const string AccountSid = "App.Twilio.AccountSid";
            public const string AuthToken = "App.Twilio.AuthToken";
        }
        public static class Notification
        {
            public const string EmailNewConversation = "App.Notification.Email.NewConversation";
            public const string EmailNewMessage = "App.Notification.Email.NewMessage";
            public const string EmailNewMessageInUnassigned = "App.Notification.Email.NewMessage";
            public const string EmailMessageSent = "App.Notification.Email.MessageSent";
            public const string EmailConversationAssigned = "App.Notification.Email.ConversationAssigned";
            public const string EmailContactAssigned = "App.Notification.Email.ContactAssigned";
            public const string EmailConversationFollowed = "App.Notification.Email.ConversationFollowed";
            public const string EmailLibraryItemCreated = "App.Notification.Email.LibraryItemCreated";
            public const string EmailConversationflagged = "App.Notification.Email.Conversationflagged";
        }

        public static class AI
        {
            public const string QnABotSubscriptionKey = "App.AI.QnABotSubscriptionKey";
            public const string QnABotKnowledgeBaseId = "App.AI.QnABotKnowledgeBaseId";
            public const string QnABotEnabled = "App.AI.QnABotEnabled";
            public const string QnABotName = "App.AI.QnABotName";
            public const string QnABotAvatar = "App.AI.QnABotAvatar";
            public const string QnABotIntro = "App.AI.QnABotIntro";
            public const string QnABotOutro = "App.AI.QnABotOutro";
            public const string QnABotDeactivationEnabled = "App.AI.QnABotDeactivationEnabled";
            public const string QnABotThreshold = "App.AI.QnABotThreshold";            
        }

        public static class Smooch
        {
            public const string AppKeyId = "App.Smooch.AppKeyId";
            public const string AppSecret = "App.Smooch.AppSecret";
            public const string AppToken = "App.Smooch.AppToken";
            public const string AppId = "App.Smooch.AppId";
        }

        public static class Email
        {
            public const string PostmarkAccountToken = "App.Email.Postmark.AccountToken";
            public const string PostmarkServerToken = "App.Email.Postmark.ServerToken";
            public const string PostmarkInbound = "App.Email.Postmark.Inbound";
            public const string PostmarkSenderId = "App.Email.Postmark.SenderId";
            public const string PostmarkIsDkimVerified = "App.Email.Postmark.IsDkimVerified";
            public const string SenderEmail = "App.Email.Sender.Email";
            public const string SenderEmailName = "App.Email.Sender.EmailName";
            
        }

        
        public static class Sendgrid
        {
            public const string ApiKey = "App.Sendgrid.ApiKey";
        }

        public static class Klik
        {
            public const string KlikTemplateId = "App.Klik.TemplateId";
            public const string KlikEventId = "App.Klik.EventId";
            public const string KlikRefreshToken = "App.Klik.RefreshToken";
        }

        public static class E180
        {
            public const string E180Token = "App.E180.Token";
            public const string E180Group = "App.E180.Group";
        }

        public static class Stay22
        {
            public const string Stay22Event = "App.Stay22.Event";
        }

        public static class AzureStorage
        {
            public const string ConnectionString = "App.AzureStorage.ConnectionString";
            public const string Container = "App.AzureStorage.Container";
            public const string ConversationAttachmentPath = "App.AzureStorage.ConversationAttachmentPath";
            public const string AvatarPath = "App.AzureStorage.AvatarPath";
        }

    }
}