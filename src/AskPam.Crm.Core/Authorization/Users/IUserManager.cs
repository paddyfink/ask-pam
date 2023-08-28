using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AskPam.Crm.Organizations;
using AskPam.Domain.Services;

namespace AskPam.Crm.Authorization
{
    public interface IUserManager : IDomainService
    {
        Task AddToRole(User user, string roleName, Guid organizationId);
        Task<Token> AuthenticateUser(string email, string password);
        Task ChangePassword(User user, string password, Guid organisationId);
        Task<User> CreateOrAddtoOrganization(string firstname, string lastname, string email, string roleName, Guid organizationId);
        Task<User> FindByEmailAsync(string email);
        Task<User> FindByIdAsync(string userId);
        Task ForgotPassword(string email);
        IQueryable<Role> GetAllRoles(Guid organizationId);
        IQueryable<UserRole> GetAllUserRoles(Guid organizationId);
        IQueryable<User> GetAllUsers(Guid organizationId);
        Task<Organization> GetDefaultOrganizationAsync(string userId);
        Task<IEnumerable<Organization>> GetOrganizationsAsync(string userId);
        Task<IEnumerable<string>> GetRolesAsync(string userId, Guid? organizationId = null);
        Task<User> GetUserInfo(string token);
        Task<IList<User>> GetUsersInRoleAsync(Guid organizationId, string roleName);
        Task<User> ResetProfilePicture(User user);
        Task SetDefaultOrganizationAsync(string userId, Guid organizationId);
        Task<User> UpdateEmailSettings(User user, string signature);
        Task<User> UpdateProfile(User user, string firstName, string lastName);
        Task<User> UpdateProfilePicture(User user, string picture);
        Task RemoveUSer(Guid organizationId, string userId);
    }
}