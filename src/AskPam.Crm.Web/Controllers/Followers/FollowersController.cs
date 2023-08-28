using AskPam.Crm.Authorization;
using AskPam.Crm.Controllers.Followers.Dtos;
using AskPam.Crm.Conversations;
using AskPam.Crm.Followers;
using AskPam.Crm.Runtime.Session;
using AskPam.Crm.Users.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.Controllers.Followers
{
    [Authorize]
    [Route("api/[controller]")]
    public class FollowersController : BaseController
  {
    private IUserManager _userManager;
    private IFollowersManager _followersManager;
    private IConversationsManager _conversationManager;

    public FollowersController(
        ICrmSession session,
        IMapper mapper,
        IFollowersManager followersManager,
        IUserManager userManager,
        IConversationsManager conversationManager)
        : base(session, mapper)
    {
      _userManager = userManager;
      _followersManager = followersManager;
      _conversationManager = conversationManager;
    }


    [HttpPost("[action]")]
    public async Task<IActionResult> Follow([FromBody]FollowerRelationDto followerRelationDto)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      EnsureOrganization();

      var user = await _userManager.FindByIdAsync(followerRelationDto.UserId);
      var conversation = await _conversationManager.FindByIdAsync(followerRelationDto.ConversationId, Session.OrganizationId.Value);

      await _followersManager.Follow(user, conversation);

      return Ok();
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Unfollow([FromBody]FollowerRelationDto followerRelationDto)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      EnsureOrganization();

      var user = await _userManager.FindByIdAsync(followerRelationDto.UserId);
      await _followersManager.Unfollow(user, followerRelationDto.ConversationId);

      return Ok();
    }

    [HttpGet("[action]")]
    [ProducesResponseType(typeof(IEnumerable<UserDto>), 200)]
    public async Task<IActionResult> GetFollowers(int conversationId)
    {
      EnsureOrganization();

      var result = await _followersManager.GetFollowers(conversationId, Session.OrganizationId.Value);
      return new ObjectResult(result.Select(m => Mapper.Map<User, UserDto>(m)));
    }

  }
}
