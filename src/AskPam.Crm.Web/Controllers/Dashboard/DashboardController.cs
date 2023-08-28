using System.Linq;
using System.Threading.Tasks;
using AskPam.Crm.Runtime.Session;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using AskPam.Crm.Conversations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AskPam.Domain.Repositories;
using System;

namespace AskPam.Crm.Controllers.Dashboard
{
    [Authorize]
    [Route("api/[controller]")]
    public class DashboardController : BaseController
    {
        private readonly IRepository<Conversation> _conversationsRepository;
        private readonly IRepository<Message> _messageRepository;

        public DashboardController(
            IRepository<Message> messageRepository,
            IRepository<Conversation> conversationsRepository,
            ICrmSession session,
            IMapper mapper) : base(session, mapper)
        {
            _conversationsRepository = conversationsRepository;
            _messageRepository = messageRepository;
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> GetConversationsAssignedCount()
        {
            EnsureOrganization();

            var result = await GetAllConversations(Session.OrganizationId.Value)
                .CountAsync(c => c.AssignedToId == Session.UserId);

            return new ObjectResult(result);
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> GetConversationsUnreadCount()
        {
            EnsureOrganization();

            var result = await GetAllConversations(Session.OrganizationId.Value)
                .CountAsync(c => !c.Seen);

            return new ObjectResult(result);
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> GetConversationsFlaggedCount()
        {
            EnsureOrganization();

            var result = await GetAllConversations(Session.OrganizationId.Value)
                .CountAsync(c => c.IsFlagged);

            return new ObjectResult(result);
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> GetConversationsFollowedCount()
        {
            EnsureOrganization();

            var result = await GetAllConversations(Session.OrganizationId.Value)
                .CountAsync(c => c.Followers.Any(f => f.UserId == Session.UserId));

            return new ObjectResult(result);
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> GetConversationsCount()
        {
            EnsureOrganization();

            var result = await GetAllConversations(Session.OrganizationId.Value)
                .CountAsync();

            return new ObjectResult(result);
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> GetMessagesReceivedCount()
        {
            EnsureOrganization();

            var result = await GetAllMessages(Session.OrganizationId.Value)
                .CountAsync(m => m.MessageStatusId == MessageStatus.Received.Value);

            return new ObjectResult(result);
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> GetMessagesSentCount()
        {
            EnsureOrganization();

            var result = await GetAllMessages(Session.OrganizationId.Value)
                .CountAsync(m => m.MessageStatusId == MessageStatus.Sent.Value);

            return new ObjectResult(result);
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> GetMessagesUnreadCount()
        {
            EnsureOrganization();

            var result = await GetAllMessages(Session.OrganizationId.Value)
                .Where(m => m.Seen == false)
                .CountAsync();

            return new ObjectResult(result);
        }

        #region private
        private IQueryable<Conversation> GetAllConversations(Guid organizationId)
        {
            return _conversationsRepository.GetAll()
                .Where(c => c.OrganizationId == organizationId);
        }

        private IQueryable<Message> GetAllMessages(Guid OrganizationId)
        {
            return _messageRepository.GetAll()
                .Where(m => m.Conversation.OrganizationId == OrganizationId);
        }
        #endregion
    }
}
