using AskPam.Crm.Conversations;
using AskPam.Events;
using AskPam.Crm.Organizations.events;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AskPam.Crm.Organizations
{
    class OrganizationEventHandler : IEventHandler<OrganizationCreated>
    {
        private readonly ISmoochConversationService _smoochService;

        public OrganizationEventHandler(ISmoochConversationService smoochService)
        {
            _smoochService = smoochService;
        }


        public async Task Handle(OrganizationCreated args)
        {
            var organization = args.Orgnization;

            await _smoochService.CreateOrganizationApp(organization);

            await _smoochService.GenerateOrganizationAppKeys(organization);

            await _smoochService.CreateWebHook(organization);
        }
    }
}
