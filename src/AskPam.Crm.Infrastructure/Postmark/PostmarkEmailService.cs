using AskPam.Crm.Conversations;
using AskPam.Crm.Settings;
using Microsoft.Extensions.Options;
using RestSharp;
using System.Threading.Tasks;
using AskPam.Crm.Postmark.Entities;
using System;
using AskPam.Exceptions;
using System.Linq;
using Newtonsoft.Json.Linq;
using AskPam.Crm.Configuration;
using AskPam.Crm.Organizations;
using System.Net;
using AskPam.Crm.Contacts;

namespace AskPam.Crm.Postmark
{
    public class PostmarkService : IPostmarkService
    {
        private readonly ISettingManager _settingManager;

        public PostmarkService(ISettingManager settingManager)
        {
            _settingManager = settingManager;
        }



        public async Task<string> SendEmailAsync(Organization organization, Email mail)
        {
            if (string.IsNullOrEmpty(mail.Author))
            {
                throw new ArgumentNullException(nameof(mail), "Mail name is empty");
            }
            var client = new RestClient("https://api.postmarkapp.com/");

            var request = new RestRequest("email", Method.POST);
            var serverToken = await GetServerToken(organization);
            var body = string.Empty;

            bool.TryParse(await _settingManager.GetSettingValueForOrganizationAsync(AppSettingsNames.Conversation.ShowPoweredByAskPam, organization.Id), out var showPoweredByAskPam);

            body = showPoweredByAskPam ? EmailTemplate.Html.Replace("{{body}}", mail.HtmlBody) : mail.HtmlBody;

            var emailRequest = new EmailRequest()
            {
                From = $"{mail.Author} <{mail.From}>",
                To = mail.To,
                Cc = mail.Cc,
                Bcc = mail.Bcc,
                Subject = mail.Subject,
                HtmlBody = body,
                TrackOpens = true,
                Tag = organization.Name,
            };

            if (mail.Attachments != null)
            {
                emailRequest.Attachments = mail.Attachments
                    .Select(a => new AttachmentRequest()
                    {
                        Content = a.ContentString,
                        ContentType = a.ContentType,
                        Name = a.Name
                    }
                    )
                    .ToList();
            }

            // easily add HTTP Headers
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("X-Postmark-Server-Token", serverToken);


            request.AddJsonBody(emailRequest);

            var taskCompletion = new TaskCompletionSource<IRestResponse<SendEmailResponse>>();

            var handle = client.ExecuteAsync<SendEmailResponse>(request, r => taskCompletion.SetResult(r));

            var response = await taskCompletion.Task;

            var dyn = JObject.Parse(response.Content);

            var errorCode = (int)dyn["ErrorCode"];
            if (errorCode != 0)
                throw new ApiException(dyn["Message"].ToString(), HttpStatusCode.BadRequest);

            return response.Data.MessageID;
        }

        //private async Task<string> SendEmailWithTemplate(Email mail, string templateId)
        //{
        //    if (string.IsNullOrEmpty(mail.Author))
        //    {
        //        throw new ArgumentNullException("Mail name is empty");
        //    }
        //    var client = new RestClient("https://api.postmarkapp.com/");

        //    var request = new RestRequest("email/withTemplate", Method.POST);

        //    var emailRequest = new
        //    {
        //        From = $"{mail.Author} <{mail.From}>",
        //        To = mail.To,
        //        TrackOpens = true,
        //        TemplateId = templateId,
        //        TemplateModel = new
        //        {
        //            subject = mail.Subject,
        //            body = mail.HtmlBody
        //        },
        //        Attachments = mail.Attachments?.Select(a => new AttachmentRequest()
        //        {
        //            Content = a.ContentString,
        //            ContentType = a.ContentType,
        //            Name = a.Name
        //        }
        //            )
        //            .ToList()
        //    };
        //    var serverToken = await GetServerToken(organization);
        //    // easily add HTTP Headers
        //    request.AddHeader("Content-Type", "application/json");
        //    request.AddHeader("Accept", "application/json");
        //    request.AddHeader("X-Postmark-Server-Token", integration.Token);


        //    request.AddJsonBody(emailRequest);

        //    var taskCompletion = new TaskCompletionSource<IRestResponse<SendEmailResponse>>();

        //    var handle = client.ExecuteAsync<SendEmailResponse>(request, r => taskCompletion.SetResult(r));

        //    var response = await taskCompletion.Task;

        //    JObject dyn = JObject.Parse(response.Content);

        //    var errorCode = (int)dyn["ErrorCode"];
        //    if (errorCode != 0)
        //        throw new ApiException(dyn["Message"].ToString(), HttpStatusCode.BadRequest);

        //    return response.Data.MessageID;
        //}

        public async Task<int> AddEmailSenderAsync(string email, string name)
        {
            var client = new RestClient("https://api.postmarkapp.com/");

            var request = new RestRequest("senders", Method.POST);
            var accountToken =
                await _settingManager.GetSettingValueForApplicationAsync(AppSettingsNames.Email.PostmarkAccountToken);
            // easily add HTTP Headers
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("X-Postmark-Account-Token", accountToken);

            request.AddJsonBody(new
            {
                FromEmail = email,
                Name = name,
            });

            var taskCompletion = new TaskCompletionSource<IRestResponse>();

            client.ExecuteAsync(request, r => taskCompletion.SetResult(r));

            var response = await taskCompletion.Task;

            if (response.ErrorException != null)
                throw new ApiException(response.ErrorException);

            var dyn = JObject.Parse(response.Content);

            if (dyn["ErrorCode"] != null)
                throw new ApiException(dyn["Message"].ToString(), HttpStatusCode.BadRequest);

            return int.Parse(dyn["ID"].ToString());

        }


        public async Task<int> UpdateEmailSenderAsync(int signatureSenderId, string name)
        {
            var client = new RestClient("https://api.postmarkapp.com/");

            var request = new RestRequest("senders/{signatureid}", Method.PUT);
            var accountToken =
                await _settingManager.GetSettingValueForApplicationAsync(AppSettingsNames.Email.PostmarkAccountToken);

            // easily add HTTP Headers
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("X-Postmark-Account-Token", accountToken);

            request.AddUrlSegment("signatureid", signatureSenderId);
            request.AddJsonBody(new
            {
                Name = name,
            });

            var taskCompletion = new TaskCompletionSource<IRestResponse>();

            var handle = client.ExecuteAsync(request, r => taskCompletion.SetResult(r));

            var response = await taskCompletion.Task;

            if (response.ErrorException != null)
                throw new ApiException(response.ErrorException);

            var dyn = JObject.Parse(response.Content);

            return int.Parse(dyn["ID"].ToString());

        }

        public async Task DeleteEmailSenderAsync(int signatureSenderId)
        {
            var client = new RestClient("https://api.postmarkapp.com/");

            var request = new RestRequest("senders/{signatureid}", Method.DELETE);
            var accountToken =
                await _settingManager.GetSettingValueForApplicationAsync(AppSettingsNames.Email.PostmarkAccountToken);

            // easily add HTTP Headers
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("X-Postmark-Account-Token", accountToken);

            request.AddUrlSegment("signatureid", signatureSenderId);


            var taskCompletion = new TaskCompletionSource<IRestResponse>();

            client.ExecuteAsync(request, r => taskCompletion.SetResult(r));

            var response = await taskCompletion.Task;

        }

        #region private
        private async Task<string> GetServerToken(Organization org)
        {
            return await _settingManager.GetSettingValueForOrganizationAsync(AppSettingsNames.Email.PostmarkServerToken, org.Id);
        }
        #endregion
    }
}
