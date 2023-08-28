using AskPam.Crm.Authorization;
using AskPam.Crm.Organizations.Dtos;
using AskPam.Crm.Runtime.Session;
using AskPam.Domain.Repositories;
using AskPam.Application.Dto;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AskPam.Crm.Configuration;
using AskPam.Crm.Users.Dtos;
using AskPam.Crm.Integrations;
using AskPam.Crm.Integrations.Dtos;
using AskPam.Crm.Conversations;

namespace AskPam.Crm.Organizations
{
    [Authorize]
    [Route("api/[controller]")]
    public class OrganizationController : BaseController
    {
        private readonly IRepository<Organization, Guid> _organizationRepository;
        private readonly IOrganizationManager _organizationManager;
        private readonly IUserManager _userManager;
        private readonly ISettingManager _settingsManager;

        public OrganizationController(
            ICrmSession session,
            IMapper mapper,
            IRepository<Organization, Guid> organizations,
            IOrganizationManager organizationManager,
            IUserManager userManager,
            ISettingManager settingManager
        ) : base(session, mapper)
        {
            var sessions = Session;
            _organizationRepository = organizations;
            _organizationManager = organizationManager;
            _userManager = userManager;
            _settingsManager = settingManager;
        }

        [HttpGet("{id}/features")]
        [ProducesResponseType(typeof(FeaturesDto), 200)]
        public async Task<IActionResult> GetFeatures(Guid id)
        {
            EnsureOrganization();
            FeaturesDto settings = new FeaturesDto();

            var organization = await _organizationManager.FindByIdAsync(Session.OrganizationId.Value);
            if (organization.Stay22)
            {
                var eventName = await _settingsManager.GetSettingValueForOrganizationAsync(AppSettingsNames.Stay22.Stay22Event, organization.Id);
                settings = new FeaturesDto { Stay22 = true, Stay22Event = eventName };
            }

            return new ObjectResult(settings);
        }


        [HttpPost("[action]")]
        [ProducesResponseType(typeof(PagedResultDto<OrganizationDto>), 200)]
        public async Task<IActionResult> GetOrganizations([FromBody]GetOrganizationsRequestDto input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var query = _organizationRepository.GetAll();

            if (!string.IsNullOrEmpty(input.Filter))
                query = query.Where(o => o.Name.Contains(input.Filter));

            var totalCount = await query.CountAsync();
            var hasNext = (input.SkipCount + input.MaxResultCount) < totalCount;

            var orgs = await query
               .Skip(input.SkipCount)
               .Take(input.MaxResultCount)
               .ProjectTo<OrganizationDto>()
               .ToListAsync();

            return new ObjectResult(
                new PagedResultDto<OrganizationDto>(
                    totalCount,
                    orgs,
                    hasNext
                )
            );
        }

        [HttpPost()]
        [ProducesResponseType(typeof(OrganizationDto), 200)]
        public async Task<IActionResult> CreateOrganization([FromBody]CreateOrganizationDto input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var organization = new Organization(input.Name);
            organization = await _organizationManager.CreateOrganization(organization);
            var dto = Mapper.Map<Organization, OrganizationDto>(organization);
            return new ObjectResult(dto);
        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrganizationDto), 200)]
        public async Task<IActionResult> GetOrganizationDetail(Guid id)
        {

            var organization = await _organizationRepository.GetAll()
                .FirstOrDefaultAsync(o => o.Id == id);

            return new ObjectResult(Mapper.Map<Organization, OrganizationDto>(organization));
        }

        [HttpGet("{id}/users")]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), 200)]
        public async Task<IActionResult> GetUsers(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EnsureOrganization();

            var users = await (from user in _userManager.GetAllUsers(id)
                               join userRole in _userManager.GetAllUserRoles(id)
                                   on user.Id equals userRole.UserId
                               join role in _userManager.GetAllRoles(id)
                                   on userRole.RoleId equals role.Id
                               select new UserDto
                               {
                                   Id = user.Id,
                                   Role = role.Name,
                                   Email = user.Email,
                                   FirstName = user.FirstName,
                                   LastName = user.LastName,
                                   Picture = user.Picture
                               }
                        )
                        .ToListAsync();


            return new ObjectResult(users);
        }


        [HttpPost("{id}/integrations")]
        [ProducesResponseType(typeof(IntegrationDto), 200)]
        public async Task<IActionResult> CreateIntegration(Guid id, [FromBody]IntegrationDto input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }

        [HttpPost("{id}/users")]
        [ProducesResponseType(typeof(UserDto), 200)]
        public async Task<IActionResult> AddUser(Guid id, [FromBody]UserInvitationDto input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = new User(input.FirstName, input.LastName, input.Email);
            user = await _userManager.CreateOrAddtoOrganization(input.FirstName, input.LastName, input.Email, input.RoleName, id);


            return new ObjectResult(Mapper.Map<User, UserDto>(user));
        }

        [HttpDelete("{id}/users")]
        public async Task<IActionResult> RemoveUser(Guid id, [FromBody]string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _userManager.RemoveUSer(id, userId);


            return new OkResult();
        }
    }
}
