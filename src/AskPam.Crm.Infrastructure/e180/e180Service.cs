using AskPam.Crm.Configuration;
using AskPam.Crm.Contacts;
using AskPam.Crm.Organizations;
using AskPam.Domain.Repositories;
using AskPam.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskPam.Crm.e180
{
    public class e180Service
    {
        private readonly IRepository<Organization, Guid> _orgRepo;
        private readonly IRepository<Contact> _contactRepo;
        private readonly ISettingManager _settingManager;

        public e180Service(IRepository<Organization, Guid> orgRepo, ISettingManager settingManager, IRepository<Contact> contactRepo)
        {
            _orgRepo = orgRepo;
            _contactRepo = contactRepo;
            _settingManager = settingManager;
        }

        public void Sync()
        {

            var organizations = _orgRepo.GetAll().Where(o => o.BrainDates).ToList();

            foreach (var organization in organizations)
            {
                var token = _settingManager.GetSettingValueForOrganizationAsync(AppSettingsNames.E180.E180Token, organization.Id).Result;
                var group = _settingManager.GetSettingValueForOrganizationAsync(AppSettingsNames.E180.E180Group, organization.Id).Result;

                if (token.IsNullOrEmpty() || group.IsNullOrEmpty())
                    return;

                var url = $"https://api.e-180.com/users/summary?group={group}&count=500";

                while (!url.IsNullOrEmpty())
                {
                    var client = new RestClient(url);
                    var request = new RestRequest(Method.GET);

                    request.AddHeader("Authorization", "Token "+token);

                    TaskCompletionSource<IRestResponse<BraindatesSummary>> taskCompletion = new TaskCompletionSource<IRestResponse<BraindatesSummary>>();

                    RestRequestAsyncHandle handle = client.ExecuteAsync<BraindatesSummary>(request, r => taskCompletion.SetResult(r));

                    var response = taskCompletion.Task.Result;
                    var braindates = response.Data;

                    foreach (var stats in braindates.results)
                    {
                        var contact = _contactRepo.GetAll()
                            .Where(c => c.EmailAddress == stats.email || c.EmailAddress2 == stats.email)
                            .Where(c=>c.OrganizationId==organization.Id)
                            .FirstOrDefault();

                        if (contact != null)
                        {
                            if (contact.CustomFields.IsNullOrEmpty())
                            {
                                var custom = new
                                {
                                    braindates_requests = stats.statistics.requests,
                                    braindates_confirmed = stats.statistics.confirmed,
                                    braindates_pendings = stats.statistics.pendings,
                                    braindates_offers = stats.statistics.offers
                                };

                                contact.UpdateField(nameof(contact.CustomFields), JsonConvert.SerializeObject(custom));
                            }
                            else
                            {
                                var custom = JObject.Parse(contact.CustomFields);
                                custom["braindates_requests"] = stats.statistics.requests;
                                custom["braindates_confirmed"] = stats.statistics.confirmed;
                                custom["braindates_pendings"] = stats.statistics.pendings;
                                custom["braindates_offers"] = stats.statistics.offers;

                                contact.UpdateField(nameof(contact.CustomFields), JsonConvert.SerializeObject(custom));
                            }

                            _contactRepo.Update(contact);
                        }
                    }

                    url = braindates.next;
                }
                _contactRepo.SaveChanges();
            }
        }
    }
}
