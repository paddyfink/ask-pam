using AskPam.Crm.Twilio.Entities;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AskPam.Crm.Configuration;
using AskPam.Crm.Common;
using AskPam.Exceptions;
using Newtonsoft.Json.Linq;

namespace AskPam.Crm.Twilio
{
    public class TwilioPhoneNumberLookupService : IPhoneNumberLookupService
    {
        private ISettingManager _settingManager;

        public TwilioPhoneNumberLookupService(ISettingManager settingManager)
        {
            _settingManager = settingManager;
        }
        public async Task<Phone> Format(string phoneNumber, bool forceValidation = true)
        {
            var accountSid = await _settingManager.GetSettingValueForApplicationAsync(AppSettingsNames.Twilio.AccountSid);
            var token = await _settingManager.GetSettingValueForApplicationAsync(AppSettingsNames.Twilio.AuthToken);

            var client = new RestClient
            {
                BaseUrl = new Uri($"https://lookups.twilio.com/v1/PhoneNumbers/{phoneNumber}"),
                Authenticator = new HttpBasicAuthenticator(accountSid, token)
            };

            var request = new RestRequest("", Method.GET);
            request.AddHeader("Content-Type", "application/json");

            TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse>();

            RestRequestAsyncHandle handle = client.ExecuteAsync(request, r => taskCompletion.SetResult(r));

            RestResponse response = (RestResponse)(await taskCompletion.Task);
            JObject dyn = JObject.Parse(response.Content);

            if (!Util.IsSuccessStatusCode(response.StatusCode))
            {
                if (forceValidation)
                    throw new ApiException($"{phoneNumber} is not valid phone number", response.StatusCode);
                else
                    return new Phone(phoneNumber);
            }

            var twilioResponse = JsonConvert.DeserializeObject<TwilioLookUpResponse>(response.Content);

            return new Phone(
                twilioResponse.phone_number,
                twilioResponse.country_code?.ToLower(),
                twilioResponse.national_format
            );
        }

    }
}
