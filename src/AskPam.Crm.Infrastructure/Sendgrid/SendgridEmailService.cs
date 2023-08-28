using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AskPam.Crm.Notifications;
using SendGrid;
using SendGrid.Helpers.Mail;
using Newtonsoft.Json;
using AskPam.Crm.Authorization;
using AskPam.Exceptions;
using System.Net;
using AskPam.Crm.Configuration;

namespace AskPam.Crm.Sendgrid
{
    public class SendgridEmailService : IMailService
    {
        private readonly string _apiKey;
        private readonly string _noReplyEmail;
        private readonly string _rootUrl;


        public SendgridEmailService(ISettingManager settingManager)
        {

            _apiKey = settingManager.GetSettingValueForApplicationAsync(AppSettingsNames.Sendgrid.ApiKey).Result;
            _noReplyEmail = settingManager.GetSettingValueForApplicationAsync(AppSettingsNames.Application.NoReplyEmail).Result;
            _rootUrl = settingManager.GetSettingValueForApplicationAsync(AppSettingsNames.Application.RootUrl).Result;
        }
    

        public async Task SendNotificationAsync(string to, string subject, string body, string entity, string link)
        {
          
            var substitutions = new Dictionary<string, string>()
                {
                    { "{{entity}}", entity },
                    { "{{action_url}}", link },
                    { "{{notifications_url}}", $"{_rootUrl}/profile/notifications" }
                };

            

            var sg = new SendGridClient(_apiKey);

            var msg = new SendGridMessage
            {
                From = new EmailAddress(_noReplyEmail, "Ask PAM"),

                TemplateId = "145c9491-c0cb-4864-b018-0f7e196b123f",
                Subject = subject,
                HtmlContent = body,
                Personalizations = new List<Personalization>()
                {
                    new Personalization()
                    {
                        Tos = new List<EmailAddress> {new EmailAddress(to)},
                        Substitutions = substitutions
                    }
                },
                Categories = new List<string>() {"CRM", "Notification"}
            };




            var response = await sg.SendEmailAsync(msg);
            var text = await response.Body.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.Accepted)
            {
                throw new ApiException(text);
            }
        }

       
    }
}
