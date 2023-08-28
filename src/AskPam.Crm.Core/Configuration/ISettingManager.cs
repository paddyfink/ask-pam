using AskPam.Domain.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AskPam.Crm.Configuration
{
    public interface ISettingManager : IDomainService
    {
        Task ChangeSettingForApplicationAsync(string name, string value);
        Task ChangeSettingForOrganizationAsync(string name, string value, Guid organizationId);
        Task ChangeSettingForUserAsync(string userId, Guid? OrganizationId, string name, string value);
        Task<IReadOnlyList<SettingManager.SettingValue>> GetAllSettingValuesAsync();
        Task<IReadOnlyList<SettingManager.SettingValue>> GetAllSettingValuesAsync(SettingScopes scopes, Guid? organizationId = null, string userId = null, bool? isVisibleToClient = null);
        Task<IReadOnlyList<SettingManager.SettingValue>> GetAllSettingValuesForApplicationAsync();
        Task<IReadOnlyList<SettingManager.SettingValue>> GetAllSettingValuesForOrganizationAsync(Guid organizationId);
        Task<IReadOnlyList<SettingManager.SettingValue>> GetAllSettingValuesForUserAsync(string userId, Guid? organizationId);
        Task<string> GetSettingValueAsync(string name);
        Task<string> GetSettingValueForApplicationAsync(string name);
        Task<string> GetSettingValueForOrganizationAsync(string name, Guid organizationId);
        Task<string> GetSettingValueForUserAsync(string name, Guid? organizationId, string userId);
    }
}