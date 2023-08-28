using AskPam.Domain.Services;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace AskPam.Crm.Configuration
{
    public class SettingDefinitionManager : ISettingDefinitionManager
    {
        private readonly IDictionary<string, SettingDefinition> _settings;

        public SettingDefinitionManager()
        {
            _settings = new Dictionary<string, SettingDefinition>();

            foreach (var settings in GetSettingDefinitions())
            {
                _settings[settings.Name] = settings;
            }
        }
        private IEnumerable<SettingDefinition> GetSettingDefinitions()
        {

            return new[]
            {

                //Application
                new SettingDefinition(AppSettingsNames.Application.RootUrl, "http://localhost:53069",isVisibleToClients:true),
                new SettingDefinition(AppSettingsNames.Application.IntercomAppId, "q0vmuzgs",isVisibleToClients:true),
                new SettingDefinition(AppSettingsNames.Application.EmailsToExclude, "support@ask-pam.com"),
                new SettingDefinition(AppSettingsNames.Application.NoReplyEmail, "noreply@ask-pam.com"),
                new SettingDefinition(AppSettingsNames.Application.AvatarColors, "#EF5350,#D32F2F,#B71C1C,#F06292,#D81B60,#880E4F,#BA68C8,#8E24AA,#4A148C,#FF5722,#8D6E63,#6D4C41,#3E2723,#78909C,#4A148C,#512DA8,#311B92,#311B92,#039BE5,#039BE5,#004D40,#43A047,#33691E,#827717,#EF6C00,#757575,#546E7A"),

                //Twilio
                new SettingDefinition(AppSettingsNames.Twilio.AccountSid, "ACe1f1d4285cb76d3a8318c1f3ebe5faa9",scopes:SettingScopes.Application|SettingScopes.Organization, isVisibleToClients:true),
                new SettingDefinition(AppSettingsNames.Twilio.AuthToken, "2ad57a688fbb7f98050738eb7d59d3af",scopes:SettingScopes.Application|SettingScopes.Organization),

                //conversation
                new SettingDefinition(AppSettingsNames.Conversation.ShowPoweredByAskPam, "true",scopes:SettingScopes.Application|SettingScopes.Organization),


                //Bot settings
                new SettingDefinition(AppSettingsNames.AI.QnABotSubscriptionKey , "c7c75d384fe7423e8c4015d007265d91"),
                new SettingDefinition(AppSettingsNames.AI.QnABotKnowledgeBaseId , "",scopes:SettingScopes.Organization),
                new SettingDefinition(AppSettingsNames.AI.QnABotName, "Gustav",scopes:SettingScopes.Application|SettingScopes.Organization),
                new SettingDefinition(AppSettingsNames.AI.QnABotAvatar, "https://secure.gravatar.com/avatar/04babd6a9221bead01133a7f724a33db",scopes:SettingScopes.Application|SettingScopes.Organization),
                new SettingDefinition(AppSettingsNames.AI.QnABotIntro, "Hello there! I'm Gustav the bot. Let me assist you",scopes:SettingScopes.Application|SettingScopes.Organization),
                new SettingDefinition(AppSettingsNames.AI.QnABotOutro, "Sorry, I don't know the answer, let me get one of my human colleague.",scopes:SettingScopes.Application|SettingScopes.Organization),
                new SettingDefinition(AppSettingsNames.AI.QnABotEnabled, false.ToString(),scopes:SettingScopes.Application|SettingScopes.Organization),
                new SettingDefinition(AppSettingsNames.AI.QnABotDeactivationEnabled, false.ToString(),scopes:SettingScopes.Application|SettingScopes.Organization),
                new SettingDefinition(AppSettingsNames.AI.QnABotThreshold , "60",scopes:SettingScopes.Application|SettingScopes.Organization),

                //Smooch settings
                new SettingDefinition(AppSettingsNames.Smooch.AppKeyId, "act_589bafa886eade5e001cac60",scopes:SettingScopes.Application|SettingScopes.Organization),
                new SettingDefinition(AppSettingsNames.Smooch.AppSecret, "ZlJyUcdDewMjLrcR34XbrIk3",scopes:SettingScopes.Application|SettingScopes.Organization),

                //Email settings
                new SettingDefinition(AppSettingsNames.Email.PostmarkAccountToken, "c32b3521-61e1-4866-9e77-887396dd228e"),
                new SettingDefinition(AppSettingsNames.Email.PostmarkServerToken, "a11392f8-cc77-4145-879f-1dcaf7bdcf30",scopes:SettingScopes.Application|SettingScopes.Organization),
                new SettingDefinition(AppSettingsNames.Email.PostmarkInbound, "",scopes:SettingScopes.Organization),
                new SettingDefinition(AppSettingsNames.Email.PostmarkSenderId, "",scopes:SettingScopes.Organization),
                new SettingDefinition(AppSettingsNames.Email.PostmarkIsDkimVerified, "false",scopes:SettingScopes.Organization),
                new SettingDefinition(AppSettingsNames.Email.PostmarkSenderId, "",scopes:SettingScopes.Organization),
                new SettingDefinition(AppSettingsNames.Email.SenderEmail, "",scopes:SettingScopes.Organization),
                new SettingDefinition(AppSettingsNames.Email.SenderEmailName, "",scopes:SettingScopes.Organization),

                //Sendgrid settings
                new SettingDefinition(AppSettingsNames.Sendgrid.ApiKey, "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"),
              
                //Email notification settings
                new SettingDefinition(AppSettingsNames.Notification.EmailNewConversation, false.ToString(),scopes:SettingScopes.User),
                new SettingDefinition(AppSettingsNames.Notification.EmailNewMessage, true.ToString(),scopes:SettingScopes.User),
                new SettingDefinition(AppSettingsNames.Notification.EmailMessageSent, true.ToString(),scopes:SettingScopes.User),
                new SettingDefinition(AppSettingsNames.Notification.EmailConversationAssigned, true.ToString(),scopes:SettingScopes.User),
                new SettingDefinition(AppSettingsNames.Notification.EmailContactAssigned, true.ToString(),scopes:SettingScopes.User),
                new SettingDefinition(AppSettingsNames.Notification.EmailConversationFollowed, true.ToString(),scopes:SettingScopes.User),
                new SettingDefinition(AppSettingsNames.Notification.EmailLibraryItemCreated, true.ToString(),scopes:SettingScopes.User),
                new SettingDefinition(AppSettingsNames.Notification.EmailConversationflagged, true.ToString(),scopes:SettingScopes.User),

                //Klik settings
                new SettingDefinition(AppSettingsNames.Klik.KlikTemplateId, "",scopes:SettingScopes.Organization),
                new SettingDefinition(AppSettingsNames.Klik.KlikEventId, "",scopes:SettingScopes.Organization),
                new SettingDefinition(AppSettingsNames.Klik.KlikRefreshToken, "",scopes:SettingScopes.Organization),

                //E180 settings
                new SettingDefinition(AppSettingsNames.E180.E180Token, "",scopes:SettingScopes.Organization),
                new SettingDefinition(AppSettingsNames.E180.E180Group, "",scopes:SettingScopes.Organization),

                //Stay22
                new SettingDefinition(AppSettingsNames.Stay22.Stay22Event, "",scopes:SettingScopes.Organization,isVisibleToClients:true),

                // Azue Storage
                new SettingDefinition(AppSettingsNames.AzureStorage.ConnectionString, "UseDevelopmentStorage=true"),
                new SettingDefinition(AppSettingsNames.AzureStorage.AvatarPath, "app"),
                new SettingDefinition(AppSettingsNames.AzureStorage.Container, "Organization_{0}/Attachments/Conversation_{1}/{2}"),
                new SettingDefinition(AppSettingsNames.AzureStorage.ConversationAttachmentPath, "Account/Pictures/{0}{1}"),
            };
        }

        public IReadOnlyList<SettingDefinition> GetAllSettingDefinitions()
        {
            return _settings.Values.ToImmutableList();
        }

        public SettingDefinition GetSettingDefinition(string name)
        {
            if (!_settings.TryGetValue(name, out var settingDefinition))
            {
                throw new Exception("There is no setting defined with name: " + name);
            }

            return settingDefinition;
        }
    }
}
