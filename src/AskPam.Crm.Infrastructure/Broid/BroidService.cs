//using AskPam.Crm.Authorization;
//using AskPam.Crm.Common;
//using AskPam.Crm.Configuration;
//using AskPam.Crm.Conversations;
//using AskPam.Crm.Integrations;
//using AskPam.Crm.Organizations;
//using AskPam.Exceptions;
//using Microsoft.Extensions.Caching.Memory;
//using Newtonsoft.Json.Linq;
//using RestSharp;
//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Text;
//using System.Threading.Tasks;

//namespace AskPam.Crm.Broid
//{
//    public class BroidService : IBroidService
//    {
//        private RestClient _client;
//        private readonly ISettingManager _settingManager;
//        private IMemoryCache _cache;

//        public BroidService(ISettingManager settingManager, IMemoryCache cache)
//        {
//            _cache = cache;
//            _settingManager = settingManager;
//            _client = new RestClient("https://api.broid.ai/1");
//        }

//        public async Task CreateApplication(Organization organization)
//        {
//            var token = getAccountToken();

//            var request = new RestRequest("applications", Method.POST);
//            var teamId = _settingManager.GetSettingValueForOrganizationAsync(AppSettingsNames.Broid.TeamId, organization.Id).Result;
//            var primaryAccountId = _settingManager.GetSettingValueForOrganizationAsync(AppSettingsNames.Broid.PrimaryAccountId, organization.Id).Result;
//            var rootUrl = _settingManager.GetSettingValueForOrganizationAsync(AppSettingsNames.Application.RootUrl, organization.Id).Result;

//            // easily add HTTP Headers
//            request.AddHeader("Content-Type", "application/json");
//            request.AddHeader("Authorization", $"Bearer {token}");

//            request.AddJsonBody(new
//            {
//                team_id = teamId,
//                owner_id = primaryAccountId,
//                state = "enable",
//                name = organization.Name,
//                webhook_url = $"{rootUrl}/api/broid/income"

//            });

//            TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse>();

//            RestRequestAsyncHandle handle = _client.ExecuteAsync(request, r => taskCompletion.SetResult(r));

//            var response = await taskCompletion.Task;
//            JObject dyn = JObject.Parse(response.Content);

//            if (!Util.IsSuccessStatusCode(response.StatusCode))
//            {
//                throw new ApiException($"{dyn["type"]} : {dyn["message"]}", response.StatusCode);
//            }

//            await _settingManager.ChangeSettingForOrganizationAsync(AppSettingsNames.Broid.ApplicationId, dyn["id"].ToString(), organization.Id);
//        }

//        public async Task CreateApplicationKey(Organization organization)
//        {
//            var token = getAccountToken();

//            var request = new RestRequest("keys/applications", Method.POST);

//            var applicationId = await _settingManager.GetSettingValueForOrganizationAsync(AppSettingsNames.Broid.ApplicationId, organization.Id);

//            // easily add HTTP Headers
//            request.AddHeader("Content-Type", "application/json");
//            request.AddHeader("Authorization", $"Bearer {token}");

//            request.AddJsonBody(new
//            {
//                resource_id = long.Parse(applicationId),
//                state = "enable"
//            });

//            TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse>();

//            RestRequestAsyncHandle handle = _client.ExecuteAsync(request, r => taskCompletion.SetResult(r));

//            var response = await taskCompletion.Task;
//            JObject dyn = JObject.Parse(response.Content);

//            if (!Util.IsSuccessStatusCode(response.StatusCode))
//            {
//                throw new ApiException($"{dyn["type"]} : {dyn["message"]}", response.StatusCode);
//            }

//            await _settingManager.ChangeSettingForOrganizationAsync(AppSettingsNames.Broid.AppKeySecret, dyn["secret"].ToString(), organization.Id);
//            await _settingManager.ChangeSettingForOrganizationAsync(AppSettingsNames.Broid.AppKeyId, dyn["id"].ToString(), organization.Id);
//        }

//        public async Task<Integration> CreateIntegration(Integration integration)
//        {
//            var token = getAppToken(integration.OrganizationId);
//            var applicationId = await _settingManager.GetSettingValueForOrganizationAsync(AppSettingsNames.Broid.ApplicationId, integration.OrganizationId);

//            var request = new RestRequest("integrations", Method.POST);
            
//            // easily add HTTP Headers
//            request.AddHeader("Content-Type", "application/json");
//            request.AddHeader("Authorization", $"Bearer {token}");

//            request.AddJsonBody(new
//            {
//                application_id = long.Parse(applicationId),
//                state = "enable",
//                username = integration.Username,
//                token = integration.Token,
//                secret = integration.Secret

//            });

//            TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse>();

//            RestRequestAsyncHandle handle = _client.ExecuteAsync(request, r => taskCompletion.SetResult(r));

//            var response = await taskCompletion.Task;
//            JObject dyn = JObject.Parse(response.Content);

//            if (!Util.IsSuccessStatusCode(response.StatusCode))
//            {
//                throw new ApiException($"{dyn["type"]} : {dyn["message"]}", response.StatusCode);
//            }

//            integration.BroidId = dyn["id"].ToString();

//            return integration;
//        }

//        public async Task SendMessage(Integration integration, Channel channel, string message)
//        {
//            var token = getAppToken(integration.OrganizationId);

//            var request = new RestRequest("message", Method.POST);

//            // easily add HTTP Headers
//            request.AddHeader("Content-Type", "application/json");
//            request.AddHeader("Authorization", $"Bearer {token}");

//            request.AddJsonBody(new
//            {
//                message = new
//                {
//                    to = new
//                    {
//                        type = "Person",
//                        id = channel.Recipient
//                    },
//                    generator = new
//                    {
//                        type = "Service",
//                        id = integration.BroidId,
//                        name = integration.BroidName
//                    },
//                    @object = new
//                    {
//                        content = message,
//                        type = "Note"
//                    },
//                    @context = "https=//www.w3.org/ns/activitystreams",
//                    type = "Create"
//                }
//            });

//            TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse>();

//            RestRequestAsyncHandle handle = _client.ExecuteAsync(request, r => taskCompletion.SetResult(r));

//            var response = await taskCompletion.Task;
           
//            if (!Util.IsSuccessStatusCode(response.StatusCode))
//            {
//                JObject dyn = JObject.Parse(response.Content);
//                throw new ApiException($"{dyn["type"]} : {dyn["message"]}", response.StatusCode);
//            }
//        }

//        private string getAccountToken()
//        {
//            return _cache.GetOrCreate("broidToken", entry =>
//            {
//                entry.SlidingExpiration = TimeSpan.FromHours(2);
//                return generateAccountToken();
//            });
//        }

//        private string getAppToken(Guid orgId)
//        {
//            return _cache.GetOrCreate($"{orgId}_broidAppToken", entry =>
//            {
//                entry.SlidingExpiration = TimeSpan.FromHours(2);
//                return generateAppToken(orgId);
//            });
//        }
//        private string generateAccountToken()
//        {
//            var cacheEntry = _cache.GetOrCreate("", entry =>
//            {
//                entry.SlidingExpiration = TimeSpan.FromHours(2);
//                return DateTime.Now;
//            });

//            var secretId = _settingManager.GetSettingValueForApplicationAsync(AppSettingsNames.Broid.SecretId).Result;
//            var Secret = _settingManager.GetSettingValueForApplicationAsync(AppSettingsNames.Broid.Secret).Result;

//            TimeSpan t = DateTime.UtcNow.AddHours(2) - new DateTime(1970, 1, 1);
//            int secondsSinceEpoch = (int)t.TotalSeconds;

//            var payload = new Dictionary<string, object>()
//                {
//                    { "iss", secretId },
//                    { "exp",(int)t.TotalSeconds},
//                };

//            var header = new Dictionary<string, object>()
//                {
//                    { "typ", "JWT" },
//                };

//            var token = Jose.JWT.Encode(payload, Encoding.ASCII.GetBytes(Secret), Jose.JwsAlgorithm.HS256, extraHeaders: header);

//            return token;
//        }
//        private string generateAppToken(Guid orgId)
//        {
//            var cacheEntry = _cache.GetOrCreate("", entry =>
//            {
//                entry.SlidingExpiration = TimeSpan.FromHours(2);
//                return DateTime.Now;
//            });

//            var appKeyId = _settingManager.GetSettingValueForOrganizationAsync(AppSettingsNames.Broid.AppKeyId, orgId).Result;
//            var appKeySecret = _settingManager.GetSettingValueForOrganizationAsync(AppSettingsNames.Broid.AppKeySecret, orgId).Result;

//            TimeSpan t = DateTime.UtcNow.AddHours(2) - new DateTime(1970, 1, 1);
//            int secondsSinceEpoch = (int)t.TotalSeconds;

//            var payload = new Dictionary<string, object>()
//                {
//                    { "iss", appKeyId },
//                    { "exp",(int)t.TotalSeconds},
//                };

//            var header = new Dictionary<string, object>()
//                {
//                    { "typ", "JWT" },
//                };

//            var token = Jose.JWT.Encode(payload, Encoding.ASCII.GetBytes(appKeySecret), Jose.JwsAlgorithm.HS256, extraHeaders: header);

//            return token;
//        }
       
//    }
//}