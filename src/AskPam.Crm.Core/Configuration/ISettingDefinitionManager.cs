using AskPam.Domain.Services;
using System.Collections.Generic;

namespace AskPam.Crm.Configuration
{
    public interface ISettingDefinitionManager: IDomainService
    {
        IReadOnlyList<SettingDefinition> GetAllSettingDefinitions();
        SettingDefinition GetSettingDefinition(string name);
    }
}