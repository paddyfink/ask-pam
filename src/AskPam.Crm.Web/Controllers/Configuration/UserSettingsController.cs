using AskPam.Crm.Configuration;
using AskPam.Crm.Controllers.Configuration.Dtos;
using AskPam.Crm.Runtime.Session;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.Controllers.Configuration
{
    [Route("api/[controller]/[action]")]
    public class UserSettingsController : BaseController
    {
        private ISettingManager _settingManager;

        public UserSettingsController(
            ICrmSession session, IMapper mapper,
            ISettingManager settingManager
        ) : base(session, mapper)
        {
            _settingManager = settingManager;
        }

        [HttpGet]
        [ProducesResponseType(typeof(EmailNotificationSettingsDto), 200)]
        public async Task<IActionResult> GetEmailNotificationSettings()
        {
            EnsureOrganization();

            var settings = new EmailNotificationSettingsDto
            {
                NewConversation = bool.Parse(await _settingManager.GetSettingValueForUserAsync(AppSettingsNames.Notification.EmailNewConversation, Session.OrganizationId.Value, Session.UserId)),
                NewMessage = bool.Parse(await _settingManager.GetSettingValueForUserAsync(AppSettingsNames.Notification.EmailNewMessage, Session.OrganizationId.Value, Session.UserId)),
                MessageSent = bool.Parse(await _settingManager.GetSettingValueForUserAsync(AppSettingsNames.Notification.EmailMessageSent, Session.OrganizationId.Value, Session.UserId)),
                ConversationAssigned = bool.Parse(await _settingManager.GetSettingValueForUserAsync(AppSettingsNames.Notification.EmailConversationAssigned, Session.OrganizationId.Value, Session.UserId)),
                ContactAssigned = bool.Parse(await _settingManager.GetSettingValueForUserAsync(AppSettingsNames.Notification.EmailContactAssigned, Session.OrganizationId.Value, Session.UserId)),
                ConversationFollowed = bool.Parse(await _settingManager.GetSettingValueForUserAsync(AppSettingsNames.Notification.EmailConversationFollowed, Session.OrganizationId.Value, Session.UserId)),
                LibraryItemCreated = bool.Parse(await _settingManager.GetSettingValueForUserAsync(AppSettingsNames.Notification.EmailLibraryItemCreated, Session.OrganizationId.Value, Session.UserId)),
                ConversationFlagged = bool.Parse(await _settingManager.GetSettingValueForUserAsync(AppSettingsNames.Notification.EmailConversationflagged, Session.OrganizationId.Value, Session.UserId)),
            };

            return new ObjectResult(settings);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEmailNotificationSettings([FromBody] EmailNotificationSettingsDto dto)
        {
            EnsureOrganization();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _settingManager.ChangeSettingForUserAsync(Session.UserId, Session.OrganizationId.Value, AppSettingsNames.Notification.EmailNewConversation, dto.NewMessage.ToString());
            await _settingManager.ChangeSettingForUserAsync(Session.UserId, Session.OrganizationId.Value, AppSettingsNames.Notification.EmailNewMessage, dto.NewMessage.ToString());
            await _settingManager.ChangeSettingForUserAsync(Session.UserId, Session.OrganizationId.Value, AppSettingsNames.Notification.EmailMessageSent, dto.MessageSent.ToString());
            await _settingManager.ChangeSettingForUserAsync(Session.UserId, Session.OrganizationId.Value, AppSettingsNames.Notification.EmailConversationAssigned, dto.ConversationAssigned.ToString());
            await _settingManager.ChangeSettingForUserAsync(Session.UserId, Session.OrganizationId.Value, AppSettingsNames.Notification.EmailContactAssigned, dto.ContactAssigned.ToString());
            await _settingManager.ChangeSettingForUserAsync(Session.UserId, Session.OrganizationId.Value, AppSettingsNames.Notification.EmailConversationFollowed, dto.ConversationFollowed.ToString());
            await _settingManager.ChangeSettingForUserAsync(Session.UserId, Session.OrganizationId.Value, AppSettingsNames.Notification.EmailLibraryItemCreated, dto.LibraryItemCreated.ToString());
            await _settingManager.ChangeSettingForUserAsync(Session.UserId, Session.OrganizationId.Value, AppSettingsNames.Notification.EmailConversationflagged, dto.ConversationFlagged.ToString());
            return Ok();
        }
    }
}
