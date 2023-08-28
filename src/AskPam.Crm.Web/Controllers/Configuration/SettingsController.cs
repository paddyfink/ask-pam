using AskPam.Application.Dto;
using AskPam.Crm.Configuration.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AskPam.Crm.Runtime.Session;
using AutoMapper;
using AskPam.Domain.Repositories;
using AskPam.Extensions;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using AskPam.Crm.Settings;
using Microsoft.AspNetCore.Authorization;

namespace AskPam.Crm.Configuration
{
    [Authorize]
    [Route("api/[controller]")]
    public class SettingsController : BaseController
    {
        private readonly ISettingManager _settingsManager;

        public SettingsController(ISettingManager settingsManager, ICrmSession session, IMapper mapper) : base(session, mapper)
        {
            _settingsManager = settingsManager;
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(AppSettingsDto), 200)]
        public async Task<IActionResult> GetAppSettings()
        {
            var intercomAppId =
                await _settingsManager.GetSettingValueForApplicationAsync(AppSettingsNames.Application.IntercomAppId);
            var settings = new AppSettingsDto { IntercomAppId = intercomAppId };

            return new ObjectResult(settings);
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(IEnumerable<SettingDto>), 200)]
        public async Task<IActionResult> GetSettings([FromBody]SettingsRequestDto input)
        {

            if (!input.UserId.IsNullOrEmpty())
                return new ObjectResult((await _settingsManager.GetAllSettingValuesAsync(SettingScopes.User, input.OrganizationId, input.UserId))
                .Select(s => new SettingDto { Name = s.Name, Value = s.Value, OrganizationId = input.OrganizationId, UserId = input.UserId })
                );
            else if (input.OrganizationId.HasValue)
                return new ObjectResult((await _settingsManager.GetAllSettingValuesAsync(SettingScopes.Organization, input.OrganizationId.Value))
            .Select(s => new SettingDto { Name = s.Name, Value = s.Value, OrganizationId = input.OrganizationId }));

            else return new ObjectResult((await _settingsManager.GetAllSettingValuesAsync(SettingScopes.Application))
            .Select(s => new SettingDto { Name = s.Name, Value = s.Value }));

        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateSettings([FromBody]SettingDto setting)
        {
            if (!setting.UserId.IsNullOrEmpty())
                await _settingsManager.ChangeSettingForUserAsync(setting.UserId, setting.OrganizationId, setting.Name, setting.Value);
            else if (setting.OrganizationId.HasValue)
                await _settingsManager.ChangeSettingForOrganizationAsync(setting.Name, setting.Value, setting.OrganizationId.Value);
            else
                await _settingsManager.ChangeSettingForApplicationAsync(setting.Name, setting.Value);

            return Ok();
        }
    }
}
