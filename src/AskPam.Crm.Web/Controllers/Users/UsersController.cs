using AskPam.Crm.Authorization;
using AskPam.Crm.Runtime.Session;
using AskPam.Application.Dto;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq;
using System.Threading.Tasks;
using AskPam.Crm.Users.Dtos;
using System.Collections.Generic;

namespace AskPam.Crm.Users
{
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : BaseController
    {
        private readonly IUserManager _userManager;

        public UsersController(
            ICrmSession session,
            IMapper mapper,
            IUserManager userManager
        ) : base(session, mapper)
        {
            _userManager = userManager;
        }

    
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(IEnumerable<UserRoleDto>), 200)]
        public async Task<IActionResult> GetRoles()
        {
            EnsureOrganization();

            return new ObjectResult(
                await _userManager.GetAllRoles(Session.OrganizationId.Value)
                    .OrderBy(r => r.DisplayName)
                    .Select(r => Mapper.Map<Role, UserRoleDto>(r))
                    .ToListAsync()
            );
        }

 
        [HttpPost("{id}/assignToRole")]
        public async Task<IActionResult> AssignToRole(string id, [FromBody]string roleName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EnsureOrganization();

            var user = await _userManager.FindByIdAsync(id);
            await _userManager.AddToRole(user, roleName, Session.OrganizationId.Value);

            return Ok();
        }

        [HttpPost("invite")]
        public async Task<IActionResult> Invite([FromBody]UserInvitationDto invitation)
        {
            EnsureOrganization();

            await _userManager.CreateOrAddtoOrganization(
                invitation.FirstName,
                invitation.LastName,
                invitation.Email,
                invitation.RoleName,
                Session.OrganizationId.Value
            );

            return Ok();
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(PagedResultDto<UserDto>), 200)]
        public async Task<IActionResult> GetUsers(UserListRequestDto input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EnsureOrganization();

            var query = from user in _userManager.GetAllUsers(Session.OrganizationId.Value)
                join userRole in _userManager.GetAllUserRoles(Session.OrganizationId.Value)
                    on user.Id equals userRole.UserId
                join role in _userManager.GetAllRoles(Session.OrganizationId.Value)
                    on userRole.RoleId equals role.Id
                select new UserDto
                {
                    Id = user.Id,
                    Role = role.Name,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    FullName = user.FullName,
                    Picture = user.Picture
                };




            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                query = query.Where(u => EF.Functions.Like(u.FullName, $"%{input.Filter}%") || EF.Functions.Like(u.Email, $"%{input.Filter}%"));
            }

            query = !string.IsNullOrWhiteSpace(input.Sorting) ? query.OrderBy(input.Sorting) : query.OrderBy(u => u.FullName);
            

            var totalCount = await query.CountAsync();
            var hasNext = (input.SkipCount + input.MaxResultCount) < totalCount;

            return new ObjectResult(
                new PagedResultDto<UserDto>(
                    totalCount,
                    await query.ToListAsync(),
                    hasNext
                )
            );
        }
    }
}
