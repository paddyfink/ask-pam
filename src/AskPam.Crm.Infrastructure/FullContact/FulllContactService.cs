using AskPam.Crm.Contacts;
using AskPam.Extensions;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AskPam.Crm.FullContact
{
    public class FulllContactService : IFullContactService
    {
        public async Task<string> Search(string email = null, string phone = null)
        {

            var client = new RestClient("https://api.fullcontact.com/v2");

            var request = new RestRequest("person.json", Method.GET);


            // easily add HTTP Headers
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("X-FullContact-APIKey", "f0d439669d63f20e");

            if (!email.IsNullOrEmpty())
                request.AddQueryParameter("email", email);

            if (!phone.IsNullOrEmpty())
                request.AddQueryParameter("phone", phone);

            TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse>();

            RestRequestAsyncHandle handle = client.ExecuteAsync(request, r => taskCompletion.SetResult(r));

            var response = await taskCompletion.Task;

            JObject dyn = JObject.Parse(response.Content);

            if (dyn["status"].ToString() == "200")
            {
                return response.Content;
            }
            else
                return string.Empty;
        }

    }
}
