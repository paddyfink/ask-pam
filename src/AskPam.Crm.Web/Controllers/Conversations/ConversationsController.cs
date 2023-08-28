using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AskPam.Crm.Runtime.Session;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using AskPam.Crm.Conversations.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AskPam.Crm.Authorization;
using AskPam.Crm.Controllers.Conversations.Dtos;
using AskPam.Crm.Controllers.Conversations;
using AskPam.Crm.Common.Dtos;
using AskPam.Crm.Contacts;
using AskPam.Events;
using AskPam.Crm.Followers;
using AskPam.Crm.Organizations;
using AskPam.Application.Dto;
using AskPam.Extensions;
using AskPam.Crm.Conversations.Events;
using AskPam.Domain.Repositories;
using AskPam.Crm.Contacts.Dtos;
using AskPam.Crm.Stars;
using AskPam.Domain.Entities;
using AskPam.EntityFramework.Repositories;
using AutoMapper.QueryableExtensions;

namespace AskPam.Crm.Conversations
{
    [Authorize]
    [Route("api/[controller]")]
    public class ConversationsController : BaseController
    {
        private readonly IRepository<Contact> _contactRepository;
        private readonly IConversationsManager _conversationManager;
        private readonly IOrganizationManager _organizationManager;
        private readonly IUserManager _userManager;
        private readonly IDomainEvents _domainEvents;
        private readonly IFollowersManager _followersManager;
        private readonly IStarsManager _starsManager;
        private readonly IRepository<Conversation> _conversationsRepository;
        private readonly IRepository<Message> _messageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ConversationsController(
            ICrmSession session,
            IMapper mapper,
            IConversationsManager conversationManager,
            IRepository<Contact> contactRepository,
            IFollowersManager followersManager,
            IUserManager userManager,
            IDomainEvents domainEvents,
            IOrganizationManager organizationManager,
            IRepository<Conversation> conversationsRepository,
            IStarsManager starsManager,
            IRepository<Message> messageRepository, IUnitOfWork unitOfWork)
            : base(session, mapper)
        {
            _conversationManager = conversationManager;
            _userManager = userManager;
            _contactRepository = contactRepository;
            _domainEvents = domainEvents;
            _organizationManager = organizationManager;
            _followersManager = followersManager;
            _starsManager = starsManager;
            _conversationsRepository = conversationsRepository;
            _messageRepository = messageRepository;
            _unitOfWork = unitOfWork;
        }

        #region Conversation

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(PagedResultDto<ConversationListDto>), 200)]
        public async Task<IActionResult> GetConversations([FromBody]ConversationListRequestDto input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var query = _conversationsRepository.GetAll()
                .Include(c => c.TagsRelations)
                .ThenInclude(t => t.Tag)
                .Where(c => c.OrganizationId == Session.OrganizationId.Value);

            if (input.ContactId.HasValue)
            {
                query = query.Where(c => c.ContactId == input.ContactId);
            }


            switch (input.Filter)
            {
                case ConversationFilter.All:
                    break;
                case ConversationFilter.AssignedToMe:
                    query = query.Where(c => c.AssignedToId == Session.UserId);
                    break;
                case ConversationFilter.Unassigned:
                    query = query.Where(c => c.AssignedToId == null || c.AssignedToId.Equals(""));
                    break;
                case ConversationFilter.Flagged:
                    query = query.Where(l => l.IsFlagged);
                    break;
                case ConversationFilter.Leads:
                    query = query.Where(c => c.ContactId == null);
                    break;
                case ConversationFilter.FollowedByMe:
                    query = query
                        .Where(c => c.Followers.Any(f => f.UserId == Session.UserId));
                    break;
                case ConversationFilter.Contacts:
                    query = query.Where(c => c.ContactId != null);
                    break;
                case ConversationFilter.Favorites:
                    query = query.Where(l => l.StarRelations.Any(s => s.UserId == Session.UserId));
                    break;
                case ConversationFilter.Unread:
                    query = query.Where(c => !c.Seen);
                    break;

                case ConversationFilter.Archived:
                    break;
            }


            query = query.Where(l => input.Filter == ConversationFilter.Archived ? !l.IsActive : l.IsActive);


            if (!input.Search.IsNullOrEmpty())
            {
                query = query.Where(c =>
                    EF.Functions.Like(c.Name, $"%{input.Search}%") ||
                    EF.Functions.Like(c.Contact.FullName, $"%{input.Search}%")
                );
            }

            var result = query
                .OrderByDescending(r => r.Messages
                    .Where(m => !m.IsDeleted)
                    .OrderByDescending(m => m.Date)
                    .FirstOrDefault()
                    .Date)
                .ProjectTo<ConversationListDto>(new { userId = Session.UserId });

            var totalCount = await result.CountAsync();
            var hasNext = (input.SkipCount + input.MaxResultCount) < totalCount;

            var conversations = await result
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
                    .ToListAsync();

            return new ObjectResult(
               new PagedResultDto<ConversationListDto>(
                   totalCount,
                   conversations,
                   hasNext
               )
           );
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ConversationDto), 200)]
        public async Task<IActionResult> GetConversation(int id)
        {
            EnsureOrganization();

            var currentUser = await _userManager.FindByIdAsync(Session.UserId);

            var conv = await _conversationsRepository.GetAll()
                .Where(c => c.OrganizationId == Session.OrganizationId)
                .Include(c => c.Channels)
                .Include(c => c.AssignedTo)
                .Include(c => c.Contact).ThenInclude(ct => ct.TagsRelations).ThenInclude(ct => ct.Tag)
                .Include(c => c.TagsRelations).ThenInclude(r => r.Tag)
                .FirstOrDefaultAsync(c => c.Id == id);


            var conversationDto = Mapper.Map<Conversation, ConversationDto>(conv);

            var messages = await _messageRepository.GetAll()
                .Where(m => m.Conversation.OrganizationId == Session.OrganizationId)
                .Where(m => m.ConversationId == id)
                .Include(m => m.TagsRelations).ThenInclude(t => t.Tag)
                .Include(m => m.CreatedUser)
                .Include(m => m.Attachments)
                .Include(m => m.DeliveryStatus)
                .ToListAsync();

            conversationDto.Messages = Mapper.Map<List<Message>, List<MessageDto>>(messages);

            conv.MarkAsSeen();
            await _conversationsRepository.UpdateAsync(conv);
            await _unitOfWork.SaveChangesAsync();

            await _domainEvents.RaiseAsync(new ConversationReadEvent() { Conversation = conv, User = currentUser });

            return new ObjectResult(conversationDto);
        }


        [HttpPost("{id}/linkContact")]
        [ProducesResponseType(typeof(ContactDto), 200)]
        public async Task<IActionResult> LinkContactToConversation(int id, [FromBody]int contactId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EnsureOrganization();

            var conversation = await _conversationManager.FindByIdAsync(id, Session.OrganizationId.Value);
            var contact = await _contactRepository.FirstOrDefaultAsync(c => c.Id == contactId && c.OrganizationId == Session.OrganizationId.Value);

            await _conversationManager.LinkContact(conversation, contact);

            return Ok(Mapper.Map<Contact, ContactDto>(contact));
        }

        [HttpPost("{id}/unlinkContact")]
        [ProducesResponseType(typeof(ContactDto), 200)]
        public async Task<IActionResult> UnlinkContactToConversation(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EnsureOrganization();

            var conversation = await _conversationManager.FindByIdAsync(id, Session.OrganizationId.Value);

            await _conversationManager.UnlinkContact(conversation);

            return Ok();
        }


        [HttpPost("{id}/assign")]
        public async Task<IActionResult> AssignToUser(int id, [FromBody]string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EnsureOrganization();

            var conversation = await _conversationManager.FindByIdAsync(id, Session.OrganizationId.Value);

            if (!string.IsNullOrEmpty(userId))
            {
                var assignee = await _userManager.FindByIdAsync(userId);
                var assigner = await _userManager.FindByIdAsync(Session.UserId);

                await _conversationManager.AssigntoUser(conversation, assignee, assigner);
            }
            else
            {
                await _conversationManager.RemoveUserAssignment(conversation);
            }

            return Ok();
        }

        [HttpPost("{id}/flag")]
        public async Task<IActionResult> Flag(int id)
        {
            EnsureOrganization();

            //var conversation = await _conversationManager.FindByIdAsync(id, Session.OrganizationId.Value);
            //var currentUser = await _userManager.FindByIdAsync(Session.UserId);
            //await _conversationManager.Flag(conversation, currentUser);

            return Ok();
        }

        [HttpPost("{id}/Star")]
        public async Task<IActionResult> Star(int id, [FromBody]bool isStarred)
        {
            EnsureOrganization();

            var conversation = await _conversationManager.FindByIdAsync(id, Session.OrganizationId.Value);
            var currentUser = await _userManager.FindByIdAsync(Session.UserId);
            if (isStarred)
                await _starsManager.UnStar(currentUser, conversation);
            else
                await _starsManager.Star(currentUser, conversation);

            return Ok();
        }

        [HttpPost("{id}/MarkAsUnread")]
        public async Task<IActionResult> MarkAsUnread(long id)
        {
            EnsureOrganization();

            await _conversationManager.MarkAsUnRead(id);

            return Ok();
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> ToogleBotActivation(long converationId)
        {
            EnsureOrganization();

            var conversation = await _conversationManager.FindByIdAsync(converationId, Session.OrganizationId.Value);
            var currentUser = await _userManager.FindByIdAsync(Session.UserId);
            await _conversationManager.ActivateBot(conversation);

            return Ok();
        }

        [HttpPost("{id}/archive")]
        public async Task<IActionResult> Archive(int id)
        {
            EnsureOrganization();


            var currentUser = await _userManager.FindByIdAsync(Session.UserId);
            var conversation = await _conversationManager.FindByIdAsync(id, Session.OrganizationId.Value);
            await _conversationManager.Archive(conversation, currentUser);

            return Ok();
        }

        [HttpPost("message")]
        [ProducesResponseType(typeof(MessageDto), 200)]
        public async Task<IActionResult> SendMessage([FromBody]SendMessageDto dto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EnsureOrganization();

            var user = await _userManager.FindByIdAsync(Session.UserId);


            Message message = new Message()
            {
                Author = user.FullName,
                AuthorId = user.Id,
                Avatar = user.Picture,
                Text = dto.Text,
                Date = DateTime.UtcNow,
                Status = MessageStatus.Sent,
                Subject = dto.Subject,
                MessageType = Enumeration<MessageType, string>.FromValue(dto.Type),

            };

            if (dto.Attachments != null && dto.Attachments.Any())
            {
                message.Attachments = new List<Attachment>();
                foreach (var attachment in dto.Attachments)
                {
                    message.AddAttachment(attachment.Name, attachment.Content, attachment.ContentType, attachment.ContentLength);
                }
            }

            Message msg;
            Conversation conversation;

            if (dto.ConversationId.HasValue)
            {
                conversation = await _conversationsRepository.GetAsync(dto.ConversationId.Value);
            }
            else
            {
                var recipient = dto.Recipients.First();
                conversation = await _conversationsRepository.GetAll()
                    .Where(c => EF.Functions.Like(c.Email, recipient.Recipient)).FirstOrDefaultAsync();

                if (conversation == null)
                {
                    conversation = new Conversation(Session.OrganizationId.Value, recipient.Name, color: await _conversationManager.GetRandomConversationColor(), contactId: recipient.ContactId, email: recipient.Recipient.ToLower());

                    conversation.AddChannel(new Channel
                    {
                        DisplayName = recipient.Recipient.ToLower(),
                        Active = true,
                        Type = Enumeration<ChannelType, string>.FromValue(recipient.ChannelType)
                    });
                    await _conversationsRepository.InsertAsync(conversation);
                    await _unitOfWork.SaveChangesAsync();
                }
            }
            if (message.MessageType == MessageType.Note)
            {
                msg = await _conversationManager.AddNote(conversation, user, message);
            }
            else
            {
                if (conversation.Email.IsNullOrEmpty())
                {
                    var email = new Email()
                    {
                        Author = user.FullName,
                        AuthorId = user.Id,
                        Avatar = user.Picture,
                        Text = dto.Text,
                        Date = DateTime.UtcNow,
                        Status = MessageStatus.Sent,
                        Subject = dto.Subject,
                        MessageType = Enumeration<MessageType, string>.FromValue(dto.Type),
                        Cc = dto.Cc,
                        Bcc = dto.Bcc

                    };
                    msg = await _conversationManager.SendMessage(conversation, email, user);
                }
                else
                {

                    msg = await _conversationManager.SendMessage(conversation, message, user);
                }
            }
            return new ObjectResult(Mapper.Map<Message, MessageDto>(msg));
        }


        [HttpGet("email/{messageId}")]
        [ProducesResponseType(typeof(EmailDto), 200)]
        public async Task<IActionResult> GetEmail(long messageId)
        {
            EnsureOrganization();

            var result = await _messageRepository.GetAll()
                .Include(m => m.Attachments)
                .Include(e => e.Conversation)
                .Include(m => m.CreatedUser)
                //.Select(m => Mapper.Map<Message, EmailDto>(m))
                .FirstAsync(m => m.Conversation.OrganizationId == Session.OrganizationId.Value && m.Id == messageId); ;

            return new ObjectResult(Mapper.Map<Message, EmailDto>(result));
        }

        #endregion


        #region Filters
        [HttpGet("filters")]
        [ProducesResponseType(typeof(IEnumerable<EnumValueDto>), 200)]
        public async Task<IActionResult> GetFilters()
        {
            var roles = await _userManager.GetRolesAsync(Session.UserId, Session.OrganizationId);
            List<EnumValueDto> filters = new List<EnumValueDto>();
            bool admin = roles.Contains(RolesName.Admin);

            filters.Add(new EnumValueDto { Id = ConversationFilter.All, Name = "All", Default = admin ? true : false });
            filters.Add(new EnumValueDto { Id = ConversationFilter.Unread, Name = "Unread" });
            filters.Add(new EnumValueDto { Id = ConversationFilter.AssignedToMe, Name = "Assigned To Me", Default = !admin ? true : false });
            filters.Add(new EnumValueDto { Id = ConversationFilter.Flagged, Name = "Flagged" });
            filters.Add(new EnumValueDto { Id = ConversationFilter.FollowedByMe, Name = "Following" });
            filters.Add(new EnumValueDto { Id = ConversationFilter.Contacts, Name = "Contacts" });
            filters.Add(new EnumValueDto { Id = ConversationFilter.Leads, Name = "Leads" });
            filters.Add(new EnumValueDto { Id = ConversationFilter.Unassigned, Name = "Unassigned" });
            filters.Add(new EnumValueDto { Id = ConversationFilter.Archived, Name = "Archived" });
            filters.Add(new EnumValueDto { Id = ConversationFilter.Favorites, Name = "Favorites" });

            return new ObjectResult(filters);
        }
        #endregion
    }
}
