using AskPam.Crm.Authorization;
using AskPam.Crm.Contacts;
using AskPam.Crm.Organizations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AskPam.Crm.Conversations
{
    public interface ISmoochConversationService
    {
        Task<string> PostMessageAsync(Conversation conversation, Message message);
        Task DeleteIntegration(Organization organization, string id);

        Task CreateOrganizationApp(Organization organization);
        Task GenerateOrganizationAppKeys(Organization organization);
        Task CreateWebHook(Organization organization);
    }
}
