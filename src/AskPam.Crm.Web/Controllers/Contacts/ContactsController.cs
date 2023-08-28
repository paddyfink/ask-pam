using AskPam.Crm.Contacts.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using System.Linq;
using AutoMapper;
using AskPam.Crm.Runtime.Session;
using AskPam.Application.Dto;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using AskPam.Crm.Conversations;
using System;
using AskPam.Crm.Tags;
using AskPam.Extensions;
using AskPam.Crm.Authorization;
using Newtonsoft.Json.Linq;
using AskPam.Domain.Repositories;
using AskPam.Crm.Common;
using AskPam.Crm.Conversations.Dtos;
using AskPam.Domain.Entities;

namespace AskPam.Crm.Contacts
{
    [Authorize]
    [Route("api/[controller]")]
    public class ContactsController : BaseController
    {
        private readonly IRepository<Contact> _contactRepository;
        private readonly IContactManager _contactManager;
        private readonly IUserManager _userManager;
        private readonly ITagsManager _tagManager;
        private readonly IPhoneNumberLookupService _phoneNumberLookupService;


        public ContactsController(
            ICrmSession session,
            IMapper mapper,
            IRepository<Contact> contactRepository,
            IContactManager contactManager,
            IConversationsManager conversationManager,
            ITagsManager tagManager,
            IUserManager userManager, IPhoneNumberLookupService phoneNumberLookupService) : base(session, mapper)
        {
            _contactRepository = contactRepository;
            _contactManager = contactManager;
            _tagManager = tagManager;
            _userManager = userManager;
            _phoneNumberLookupService = phoneNumberLookupService;
        }

        #region Contact
        [HttpPost]
        [ProducesResponseType(typeof(ContactDto), 200)]
        public async Task<IActionResult> CreateContact([FromBody]ContactDto contactDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EnsureOrganization();



            var contact = new Contact(
                contactDto.FirstName,
                contactDto.LastName,
                Session.OrganizationId.Value,
                emailAddress: contactDto.EmailAddress,
                emailAddress2: contactDto.EmailAddress2,
                gender: Enumeration<Gender, string>.FromValue(contactDto.Gender ?? ""),
                dateOfBirthDay: contactDto.DateOfBirth,
                maritalStatus: Enumeration<MaritalStatus, string>.FromValue(contactDto.MaritalStatus ?? ""),
                primaryLanguage: contactDto.PrimaryLanguage,
                secondaryLanguage: contactDto.SecondaryLanguage,
                address: contactDto.Address == null ? null : new Address(
                contactDto.Address.Address1,
                contactDto.Address.Address2,
                contactDto.Address.PostalCode,
                contactDto.Address.City,
                contactDto.Address.Province,
                contactDto.Address.Country),
               bio: contactDto.Bio,
               company: contactDto.Company,
               jobTitle: contactDto.JobTitle,
               groupId: contactDto.GroupId
            );

            if (contactDto.Tags != null && contactDto.Tags.Any())
                contact.TagsRelations = contactDto.Tags.Select(t => new TagsRelation(t.Id, contactId: contact.Id)).ToList();



            contact = await _contactManager.CreateContact(contact);

            if (!string.IsNullOrEmpty(contactDto.AssignedToId))
            {
                var assignee = await _userManager.FindByIdAsync(contactDto.AssignedToId);
                var assigner = await _userManager.FindByIdAsync(Session.UserId);

                await _contactManager.AssignContactToUser(contact, assignee: assignee, assigner: assigner);
            }


            return new ObjectResult(Mapper.Map<Contact, ContactDto>(contact));
        }


        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(ContactDto), 200)]
        public async Task<IActionResult> PatchContact(int id, [FromBody] JObject contactDto)
        {
            var contact = await _contactManager.FindById(0, new Guid());


            return Ok();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ContactDto), 200)]
        public async Task<IActionResult> UpdateContact(int id, [FromBody]ContactDto contactDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EnsureOrganization();

            var contact = await _contactRepository.GetAll()
                .Where(c => c.OrganizationId == Session.OrganizationId.Value && c.Id == id)
                .Include(c => c.Group)
                .Include(c => c.Address)
                .Include(c => c.TagsRelations)
                .ThenInclude(t => t.Tag)
                .FirstOrDefaultAsync();

            contact.Update(
                contactDto.FirstName,
                contactDto.LastName,
                Session.OrganizationId.Value,
                emailAddress: contactDto.EmailAddress,
                emailAddress2: contactDto.EmailAddress2,
                gender: Enumeration<Gender, string>.FromValue(contactDto.Gender),
                dateOfBirth: contactDto.DateOfBirth,
                maritalStatus: Enumeration<MaritalStatus, string>.FromValue(contactDto.MaritalStatus),
                primaryLanguage: contactDto.PrimaryLanguage,
                secondaryLanguage: contactDto.SecondaryLanguage,
                address: new Address(
                    contactDto.Address.Address1,
                    contactDto.Address.Address2,
                    contactDto.Address.PostalCode,
                    contactDto.Address.City,
                    contactDto.Address.Province,
                    contactDto.Address.Country),
                bio: contactDto.Bio,
                company: contactDto.Company,
                jobTitle: contactDto.JobTitle,
                groupId: contactDto.GroupId
            );

            if (!string.IsNullOrEmpty(contactDto.MobilePhone) && contactDto.MobilePhone != contact.MobilePhone?.Number)
            {
                var mobilePhone = await _phoneNumberLookupService.Format(contactDto.MobilePhone);
                contact.UpdateMobilePhone(mobilePhone);
            }
            var tags = contact.TagsRelations;

            contact = await _contactManager.UpdateContact(contact);

            if (contactDto.Tags != null)
            {
                foreach (var tag in tags)
                {
                    if (!contactDto.Tags.Any(t => t.Id == tag.TagId))
                        await _tagManager.Untag(tag.TagId, contactId: contact.Id);
                }

                if (contactDto.Tags.Any())
                {
                    await _tagManager.Tag(contactDto.Tags.Select(t => t.Id).ToList(), contactId: contact.Id);
                }
            }

            if (!string.IsNullOrEmpty(contactDto.AssignedToId) && contactDto.AssignedToId != contact.AssignedToId)
            {
                var assignee = await _userManager.FindByIdAsync(contactDto.AssignedToId);
                var assigner = await _userManager.FindByIdAsync(Session.UserId);

                await _contactManager.AssignContactToUser(contact, assignee, assigner);
            }
            else if (string.IsNullOrEmpty(contactDto.AssignedToId))
            {
                await _contactManager.UnAssignContactToUser(contact);
            }

            return new ObjectResult(Mapper.Map<Contact, ContactDto>(contact));
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(PagedResultDto<ContactListDto>), 200)]
        public async Task<IActionResult> GetContacts([FromBody]ContactListRequestDto input)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EnsureOrganization();

            var query = _contactRepository.GetAll()
                .Include(c => c.Conversations)
                .Where(c => c.OrganizationId == Session.OrganizationId)
                .Include(c => c.AssignedTo)
                .Include(c => c.TagsRelations)
                .ThenInclude(t => t.Tag)
                .Include(c => c.Group)
                .Select(c => c);

            var tags = new List<long>().ToArray();

            if (!input.Filter.IsNullOrEmpty())
            {
                tags = (await _tagManager.Search(input.Filter, Session.OrganizationId.Value)).Select(t => t.Id)
                    .ToArray();
            }

            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                query = query.Where(c =>
                    EF.Functions.Like(c.FullName, $"%{input.Filter}%")
                    || c.TagsRelations.Any(tr => tags.Contains(tr.TagId))
                    || EF.Functions.Like(c.EmailAddress, $"%{input.Filter}%")
                    || EF.Functions.Like(c.Company, $"%{input.Filter}%")
                    );
            }
            if (input.GroupId.HasValue)
            {
                query = query.Where(c => c.GroupId == input.GroupId.Value);
            }

            if (!input.UserId.IsNullOrEmpty())
            {
                query = query.Where(c => c.AssignedToId == input.UserId);
            }

            if (input.WithConversation.HasValue && input.WithConversation == true)
            {
                query = query.Where(c => c.Conversations.Any());
            }
            if (input.WithConversation.HasValue && input.WithConversation == false)
            {
                query = query.Where(c => c.Conversations.Count == 0);
            }

            if (!string.IsNullOrWhiteSpace(input.Sorting))
            {
                query = query.OrderBy(input.Sorting);
            }

            var totalCount = await query.CountAsync();
            var hasNext = (input.SkipCount + input.MaxResultCount) < totalCount;

            var result = await query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToListAsync();

            var contacts = result.Select(t => Mapper.Map<Contact, ContactListDto>(t)).ToList(); //Important, do not put this in the await query, the sort wont work

            return new ObjectResult(
                new PagedResultDto<ContactListDto>(
                    totalCount,
                    contacts,
                    hasNext
                )
            );

        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ContactDto), 200)]
        public async Task<IActionResult> GetContact(int id)
        {
            EnsureOrganization();

            var contact = await _contactRepository.GetAll()
                .Where(c => c.OrganizationId == Session.OrganizationId.Value && c.Id == id)
                .Include(c => c.AssignedTo)
                .Include(c => c.Group)
                .Include(c => c.Address)
                .Include(c => c.TagsRelations)
                .ThenInclude(t => t.Tag)
                .FirstOrDefaultAsync();

            var result = Mapper.Map<Contact, ContactDto>(contact);

            return new ObjectResult(Mapper.Map<Contact, ContactDto>(contact));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), 200)]
        public async Task<IActionResult> DeleteContact(int id)
        {
            EnsureOrganization();

            var contact = await _contactRepository.FirstOrDefaultAsync(c => c.Id == id);
            if (contact != null)
                await _contactManager.DeleteContact(contact);

            return NoContent();
        }

        [HttpPost("{id}/AssignToGroup")]
        [ProducesResponseType(typeof(void), 200)]
        public async Task<IActionResult> AssignToGroup(int id, [FromBody]int groupId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EnsureOrganization();

            var currentContact = await _contactRepository.FirstOrDefaultAsync(c => c.Id == id);
            var group = await _contactManager.GetContactGroup(groupId, Session.OrganizationId.Value);
            currentContact.AddToGroup(group);

            await _contactManager.UpdateContact(currentContact);

            return Ok();
        }


        [HttpGet("recipients")]
        [ProducesResponseType(typeof(IEnumerable<RecipientDto>), 200)]
        public async Task<IActionResult> GetAllRecipients(string filter)
        {
            if (filter.IsNullOrEmpty())
                return new ObjectResult(new List<RecipientDto>());

            var query1 = await _contactRepository.GetAll()
                .Include(c => c.Conversations)
                .Where(c => !string.IsNullOrEmpty(c.EmailAddress))
                .Where(c => EF.Functions.Like(c.FullName, $"%{filter}%")
                            || EF.Functions.Like(c.EmailAddress, $"%{filter}%"))
                .Select(c => new RecipientDto { Name = c.FullName, Recipient = c.EmailAddress, ChannelType = ChannelType.Email.Value, ContactId = c.Id, ConversationsCount = c.Conversations.Count() })
                .ToListAsync();

            var query2 = await _contactRepository.GetAll()
                .Include(c => c.Conversations)
                .Where(c => !string.IsNullOrEmpty(c.EmailAddress2))
                .Where(c => EF.Functions.Like(c.EmailAddress2, $"%{filter}%"))
                .Select(c => new RecipientDto { Name = c.FullName, Recipient = c.EmailAddress2, ChannelType = ChannelType.Email.Value, ContactId = c.Id, ConversationsCount = c.Conversations.Count() })
                .ToListAsync();


            var recipients = query1.Concat(query2);

            return new ObjectResult(recipients);
        }


        #endregion

        #region Contact Group
        [HttpPost("groups")]
        [ProducesResponseType(typeof(ContactGroupDto), 200)]
        public async Task<IActionResult> CreateGroup([FromBody]ContactGroupDto groupDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EnsureOrganization();

            var group = new ContactGroup(
                groupDto.Name,
                Session.OrganizationId.Value
            );

            group = await _contactManager.CreateContactGroup(group);

            return new ObjectResult(Mapper.Map<ContactGroup, ContactGroupDto>(group));
        }

        [HttpPut("groups/{id}")]
        [ProducesResponseType(typeof(void), 200)]
        public async Task<IActionResult> UpdateGroup(int id, [FromBody]ContactGroupDto groupDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EnsureOrganization();

            var group = await _contactManager.GetContactGroup(groupDto.Id, Session.OrganizationId.Value);

            group.Update(
                groupDto.Name
            );

            await _contactManager.UpdateContactGroup(group);

            return new NoContentResult();
        }

        [HttpGet("groups")]
        [ProducesResponseType(typeof(IEnumerable<ContactGroupDto>), 200)]
        public async Task<IActionResult> GetAllGroups()
        {
            EnsureOrganization();

            var result = await _contactManager.GetAllContactGroups(Session.OrganizationId.Value)
                .Include(c => c.Contacts)
                .OrderBy(g => g.Name)
                .Select(t => Mapper.Map<ContactGroup, ContactGroupDto>(t))
                .ToListAsync();

            return new ObjectResult(result);
        }

        [HttpDelete("groups/{id}")]
        [ProducesResponseType(typeof(void), 200)]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            EnsureOrganization();

            var contactGroup = await _contactManager.GetContactGroup(id, Session.OrganizationId.Value);
            await _contactManager.DeleteContactGroup(contactGroup);

            return new NoContentResult();
        }
        #endregion



    }
}
