using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AskPam.Domain.Services;

namespace AskPam.Crm.Configuration
{
    public interface ISettingStore: IDomainService
    {
        /// <summary>
        /// Gets a setting or null.
        /// </summary>
        /// <param name="organizationId">organizationId or null</param>
        /// <param name="userId">UserId or null</param>
        /// <param name="name">Name of the setting</param>
        /// <returns>Setting object</returns>
        Task<Setting> GetSettingOrNullAsync(Guid? organizationId, string userId, string name);

        /// <summary>
        /// Deletes a setting.
        /// </summary>
        /// <param name="setting">Setting to be deleted</param>
        Task DeleteAsync(Setting setting);

        /// <summary>
        /// Adds a setting.
        /// </summary>
        /// <param name="setting">Setting to add</param>
        Task CreateAsync(Setting setting);

        /// <summary>
        /// Update a setting.
        /// </summary>
        /// <param name="setting">Setting to add</param>
        Task UpdateAsync(Setting setting);

        /// <summary>
        /// Gets a list of setting.
        /// </summary>
        /// <param name="organizationId">TenantId or null</param>
        /// <param name="userId">UserId or null</param>
        /// <returns>List of settings</returns>
        Task<List<Setting>> GetAllListAsync(Guid? organizationId, string userId);
    }
}
