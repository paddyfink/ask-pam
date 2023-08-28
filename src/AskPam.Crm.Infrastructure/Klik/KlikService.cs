using AskPam.Crm.Configuration;
using AskPam.Crm.Contacts;
using AskPam.Crm.Conversations.Events;
using AskPam.Events;
using AskPam.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AskPam.Crm.Klik
{
    public class KlikService : IEventHandler<MessageSentEvent>
    {
        private RestClient _client;
        private readonly ISettingManager _settingManager;
        private readonly IContactManager _contactManager;
        private IMemoryCache _cache;
        private const string _cackePrefix = "Klik_Access_Token_";
        private ILogger<KlikService> _logger;

        public KlikService(ISettingManager settingManager, IMemoryCache cache, ILogger<KlikService> logger, IContactManager contactManager)
        {
            _client = new RestClient("https://api.klik.co");
            _settingManager = settingManager;
            _cache = cache;
            _logger = logger;
            _contactManager = contactManager;
        }

        public void Handle(MessageSentEvent args)
        {
            var organization = args.Conversation.Organization;

            if (!organization.Klik)
                return;

            var message = args.Message;
            if (message.IsNote == true)
                return;

            var contact = _contactManager.FindById(args.Conversation.ContactId.Value, organization.Id).Result;

            if (contact.CustomFields.IsNullOrEmpty())
                return;

            if (contact.ExternalId.IsNullOrEmpty())
                return;

            var token = GetToken(organization.Id).Result;

            var request = new RestRequest("/events/{event_id}/scheduled_notifications", Method.POST);

            var eventId = _settingManager.GetSettingValueForOrganizationAsync(AppSettingsNames.Klik.KlikEventId, organization.Id).Result;
            var templateId = _settingManager.GetSettingValueForOrganizationAsync(AppSettingsNames.Klik.KlikTemplateId, organization.Id).Result;

            request.AddUrlSegment("event_id", eventId);

            request.AddHeader("Authorization", "Bearer " + token);
            request.AddJsonBody(new
            {
                trigger = new { type = "date", date = "now" },
                target = new { type = "attendee", credential = "custom:"+contact.ExternalId},
                transports = new[] { new { type = "inapp" } },
                template = new
                {
                    id = templateId,
                    name = "concierge_new_message",
                    short_text = "You have a new message from your concierge!",
                    call_to_action = new
                    {
                        url = $"https=//app.ask-pam.com/widget?appToken={organization.SmoochAppToken}&lang={{attendee.language}}&userId={{attendee.custom_fields.askPamId}}&event=movinon ",
                        label = "Live Chat",
                        target = "blank"
                    }
                },
                active = true,
                allow_duplicates = false,
                type = "notification"
            });

            TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse>();
            RestRequestAsyncHandle handle = _client.ExecuteAsync(request, r => taskCompletion.SetResult(r));

            var response = taskCompletion.Task.Result;

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _settingManager.ChangeSettingForOrganizationAsync(AppSettingsNames.Klik.KlikRefreshToken, string.Empty, organization.Id).Wait();
            }

        }

        private async Task<string> GetToken(Guid organizationId)
        {

            if (!_cache.TryGetValue(_cackePrefix + organizationId, out string token))
            {
                var refreshToken = await _settingManager.GetSettingValueForOrganizationAsync(AppSettingsNames.Klik.KlikRefreshToken, organizationId);
                if (refreshToken.IsNullOrEmpty())
                    token = await Login(organizationId);
                else
                    token = await RefreshToken(organizationId, refreshToken);
            }

            return token;
        }

        private async Task<string> Login(Guid organizationId)
        {

            var request = new RestRequest("login", Method.POST);

            request.AddJsonBody(new
            {
                username = "email:paddy@ask-pam.com",
                password = "askpamklikrules!",
                grant_type = "password",
                client_id = "Ask PAM"
            });

            TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse>();

            RestRequestAsyncHandle handle = _client.ExecuteAsync(request, r => taskCompletion.SetResult(r));

            var response = await taskCompletion.Task;

            JObject dyn = JObject.Parse(response.Content);

            await _settingManager.ChangeSettingForOrganizationAsync(AppSettingsNames.Klik.KlikRefreshToken, dyn["refresh_token"].ToString(), organizationId);
            int expiration = (int)dyn["expires_in"];
            var access_token = dyn["access_token"].ToString();

            SetCache(organizationId, expiration, access_token);

            return access_token;

        }

        private async Task<string> RefreshToken(Guid organizationId, string refreshToken)
        {
            var request = new RestRequest("login", Method.POST);

            request.AddJsonBody(new
            {
                refresh_token = refreshToken,
                grant_type = "refresh_token",
                client_id = "Ask PAM"
            });

            TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse>();

            RestRequestAsyncHandle handle = _client.ExecuteAsync(request, r => taskCompletion.SetResult(r));

            var response = await taskCompletion.Task;

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await _settingManager.ChangeSettingForOrganizationAsync(AppSettingsNames.Klik.KlikRefreshToken, string.Empty, organizationId);
                return await Login(organizationId);
            }

            JObject dyn = JObject.Parse(response.Content);

            await _settingManager.ChangeSettingForOrganizationAsync(AppSettingsNames.Klik.KlikRefreshToken, dyn["refresh_token"].ToString(), organizationId);
            int expiration = (int)dyn["expires_in"];
            var access_token = dyn["access_token"].ToString();

            SetCache(organizationId, expiration, access_token);

            return access_token;
        }

        private void SetCache(Guid organizationId, int expiration, string accessToken)
        {


            var cacheEntryOptions = new MemoryCacheEntryOptions()
                   // Keep in cache for this time, reset time if accessed.
                   .SetSlidingExpiration(TimeSpan.FromSeconds(expiration));

            // Save data in cache.
            _cache.Set(_cackePrefix + organizationId, accessToken, cacheEntryOptions);
        }
    }
}
