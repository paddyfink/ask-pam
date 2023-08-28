using AskPam.Domain.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AskPam.Crm.Organizations
{
    public interface IOrganizationManager: IDomainService
    {
        Task<Organization> AddEmailChannel(Organization org, string email, string name);
        Task<Organization> CreateOrganization(Organization organization);
        Task<Organization> FindByIdAsync(Guid organizationId);
        Task<IEnumerable<string>> GetOrganizationAdmins(Organization org);
    }
}