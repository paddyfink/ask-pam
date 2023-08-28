using AskPam.Exceptions;
using AskPam.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AskPam.Events;
using AskPam.Crm.Authorization;
using AskPam.Crm.Conversations;
using AskPam.Crm.Contacts.Events;
using AskPam.Extensions;
using AskPam.EntityFramework.Repositories;
using System.Net;
using System.Text.RegularExpressions;
using AskPam.Crm.Common;

namespace AskPam.Crm.Contacts
{
    public class ContactManager : IContactManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Contact> _contactRepository;
        private readonly IRepository<Conversation> _conversationRepository;
        private readonly IRepository<ContactGroup> _contactGroupRepository;
        private readonly IDomainEvents _domainEvents;
        private readonly IUserManager _userManager;

        public ContactManager(
            IUnitOfWork unitOfWork,
            IPhoneNumberLookupService phoneNumberLookupService,
            IDomainEvents eventer,
            IUserManager userManager,
            IRepository<Contact> contactRepository,
            IRepository<ContactGroup> contactGroupRepository, IRepository<Conversation> conversationRepository)
        {
            _unitOfWork = unitOfWork;

            _domainEvents = eventer;
            _userManager = userManager;
            _contactRepository = contactRepository;
            _contactGroupRepository = contactGroupRepository;
            _conversationRepository = conversationRepository;
        }
        public async Task<Contact> GetAsync(int id)
        {
            var contact = await _contactRepository.FirstOrDefaultAsync(id);
            if (contact == null)
            {
                throw new ApiException("Could not found the client, maybe it's deleted!", HttpStatusCode.NotFound);
            }

            return contact;
        }

        #region Contact
        public async Task<Contact> CreateContact(Contact contact, List<Channel> channels = null)
        {
            await ContactExternalIdValidation(contact);

            if (!string.IsNullOrEmpty(contact.EmailAddress))
                await ContactEmailValidation(contact.OrganizationId, contact.EmailAddress);

            //if (!string.IsNullOrEmpty(contact.EmailAddress2))
            //    await ContactEmailValidation(contact.OrganizationId, contact.EmailAddress2);

            if (contact.GroupId.HasValue)
            {
                var group = await _contactGroupRepository.FirstOrDefaultAsync(contact.GroupId.Value);
                if (group == null)
                {
                    throw new ApiException("There is no contact group with id " + contact.GroupId, HttpStatusCode.BadRequest);
                }
            }

            if (contact.EmailAddress.IsNullOrEmpty() && contact.MobilePhone.Number.IsNullOrEmpty())
                throw new ApiException("A contact must have an email or a phone number", HttpStatusCode.BadRequest);



            var conversation = await _conversationRepository.FirstOrDefaultAsync(c => c.OrganizationId==contact.OrganizationId &&
                c.Email == contact.EmailAddress || (contact.EmailAddress2 != null && c.Email == contact.EmailAddress2));

            if (conversation != null)
            {
                if (contact.Conversations == null)
                {
                    contact.Conversations = new List<Conversation>();
                }
                contact.Conversations.Add(conversation);
            }

            contact = await _contactRepository.InsertAsync(contact);

            await _unitOfWork.SaveChangesAsync();

            await _domainEvents.RaiseAsync(new ContactCreated
            {
                Contact = contact
            }
           );

            return contact;
        }

        public async Task<Contact> UpdateContact(Contact contact, List<Channel> channels = null)
        {
            if (contact.EmailAddress.IsNullOrEmpty() && contact.MobilePhone.Number.IsNullOrEmpty())
                throw new ApiException("A contact must have an email or a phone number", HttpStatusCode.BadRequest);

            if (!string.IsNullOrEmpty(contact.EmailAddress))
                await ContactEmailValidation(contact.OrganizationId, contact.EmailAddress, contact.Id);

            //if (!string.IsNullOrEmpty(contact.EmailAddress2))
            //    await ContactEmailValidation(contact.OrganizationId, contact.EmailAddress2, contact.Id);

            contact = await _contactRepository.UpdateAsync(contact);

            await _unitOfWork.SaveChangesAsync();
            await _domainEvents.RaiseAsync(new ContactEvent());
            return contact;
        }


        public async Task<Contact> GetContact(int id, Guid organizationId)
        {
            var result = await GetAllContacts(organizationId)
                .Where(c => c.Id == id)
                .Include(c => c.AssignedTo)
                .Include(c => c.Group)
                .Include(c => c.TagsRelations)
                    .ThenInclude(t => t.Tag)
                .FirstOrDefaultAsync();

            if (result == null)
            {
                throw new ApiException("Contact not found, maybe it was deleted.", HttpStatusCode.NotFound);
            }

            return result;
        }

        public async Task<Contact> FindById(int id, Guid organizationId)
        {
            var result = await GetAllContacts(organizationId)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<Contact> FindByEmail(string email, Guid organizationId)
        {
            var contact = await GetAllContacts(organizationId)
                .Where(c => c.EmailAddress == email)
                .FirstOrDefaultAsync();

            return contact;
        }

        public async Task<Contact> FindByExternalId(string externalId, Guid organizationId)
        {
            var contact = await GetAllContacts(organizationId)
                .Where(c => c.ExternalId == externalId)
                .FirstOrDefaultAsync();

            return contact;
        }

        public async Task DeleteContact(Contact contact)
        {
            await _contactRepository.DeleteAsync(contact);
            await _unitOfWork.SaveChangesAsync();
            await _domainEvents.RaiseAsync(new ContactDeleted
            {
                Contact = contact
            });
        }

        public IQueryable<Contact> GetAllContacts(Guid organizationId)
        {
            return _contactRepository.GetAll()
                .Where(c => c.OrganizationId == organizationId);
        }

        public async Task AssignContactToUser(Contact contact, User assignee, User assigner = null)
        {
            if (contact.AssignedToId == assignee.Id)
                return;

            User former = null;
            if (!contact.AssignedToId.IsNullOrEmpty())
                former = await _userManager.FindByIdAsync(contact.AssignedToId);

            contact.AssignToUser(assignee.Id);

            contact = await _contactRepository.UpdateAsync(contact);
            await _unitOfWork.SaveChangesAsync();
            await _domainEvents.RaiseAsync(new ContactAssigned
            {
                Contact = contact,
                Assignee = assignee,
                Assigner = assigner
            });

            if (former != null)
                await _domainEvents.RaiseAsync(new ContactUnAssigned
                {
                    Contact = contact,
                    Assignee = assignee,
                    Assigner = assigner
                });
        }

        public async Task UnAssignContactToUser(Contact contact)
        {
            if (contact.AssignedToId.IsNullOrEmpty())
                return;

            var assignee = await _userManager.FindByIdAsync(contact.AssignedToId);

            contact.UnAssign();

            contact = await _contactRepository.UpdateAsync(contact);
            await _unitOfWork.SaveChangesAsync();
            await _domainEvents.RaiseAsync(new ContactUnAssigned
            {
                Contact = contact,
                Assignee = assignee
            });
        }

        #region Private

        private async Task ContactEmailValidation(Guid organizationId, string email, long contactId = 0)
        {
            var query = _contactRepository
                .GetAll()
                .Where(c => !c.IsDeleted)
                .Where(c => c.OrganizationId == organizationId)
                .Where(c => c.EmailAddress != null);

            if (contactId != 0)//If update
            {
                query = query.Where(c => c.Id != contactId);
            }

            var existingContacts = await query
                .AnyAsync(c => c.EmailAddress == email.ToLower()
                              // || c.EmailAddress2 == email.ToLower()
                               //|| c.EmailAddress3 == (email.ToLower())
                               );

            if (existingContacts)
                throw new ApiException($"There is already a contact with email address {email}", HttpStatusCode.BadRequest);

        }

        private async Task ContactExternalIdValidation(Contact contact)
        {
            if (!string.IsNullOrEmpty(contact.ExternalId))
            {
                var isMatch = await _contactRepository
                    .GetAll()
                    .AnyAsync(c => c.Id != contact.Id && c.ExternalId == contact.ExternalId && c.OrganizationId == contact.OrganizationId && c.IsDeleted == false);


                if (isMatch)
                    throw new ApiException($"There is already a contact with External ID : {contact.ExternalId}", HttpStatusCode.BadRequest);
            }
        }
        #endregion 
        #endregion

        #region ContactGroup
        public async Task<ContactGroup> CreateContactGroup(ContactGroup group)
        {
            await ContactGroupNameValidation(group);

            group = await _contactGroupRepository.InsertAsync(group);
            await _unitOfWork.SaveChangesAsync();
            return group;
        }

        public async Task<ContactGroup> UpdateContactGroup(ContactGroup group)
        {
            await ContactGroupNameValidation(group);

            group = await _contactGroupRepository.UpdateAsync(group);
            await _unitOfWork.SaveChangesAsync();
            return group;
        }

        public async Task DeleteContactGroup(ContactGroup group)
        {
            await _contactGroupRepository.DeleteAsync(group);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ContactGroup> GetContactGroup(int id, Guid organizationId)
        {
            var result = await _contactGroupRepository
                .GetAll()
                .Where(c => c.OrganizationId == organizationId)
                .Where(c => c.IsDeleted == false)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (result == null)
            {
                throw new ApiException("Contact group not found, maybe it was deleted.", HttpStatusCode.NotFound);
            }

            return result;
        }
        public async Task<ContactGroup> FindContactGroupByName(string name, Guid organizationId)
        {
            var result = await _contactGroupRepository
                .GetAll()
                .Where(c => c.OrganizationId == organizationId)
                .Where(c => c.IsDeleted == false)
                .Where(c => c.Name == name)
                .FirstOrDefaultAsync();

            if (result == null)
            {
                throw new ApiException("Contact group not found, maybe it was deleted.", HttpStatusCode.NotFound);
            }

            return result;
        }

        public async Task<IEnumerable<Contact>> GetContactsByContactGroup(ContactGroup contactGroup, Guid organizationId)
        {
            var result = await GetAllContacts(organizationId)
                .Where(c => c.GroupId == contactGroup.Id)
                .ToListAsync();

            return result;
        }

        public IQueryable<ContactGroup> GetAllContactGroups(Guid organizationId)
        {
            return _contactGroupRepository.GetAll()
                .Where(c => c.OrganizationId == organizationId);
        }

        #region Private
        private async Task ContactGroupNameValidation(ContactGroup group)
        {
            var query = GetAllContactGroups(group.OrganizationId)
                .Where(g => g.Name == group.Name);

            if (group.Id != 0)
            { //If update
                query = query.Where(g => g.Id != group.Id);
            }

            var existingGroup = await query
                .Where(g => g.Name.Equals(
                        group.Name,
                        StringComparison.CurrentCultureIgnoreCase
                    )
                )
                .AnyAsync();

            if (existingGroup)
                throw new ApiException("There is already a contact group with that name", HttpStatusCode.BadRequest);
        }
        #endregion 
        #endregion
    }
}
