using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AskPam.Crm.Runtime.Session;
using AutoMapper;
using AskPam.Crm.Configuration.Dtos;
using AskPam.Crm.Organizations;
using Microsoft.Extensions.Options;
using AskPam.Crm.Settings;
using AskPam.Crm.Conversations;
using AskPam.Crm.Controllers.Configuration.Dtos;
using AskPam.Crm.AI;
using AskPam.Crm.AI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using AskPam.Application.Dto;

namespace AskPam.Crm.Configuration
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class OrganizationSettingsController : BaseController
    {
        private readonly IOrganizationManager _organizationManager;
        private readonly ISettingManager _settingsManager;
        private readonly IQnAMakerManager _botManager;
        private readonly ISmoochConversationService _smoochService;

        public OrganizationSettingsController(
            IOrganizationManager organizationManager,
            IQnAMakerManager botManager,
            ISettingManager settingsManager,
            ICrmSession session,
            IMapper mapper,
            ISmoochConversationService smoochService
        ) : base(session, mapper)
        {
            _organizationManager = organizationManager;
            _settingsManager = settingsManager;
            _botManager = botManager;
            _smoochService = smoochService;
        }




        [HttpGet]
        [ProducesResponseType(typeof(WidgetSettingsDto), 200)]
        public async Task<IActionResult> GetWidgetSettings()
        {
            EnsureOrganization();

            var organization = await _organizationManager.FindByIdAsync(Session.OrganizationId.Value);
            var smoochAppToken =
                await _settingsManager.GetSettingValueForOrganizationAsync(AppSettingsNames.Smooch.AppToken,
                    organization.Id);

            var settings = new WidgetSettingsDto { SmoochAppToken = smoochAppToken };

            return new ObjectResult(settings);
        }

        [HttpGet]
        [ProducesResponseType(typeof(EmailOrganizationSettingsDto), 200)]
        public async Task<IActionResult> GetEmailSettings()
        {
            EnsureOrganization();

            var orgnization = await _organizationManager.FindByIdAsync(Session.OrganizationId.Value);
            var inbound = await _settingsManager.GetSettingValueForOrganizationAsync(AppSettingsNames.Email.PostmarkInbound, Session.OrganizationId.Value);
            var organizationEmail = await _settingsManager.GetSettingValueForOrganizationAsync(AppSettingsNames.Email.SenderEmail, Session.OrganizationId.Value);
            var organizationEmailName = await _settingsManager.GetSettingValueForOrganizationAsync(AppSettingsNames.Email.SenderEmailName, Session.OrganizationId.Value);

            var postmarkSenderId = await _settingsManager.GetSettingValueForOrganizationAsync(AppSettingsNames.Email.PostmarkSenderId, Session.OrganizationId.Value);
            var isDkimVerified = await _settingsManager.GetSettingValueForOrganizationAsync(AppSettingsNames.Email.PostmarkIsDkimVerified, Session.OrganizationId.Value);

            var settings = new EmailOrganizationSettingsDto
            {
                SenderSignatureId = postmarkSenderId,
                Email = organizationEmail,
                Name = organizationEmailName,
                IsDkimVerified = bool.Parse(isDkimVerified),
                ForwardEmail = $"{orgnization.Id.ToString().ToLower()}@{inbound}",

            };
            bool.TryParse(await _settingsManager.GetSettingValueForOrganizationAsync(AppSettingsNames.Conversation.ShowPoweredByAskPam, Session.OrganizationId.Value), out bool dd);
            settings.ShowPoweredByAskPam = dd;
            return new ObjectResult(settings);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEmailSettings([FromBody]EmailOrganizationSettingsDto settings)
        {
            EnsureOrganization();
            await _settingsManager.ChangeSettingForOrganizationAsync(AppSettingsNames.Conversation.ShowPoweredByAskPam, settings.ShowPoweredByAskPam.ToString(), Session.OrganizationId.Value);
            return Ok();
        }




        [HttpPost]
        public async Task<IActionResult> DeleteIntegration(string id)
        {
            EnsureOrganization();

            var orgnization = await _organizationManager.FindByIdAsync(Session.OrganizationId.Value);

            await _smoochService.DeleteIntegration(orgnization, id);

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(typeof(EmailOrganizationSettingsDto), 200)]
        public async Task<IActionResult> CreateEmail(string email)
        {
            EnsureOrganization();

            var organization = await _organizationManager.FindByIdAsync(Session.OrganizationId.Value);
            organization = await _organizationManager.AddEmailChannel(organization, email, organization.Name);
            var inbound = await _settingsManager.GetSettingValueForOrganizationAsync(AppSettingsNames.Email.PostmarkInbound, Session.OrganizationId.Value);
            var organizationEmail = await _settingsManager.GetSettingValueForOrganizationAsync(AppSettingsNames.Email.SenderEmail, Session.OrganizationId.Value);
            var organizationEmailName = await _settingsManager.GetSettingValueForOrganizationAsync(AppSettingsNames.Email.SenderEmailName, Session.OrganizationId.Value);
            var postmarkSenderId = await _settingsManager.GetSettingValueForOrganizationAsync(AppSettingsNames.Email.PostmarkSenderId, Session.OrganizationId.Value);
            var isDkimVerified = await _settingsManager.GetSettingValueForOrganizationAsync(AppSettingsNames.Email.PostmarkIsDkimVerified, Session.OrganizationId.Value);

            var settings = new EmailOrganizationSettingsDto
            {
                SenderSignatureId = postmarkSenderId,
                Email = organizationEmail,
                Name = organizationEmailName,
                IsDkimVerified = bool.Parse(isDkimVerified),
                ForwardEmail = $"{organization.Id.ToString().ToLower()}@{inbound}"

            };

            return new ObjectResult(settings);
        }

        #region Bot
        [HttpGet]
        [ProducesResponseType(typeof(BotSettingsDto), 200)]
        public async Task<IActionResult> GetBotSettings()
        {
            EnsureOrganization();

            var result = new BotSettingsDto()
            {
                BotName = await _settingsManager.GetSettingValueForOrganizationAsync(AppSettingsNames.AI.QnABotName, Session.OrganizationId.Value),
                BotAvatar = await _settingsManager.GetSettingValueForOrganizationAsync(AppSettingsNames.AI.QnABotAvatar, Session.OrganizationId.Value),
                BotEnabled = Boolean.Parse(await _settingsManager.GetSettingValueForOrganizationAsync(AppSettingsNames.AI.QnABotEnabled, Session.OrganizationId.Value)),
                DesactivationEnabled = Boolean.Parse(await _settingsManager.GetSettingValueForOrganizationAsync(AppSettingsNames.AI.QnABotDeactivationEnabled, Session.OrganizationId.Value)),
                Intro = await _settingsManager.GetSettingValueForOrganizationAsync(AppSettingsNames.AI.QnABotIntro, Session.OrganizationId.Value),
                Outro = await _settingsManager.GetSettingValueForOrganizationAsync(AppSettingsNames.AI.QnABotOutro, Session.OrganizationId.Value),
                Treshold = double.Parse(await _settingsManager.GetSettingValueForOrganizationAsync(AppSettingsNames.AI.QnABotThreshold, Session.OrganizationId.Value)),
            };

            return new ObjectResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBotSettings([FromBody] BotSettingsDto dto)
        {
            EnsureOrganization();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _settingsManager.ChangeSettingForOrganizationAsync(AppSettingsNames.AI.QnABotName, dto.BotName, Session.OrganizationId.Value);
            await _settingsManager.ChangeSettingForOrganizationAsync(AppSettingsNames.AI.QnABotAvatar, dto.BotAvatar, Session.OrganizationId.Value);
            await _settingsManager.ChangeSettingForOrganizationAsync(AppSettingsNames.AI.QnABotEnabled, dto.BotEnabled.ToString(), Session.OrganizationId.Value);
            await _settingsManager.ChangeSettingForOrganizationAsync(AppSettingsNames.AI.QnABotDeactivationEnabled, dto.DesactivationEnabled.ToString(), Session.OrganizationId.Value);
            await _settingsManager.ChangeSettingForOrganizationAsync(AppSettingsNames.AI.QnABotIntro, dto.Intro, Session.OrganizationId.Value);
            await _settingsManager.ChangeSettingForOrganizationAsync(AppSettingsNames.AI.QnABotOutro, dto.Outro, Session.OrganizationId.Value);
            await _settingsManager.ChangeSettingForOrganizationAsync(AppSettingsNames.AI.QnABotThreshold, dto.Treshold.ToString(), Session.OrganizationId.Value);

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<QnAPairDto>), 200)]
        public async Task<IActionResult> SaveQnAs([FromBody] IEnumerable<QnAPairDto> dto)
        {
            EnsureOrganization();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var oldQnas = await _botManager.FindbyIds(dto
                .Where(qnA => qnA.Id != 0)
                .Select(qnA => qnA.Id),
                Session.OrganizationId.Value
            );

            var qnAsToCreate = dto
                .Where(qnA => !(qnA.IsDeleted == true))
                .Where(qnA => qnA.Id == 0)
                .Select(qnA => new QnAPair(qnA.Question, qnA.Answer, Session.OrganizationId.Value));

            var qnAsToUpdate = dto
                .Where(qnA => !(qnA.IsDeleted == true))
                .Join(oldQnas,
                    (qnA) => qnA.Id,
                    (oldQnA) => oldQnA.Id,
                    (qnA, oldQna) => new { OldQnA = oldQna, NewQnA = qnA }
                )
                .Where(dyn => dyn.NewQnA.Question != dyn.OldQnA.Question || dyn.NewQnA.Answer != dyn.OldQnA.Answer)
                .Select(dyn => new OldQnAPair(dyn.OldQnA, dyn.NewQnA.Question, dyn.NewQnA.Answer));

            var qnAsToDelete = dto
                .Where(qnA => qnA.IsDeleted == true)
                .Join(oldQnas,
                    (qnA) => qnA.Id,
                    (oldQnA) => oldQnA.Id,
                    (qnA, oldQna) => new { OldQnA = oldQna, NewQnA = qnA }
                )
                .Select(dyn => dyn.OldQnA);

            var result = await _botManager.SaveKnowledgeBase(qnAsToCreate, qnAsToUpdate, qnAsToDelete, Session.OrganizationId.Value);

            return new ObjectResult(result.Select(qna => Mapper.Map<QnAPair, QnAPairDto>(qna)));
        }

        [HttpPost]
        [ProducesResponseType(typeof(PagedResultDto<QnAPairDto>), 200)]
        public async Task<IActionResult> GetQnas([FromBody]QnAPairRequestDto input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EnsureOrganization();

            var query = _botManager.GetAllQnAPairs(Session.OrganizationId.Value);

            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                query = query.Where(qnA =>
                        (qnA.Question + "|" + qnA.Answer)
                            .ToLower()
                            .Contains(input.Filter.ToLower())
                        );
            }

            var totalCount = await query.CountAsync();
            var hasNext = (input.SkipCount + input.MaxResultCount) < totalCount;

            if (!string.IsNullOrWhiteSpace(input.Sorting))
            {
                query = query.OrderBy(input.Sorting);
            }

            var qnAPairs = await query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToListAsync();

            var result = qnAPairs.Select(t => Mapper.Map<QnAPair, QnAPairDto>(t)).ToList();

            return new ObjectResult(
                new PagedResultDto<QnAPairDto>(
                    totalCount,
                    result,
                    hasNext
                )
            );
        }

        [HttpGet]
        [ProducesResponseType(typeof(QnAMakerResultDto), 200)]
        public async Task<IActionResult> Ask(string question)
        {
            EnsureOrganization();

            var result = await _botManager.Ask(question, Session.OrganizationId.Value);

            return new ObjectResult(Mapper.Map<QnAResult, QnAMakerResultDto>(result));
        }

        #endregion

    }
}
