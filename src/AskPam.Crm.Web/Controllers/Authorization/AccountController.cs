using AskPam.Crm.Authorization.Dtos;
using AskPam.Crm.Controllers.Authorization.Dtos;
using AskPam.Exceptions;
using AskPam.Crm.Organizations;
using AskPam.Crm.Runtime.Session;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using AskPam.Crm.Organizations.Dtos;
using AskPam.Crm.Configuration;

namespace AskPam.Crm.Authorization
{
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly IUserManager _userManager;

        public AccountController(
            ICrmSession session,
            IMapper mapper,
            IUserManager userManager,
            ISettingManager settingsManager
        ) : base(session, mapper)
        {
            _userManager = userManager;
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(AuthInfoDto), 200)]
        [ProducesResponseType(typeof(AuthInfoDto), 400)]
        [ProducesResponseType(typeof(AuthInfoDto), 401)]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]LoginDto loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = await _userManager.AuthenticateUser(loginModel.Email, loginModel.Password);
            var decodeToken = Jose.JWT.Payload<TokenObj>(token.IdToken);

            var organization = await _userManager.GetDefaultOrganizationAsync(decodeToken.sub);

            if (organization == null)
                throw new UnauthorizedAccessException("You are not authorized");

            var authInfo = new AuthInfoDto()
            {
                OrganizationId = organization?.Id,
                IdToken = token.IdToken
            };
            return Ok(authInfo);

        }

        [HttpGet("account/info")]
        [ProducesResponseType(typeof(AccountInfo), 200)]
        public async Task<IActionResult> GetInfo()
        {
            var user = await _userManager.FindByIdAsync(Session.UserId);

            var roles = await _userManager.GetRolesAsync(Session.UserId, Session.OrganizationId);

            var organizations = (await _userManager.GetOrganizationsAsync(Session.UserId))
                  .Select(o => Mapper.Map<Organization, OrganizationDto>(o));

            // var settigs = _se

            return new ObjectResult(new AccountInfo
            {
                Profile = new ProfileDto { Id = user.Id, FirstName = user.FirstName, LastName = user.LastName, Email = user.Email, Picture = user.Picture },
                Roles = roles,
                Organizations = organizations
            });
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword([FromBody]string email)
        {
            await _userManager.ForgotPassword(email);
            return Ok();
        }


        [HttpGet("[action]")]
        [ProducesResponseType(typeof(ProfileDto), 200)]
        public async Task<IActionResult> GetProfile()
        {
            EnsureOrganization();

            var result = await _userManager.FindByIdAsync(Session.UserId);

            return new ObjectResult(Mapper.Map<User, ProfileDto>(result));
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        public async Task<IActionResult> SetDefaultOrgnization(Guid organizationId)
        {
            EnsureOrganization();

            await _userManager.SetDefaultOrganizationAsync(Session.UserId, organizationId);

            return Ok();
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(ProfilePictureDto), 200)]
        public async Task<IActionResult> GetProfilePicture()
        {
            EnsureOrganization();

            var result = await _userManager.FindByIdAsync(Session.UserId);

            return new ObjectResult(Mapper.Map<User, ProfilePictureDto>(result));
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(ProfileDto), 200)]
        public async Task<IActionResult> UpdateProfile([FromBody]ProfileDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EnsureOrganization();

            var user = await _userManager.FindByIdAsync(dto.Id);

            user = await _userManager.UpdateProfile(user, dto.FirstName, dto.LastName);

            return new ObjectResult(Mapper.Map<User, ProfileDto>(user));
        }


        [HttpGet("[action]")]
        [ProducesResponseType(typeof(EmailSettingsDto), 200)]
        public async Task<IActionResult> GetEmailSettings()
        {
            EnsureOrganization();

            var result = await _userManager.FindByIdAsync(Session.UserId);

            return new ObjectResult(Mapper.Map<User, EmailSettingsDto>(result));
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(ProfileDto), 200)]
        public async Task<IActionResult> UpdateEmailSettings([FromBody]EmailSettingsDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EnsureOrganization();

            var user = await _userManager.FindByIdAsync(dto.Id);

            user = await _userManager.UpdateEmailSettings(user, dto.Signature);

            return new ObjectResult(Mapper.Map<User, ProfileDto>(user));
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> ChangePassword([FromBody]string password)
        {
            EnsureOrganization();

            var user = await _userManager.FindByIdAsync(Session.UserId);

            await _userManager.ChangePassword(user, password, Session.OrganizationId.Value);

            return new ObjectResult(true);
        }
    }

    public class TokenObj
    {
#pragma warning disable IDE1006 // Naming Styles
        public string iss { get; set; }
        public string sub { get; set; }
        public string aud { get; set; }
        public int exp { get; set; }
        public int iat { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }
}
