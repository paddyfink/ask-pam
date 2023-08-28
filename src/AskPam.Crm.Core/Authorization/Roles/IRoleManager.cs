using System;
using System.Threading.Tasks;
using AskPam.Crm.Organizations;
using AskPam.Domain.Services;

namespace AskPam.Crm.Authorization
{
    public interface IRoleManager: IDomainService
    {
        Task AddUserToRole(Role role, User user, Organization organization);
        Task CreateStaticRoles(Guid organizationId);
        Task<Role> FindByName(Organization org, string roleName);
    }
}