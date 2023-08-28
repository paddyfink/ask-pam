using AskPam.Crm.Helpers;
using AskPam.Crm.Runtime.Session;
using AskPam.Domain.Repositories;
using AskPam.Domain.Services;
using AskPam.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskPam.Crm.Configuration
{
    public class SettingManager : ISettingManager
    {
        public const string ApplicationSettingsCacheKey = "ApplicationSettings";
        public const string OrganizationSettingsCacheKey = "OrganizationSettings";
        public const string UserSettingsCacheKey = "UserSettings";

        private readonly ISettingStore _settingStore;
        private readonly ICrmSession _session;
        private readonly ISettingDefinitionManager _settingDefinitionManager;
        private readonly IMemoryCache _cache;


        /// <summary>
        /// Constructor.
        /// </summary>
        public SettingManager(
            IRepository<Setting, long> settingRepository,
            ICrmSession session,
            ISettingDefinitionManager settingDefinitionManager,
            IMemoryCache memoryCache, ISettingStore settingStore)
        {
            _session = session;
            _settingDefinitionManager = settingDefinitionManager;
            _cache = memoryCache;
            _settingStore = settingStore;
        }

        #region Public methods

        /// <inheritdoc/>
        public async Task<string> GetSettingValueAsync(string name)
        {
            return await GetSettingValueInternalAsync(name, _session.OrganizationId, _session.UserId);
        }

        public async Task<string> GetSettingValueForApplicationAsync(string name)
        {
            return await GetSettingValueInternalAsync(name);
        }

        public async Task<string> GetSettingValueForOrganizationAsync(string name, Guid organizationId)
        {
            return await GetSettingValueInternalAsync(name, organizationId);
        }

        public async Task<string> GetSettingValueForUserAsync(string name, Guid? organizationId, string userId)
        {
            return await GetSettingValueInternalAsync(name, organizationId, userId);
        }

        public async Task<IReadOnlyList<SettingValue>> GetAllSettingValuesAsync()
        {
            return await GetAllSettingValuesAsync(SettingScopes.Application | SettingScopes.Organization | SettingScopes.User, _session.OrganizationId, _session.UserId);
        }


        public async Task<IReadOnlyList<SettingValue>> GetAllSettingValuesAsync(SettingScopes scopes, Guid? organizationId = null, string userId = null, bool? isVisibleToClients = null)
        {
            var settingDefinitions = new Dictionary<string, SettingDefinition>();
            var settingValues = new Dictionary<string, SettingValue>();

            //Fill all setting with default values.

            foreach (var setting in _settingDefinitionManager.GetAllSettingDefinitions())
            {
                if (setting.Scopes.HasFlag(scopes) && (!isVisibleToClients.HasValue || setting.IsVisibleToClients == isVisibleToClients))
                {
                    settingDefinitions[setting.Name] = setting;
                    settingValues[setting.Name] = new SettingValue(setting.Name, setting.DefaultValue);
                }
            }

            //Overwrite application settings
            if (scopes.HasFlag(SettingScopes.Application))
            {
                foreach (var settingValue in await GetAllSettingValuesForApplicationAsync())
                {
                    var setting = settingDefinitions.GetOrDefault(settingValue.Name);

                    //TODO: Conditions get complicated, try to simplify it
                    if (setting == null || !setting.Scopes.HasFlag(SettingScopes.Application))
                    {
                        continue;
                    }

                    if (!setting.IsInherited &&
                        ((setting.Scopes.HasFlag(SettingScopes.Organization) && organizationId.HasValue) || (setting.Scopes.HasFlag(SettingScopes.User) && !userId.IsNullOrEmpty())))
                    {
                        continue;
                    }

                    settingValues[settingValue.Name] = new SettingValue(settingValue.Name, settingValue.Value);
                }
            }

            //Overwrite organization settings
            if (scopes.HasFlag(SettingScopes.Organization) && organizationId.HasValue)
            {
                foreach (var settingValue in await GetAllSettingValuesForOrganizationAsync(organizationId.Value))
                {
                    var setting = settingDefinitions.GetOrDefault(settingValue.Name);

                    //TODO: Conditions get complicated, try to simplify it
                    if (setting == null || !setting.Scopes.HasFlag(SettingScopes.Organization))
                    {
                        continue;
                    }

                    if (!setting.IsInherited &&
                        (setting.Scopes.HasFlag(SettingScopes.User) && !userId.IsNullOrEmpty()))
                    {
                        continue;
                    }

                    settingValues[settingValue.Name] = new SettingValue(settingValue.Name, settingValue.Value);
                }
            }

            //Overwrite user settings
            if (scopes.HasFlag(SettingScopes.User) && !_session.UserId.IsNullOrEmpty())
            {
                foreach (var settingValue in await GetAllSettingValuesForUserAsync(userId, organizationId))
                {
                    var setting = settingDefinitions.GetOrDefault(settingValue.Name);
                    if (setting != null && setting.Scopes.HasFlag(SettingScopes.User))
                    {
                        settingValues[settingValue.Name] = new SettingValue(settingValue.Name, settingValue.Value);
                    }
                }
            }

            return settingValues.Values.ToImmutableList();
        }


        public async Task<IReadOnlyList<SettingValue>> GetAllSettingValuesForApplicationAsync()
        {
            return (await GetApplicationSettingsAsync()).Values
                  .Select(setting => new SettingValue(setting.Name, setting.Value))
                  .ToImmutableList();
        }


        public async Task<IReadOnlyList<SettingValue>> GetAllSettingValuesForOrganizationAsync(Guid organizationId)
        {
            return (await GetOrganizationSettingsAsync(organizationId)).Values
                  .Select(setting => new SettingValue(setting.Name, setting.Value))
                  .ToImmutableList();
        }


        public async Task<IReadOnlyList<SettingValue>> GetAllSettingValuesForUserAsync(string userId, Guid? organizationId)
        {
            return (await GetUserSettingsAsync(userId, organizationId)).Values
                .Select(setting => new SettingValue(setting.Name, setting.Value))
                .ToImmutableList();
        }


        public async Task ChangeSettingForApplicationAsync(string name, string value)
        {
            await InsertOrUpdateOrDeleteSettingValueAsync(name, value, null, null);
            _cache.Remove(ApplicationSettingsCacheKey);
        }



        public async Task ChangeSettingForOrganizationAsync(string name, string value, Guid organizationId)
        {
            await InsertOrUpdateOrDeleteSettingValueAsync(name, value, organizationId, null);
            _cache.Remove($"{OrganizationSettingsCacheKey}_{organizationId}");
        }



        public async Task ChangeSettingForUserAsync(string userId, Guid? OrganizationId, string name, string value)
        {
            await InsertOrUpdateOrDeleteSettingValueAsync(name, value, OrganizationId, userId);
            _cache.Remove($"{UserSettingsCacheKey}_{userId}");
        }

        #endregion

        #region Private methods
        private async Task<string> GetSettingValueInternalAsync(string name, Guid? organizationId = null, string userId = null)
        {
            var settingDefinition = _settingDefinitionManager.GetSettingDefinition(name);

            //Get for user if defined
            if (settingDefinition.Scopes.HasFlag(SettingScopes.User) && !userId.IsNullOrEmpty())
            {
                var settingValue = await _settingStore.GetSettingOrNullAsync(organizationId, userId, name);
                if (settingValue != null)
                {
                    return settingValue.Value;
                }

                if (!settingDefinition.IsInherited)
                {
                    return settingDefinition.DefaultValue;
                }
            }

            //Get for organization if defined
            if (settingDefinition.Scopes.HasFlag(SettingScopes.Organization) && organizationId.HasValue)
            {
                var settingValue = await _settingStore.GetSettingOrNullAsync(organizationId.Value, null, name);
                if (settingValue != null)
                {
                    return settingValue.Value;
                }

                if (!settingDefinition.IsInherited)
                {
                    return settingDefinition.DefaultValue;
                }
            }

            //Get for application if defined
            if (settingDefinition.Scopes.HasFlag(SettingScopes.Application))
            {
                var settingValue = await _settingStore.GetSettingOrNullAsync(null, null, name);
                if (settingValue != null)
                {
                    return settingValue.Value;
                }
            }

            //Not defined, get default value
            return settingDefinition.DefaultValue;
        }

        private async Task<Dictionary<string, Setting>> GetApplicationSettingsAsync()
        {

            var dictionary = new Dictionary<string, Setting>();

            var settingValues = await _cache.GetOrCreateAsync<List<Setting>>(ApplicationSettingsCacheKey, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromDays(1);
                return _settingStore.GetAllListAsync(null, null);
            });

            foreach (var settingValue in settingValues)
            {
                dictionary[settingValue.Name] = settingValue;
            }

            return dictionary;
        }

        private async Task<Dictionary<string, Setting>> GetOrganizationSettingsAsync(Guid organizationId)
        {

            var dictionary = new Dictionary<string, Setting>();

            var settingValues = await _cache.GetOrCreateAsync<List<Setting>>($"{OrganizationSettingsCacheKey}_{organizationId}", entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromDays(1);
                return _settingStore.GetAllListAsync(organizationId, null);
            });

            foreach (var settingValue in settingValues)
            {
                dictionary[settingValue.Name] = settingValue;
            }

            return dictionary;
        }


        private async Task<Dictionary<string, Setting>> GetUserSettingsAsync(string userId, Guid? organizationId)
        {

            var dictionary = new Dictionary<string, Setting>();

            var settingValues = await _cache.GetOrCreateAsync<List<Setting>>($"{UserSettingsCacheKey}_{userId}", entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromDays(1);
                return _settingStore.GetAllListAsync(organizationId, userId);
            });

            foreach (var settingValue in settingValues)
            {
                dictionary[settingValue.Name] = settingValue;
            }

            return dictionary;
        }

        private async Task<Setting> GetSettingValueForApplicationOrNullAsync(string name)
        {
            return (await GetApplicationSettingsAsync()).GetOrDefault(name);
        }

        private async Task<Setting> GetSettingValueForOrganizationOrNullAsync(Guid organizationId, string name)
        {
            return (await GetOrganizationSettingsAsync(organizationId)).GetOrDefault(name);
        }

        private async Task<Setting> GetSettingValueForUserOrNullAsync(string userId, Guid? organizationId, string name)
        {
            return (await GetUserSettingsAsync(userId, organizationId)).GetOrDefault(name);
        }

        private async Task<Setting> InsertOrUpdateOrDeleteSettingValueAsync(string name, string value, Guid? organizationId, string userId)
        {
            var settingDefinition = _settingDefinitionManager.GetSettingDefinition(name);
            var settingValue = await _settingStore.GetSettingOrNullAsync(organizationId, userId, name);

            //Determine defaultValue
            var defaultValue = settingDefinition.DefaultValue;

            if (settingDefinition.IsInherited)
            {
                //For organization and User, Application's value overrides Setting Definition's default value.
                if (!organizationId.HasValue || userId.IsNullOrEmpty())
                {
                    var applicationValue = await GetSettingValueForApplicationOrNullAsync(name);
                    if (applicationValue != null)
                    {
                        defaultValue = applicationValue.Value;
                    }
                }

                //For User, organization's value overrides Application's default value.
                if (userId.IsNullOrEmpty() && organizationId.HasValue)
                {
                    var organizationValue = await GetSettingValueForOrganizationOrNullAsync(organizationId.Value, name);
                    if (organizationValue != null)
                    {
                        defaultValue = organizationValue.Value;
                    }
                }
            }

            //No need to store on database if the value is the default value
            if (value == defaultValue)
            {
                if (settingValue != null)
                {
                    await _settingStore.DeleteAsync(settingValue);
                }

                return null;
            }

            //If it's not default value and not stored on database, then insert it
            if (settingValue == null)
            {
                settingValue = new Setting
                {
                    OrganizationId = organizationId,
                    UserId = userId,
                    Name = name,
                    Value = value
                };

                await _settingStore.CreateAsync(settingValue);
                return settingValue;
            }

            //It's same value in database, no need to update
            if (settingValue.Value == value)
            {
                return settingValue;
            }

            //Update the setting on database.
            settingValue.Value = value;
            await _settingStore.UpdateAsync(settingValue);

            return settingValue;
        }




        #endregion

        #region Nested classes

        public class SettingValue
        {
            public string Name { get; private set; }

            public string Value { get; private set; }

            public SettingValue(string name, string value)
            {
                Value = value;
                Name = name;
            }
        }

        #endregion
    }
}
