using AskPam.Crm.Authorization;
using AskPam.Crm.Configuration;
using AskPam.Crm.Conversations;
using AskPam.Exceptions;
using AskPam.Events;
using AskPam.Crm.Organizations;
using AskPam.Crm.Organizations.events;
using AskPam.Crm.Settings;
using AskPam.Crm.Smooch.Model;
using AskPam.Domain.Repositories;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AskPam.Crm.Common;
using AskPam.Crm.Contacts;
using Conversation = AskPam.Crm.Conversations.Conversation;

namespace AskPam.Crm.Smooch
{
    public class SmoochConversationService : ISmoochConversationService
    {

        //private string _token;
        private readonly IRepository<Organization, Guid> _orgRepository;
        private readonly IDomainEvents _domainEvents;
        private readonly ISettingManager _settingManager;
        private RestClient client;

        public SmoochConversationService(IRepository<Organization, Guid> orgRepository, IDomainEvents domainEvents, ISettingManager settingManager)
        {
            _orgRepository = orgRepository;
            _domainEvents = domainEvents;
            _settingManager = settingManager;
            client =  new RestClient("https://api.smooch.io/v1");
        }
        public async Task<string> PostMessageAsync(Conversation conversation, Conversations.Message message)
        {
            var org = await _orgRepository.GetAsync(conversation.OrganizationId);
            var token = await GetAppToken(org);
            
            var request = new RestRequest("appusers/{userId}/messages", Method.POST);

            request.AddUrlSegment("userId", conversation.SmoochUserId); // replaces matching token in request.Resource

            // easily add HTTP Headers
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer {token}");

            request.AddJsonBody(new
            {
                text = message.Text,
                role = "appMaker",
                name = message.Author,
                type = "text",
                authorId = message.AuthorId,
                avatarUrl = message.Avatar
            });


            var taskCompletion = new TaskCompletionSource<IRestResponse<AppMakerMessage>>();

            client.ExecuteAsync<AppMakerMessage>(request, r => taskCompletion.SetResult(r));

            var response = await taskCompletion.Task;

            CheckError(response);

            return response.Data.message._id;
        }



        //public async Task<IEnumerable<Integration>> GetIntegrations(Organization organization)
        //{


        //    var token = await getAppToken(organization);
        //    
        //    // client.Authenticator = new HttpBasicAuthenticator(username, password);

        //    var request = new RestRequest("apps/{appId}/integrations", Method.GET);

        //    request.AddUrlSegment("appId", organization.SmoochAppId); // replaces matching token in request.Resource

        //    // easily add HTTP Headers
        //    request.AddHeader("Content-Type", "application/json");
        //    request.AddHeader("Authorization", $"Bearer {token}");



        //    TaskCompletionSource<IRestResponse<IntegrationList>> taskCompletion = new TaskCompletionSource<IRestResponse<IntegrationList>>();

        //    RestRequestAsyncHandle handle = client.ExecuteAsync<IntegrationList>(request, r => taskCompletion.SetResult(r));

        //    var response = await taskCompletion.Task;


        //    return response.Data.integrations;
        //}

        //public async Task DeleteIntegration(Organization organization, string id)
        //{


        //    var token = getAppToken(organization);
        //    
        //    // client.Authenticator = new HttpBasicAuthenticator(username, password);

        //    var request = new RestRequest("apps/{appId}/integrations/{integrationId}", Method.DELETE);

        //    request.AddUrlSegment("appId", organization.SmoochAppId); 
        //    request.AddUrlSegment("integrationId", id); 

        //    // easily add HTTP Headers
        //    request.AddHeader("Content-Type", "application/json");
        //    request.AddHeader("Authorization", $"Bearer {token}");

        //    TaskCompletionSource<IRestResponse<IntegrationList>> taskCompletion = new TaskCompletionSource<IRestResponse<IntegrationList>>();

        //    RestRequestAsyncHandle handle = client.ExecuteAsync<IntegrationList>(request, r => taskCompletion.SetResult(r));

        //    await taskCompletion.Task;


        //}

        //public async Task<Integration> CreateFacebookIntegration(Organization organization, Integration integration)
        //{
        //    var token = getAppToken(organization);
        //    

        //    var request = new RestRequest("apps/{appId}/integrations", Method.POST);

        //    request.AddUrlSegment("appId", organization.SmoochAppId);

        //    // easily add HTTP Headers
        //    request.AddHeader("Content-Type", "application/json");
        //    request.AddHeader("Authorization", $"Bearer {token}");

        //    request.AddJsonBody(new
        //    {
        //        type = "messenger",
        //        pageAccessToken = integration.PageAccessToken,
        //        appId = integration.AppId,
        //        appSecret = integration.AppSecret
        //    });

        //    TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse>();

        //    RestRequestAsyncHandle handle = client.ExecuteAsync<IntegrationList>(request, r => taskCompletion.SetResult(r));

        //    var response = await taskCompletion.Task;

        //    JObject dyn = JObject.Parse(response.Content);


        //    if (dyn["error"] != null)
        //    {
        //        throw new ApiException(dyn["error"]["description"].ToString());
        //    }
        //    integration.PageId= dyn["integration"]["pageId"].ToString();
        //    integration.Id = dyn["integration"]["_id"].ToString();

        //    return integration;
        //}

        public async Task CreateOrganizationApp(Organization organization)
        {
            var token = await GetAccountToken();

            var request = new RestRequest("apps", Method.POST);

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer {token}");

            request.AddJsonBody(new
            {
                name = organization.Name
            });

            var taskCompletion = new TaskCompletionSource<IRestResponse>();

            var handle = client.ExecuteAsync(request, r => taskCompletion.SetResult(r));

            var response = await taskCompletion.Task;

            var dyn = JObject.Parse(response.Content);

            await _settingManager.ChangeSettingForOrganizationAsync(AppSettingsNames.Smooch.AppId,
                dyn["app"]["_id"].ToString(), organization.Id);
            await _settingManager.ChangeSettingForOrganizationAsync(AppSettingsNames.Smooch.AppToken,
                dyn["app"]["appToken"].ToString(), organization.Id);

           
        }

        public async Task GenerateOrganizationAppKeys(Organization organization)
        {
            var token = await GetAccountToken();
            
            var request = new RestRequest("apps/{appId}/keys", Method.POST);
            var smoochAppId =
                 await _settingManager.GetSettingValueForOrganizationAsync(AppSettingsNames.Smooch.AppId, organization.Id);

            request.AddUrlSegment("appId", smoochAppId); // replaces matching token in request.Resource

            // easily add HTTP Headers
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer {token}");

            request.AddJsonBody(new
            {
                name = "key"
            });

            var taskCompletion = new TaskCompletionSource<IRestResponse>();

            RestRequestAsyncHandle handle = client.ExecuteAsync(request, r => taskCompletion.SetResult(r));

            var response = await taskCompletion.Task;

            JObject dyn = JObject.Parse(response.Content);

            await _settingManager.ChangeSettingForOrganizationAsync(AppSettingsNames.Smooch.AppKeyId, dyn["key"]["_id"].ToString(), organization.Id);
            await _settingManager.ChangeSettingForOrganizationAsync(AppSettingsNames.Smooch.AppSecret, dyn["key"]["secret"].ToString(), organization.Id);
        }

        public async Task CreateWebHook(Organization organization)
        {
            var token = await GetAppToken(organization);
            
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest("webhooks", Method.POST);

            // easily add HTTP Headers
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer {token}");

            request.AddJsonBody(new
            {
                target = "https://app.ask-pam.com/api/smooch/income",
                triggers = new[] { "*" }
            });

            var taskCompletion = new TaskCompletionSource<IRestResponse>();

            client.ExecuteAsync(request, r => taskCompletion.SetResult(r));

            var response = await taskCompletion.Task;

            //dynamic dyn = JObject.Parse(response.Content);            
            //organization.webhooksecret = dyn["webhook"]["secret"];
            //return organization;
        }

        public Task DeleteIntegration(Organization organization, string id)
        {
            throw new NotImplementedException();
        }


        #region Private

        private async Task<string> GetAppToken(Organization org)
        {
            var smoochAppKeyId = await _settingManager.GetSettingValueForOrganizationAsync(AppSettingsNames.Smooch.AppKeyId, org.Id);
            var smoochAppSecret = await _settingManager.GetSettingValueForOrganizationAsync(AppSettingsNames.Smooch.AppSecret, org.Id);


            var payload = new Dictionary<string, object>()
                {
                    { "scope", "app" },
                    { "iss", "Ask PAM" },
                    { "aud", "www.ask-pam.com" },
                    { "sub", "support@ask-pam.com" },
                };

            var header = new Dictionary<string, object>()
                {
                    { "kid", smoochAppKeyId },
                    { "typ", "JWT" },
                };

            var token = Jose.JWT.Encode(payload, Encoding.ASCII.GetBytes(smoochAppSecret), Jose.JwsAlgorithm.HS256, extraHeaders: header);

            return token;
        }

        private async Task<string> GetAccountToken()
        {
            var smoochKeyId = await _settingManager.GetSettingValueForApplicationAsync(AppSettingsNames.Smooch.AppKeyId);
            var smoochSecret = await _settingManager.GetSettingValueForApplicationAsync(AppSettingsNames.Smooch.AppSecret);

            var payload = new Dictionary<string, object>()
                {
                    { "scope", "account" },
                    { "iss", "Ask PAM" },
                    { "aud", "www.ask-pam.com" },
                    { "sub", "support@ask-pam.com" },
                };

            var header = new Dictionary<string, object>()
                {
                    { "kid", smoochKeyId},
                    { "typ", "JWT" },
                };

            var token = Jose.JWT.Encode(payload, Encoding.ASCII.GetBytes(smoochSecret), Jose.JwsAlgorithm.HS256, extraHeaders: header);

            return token;
        }

        private static void CheckError(IRestResponse response)
        {
            var dyn = JObject.Parse(response.Content);

            if (!Util.IsSuccessStatusCode(response.StatusCode))
            {
                throw new ApiException($"{dyn["error"]["code"]} : {dyn["error"]["description"]}", response.StatusCode);
            }
        }

        #endregion
    }
}
