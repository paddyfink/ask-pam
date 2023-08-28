using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AskPam.Crm.Authorization;
using AskPam.Crm.Conversations;
using AskPam.Domain.Services;

namespace AskPam.Crm.Contacts
{
    public interface IContactManager: IDomainService
    {
        Task<Contact> CreateContact(Contact contact, List<Channel> channels = null);
        Task<ContactGroup> CreateContactGroup(ContactGroup group);
        Task DeleteContact(Contact contact);
        Task DeleteContactGroup(ContactGroup group);
        Task<Contact> GetContact(int id, Guid organizationId);
        Task<Contact> FindByEmail(string email, Guid organizationId);
        Task<Contact> FindByExternalId(string externalId, Guid organizationId);
        Task<Contact> FindById(int id, Guid organizationId);
        Task<ContactGroup> FindContactGroupByName(string name, Guid organizationId);
        IQueryable<ContactGroup> GetAllContactGroups(Guid organizationId);
        IQueryable<Contact> GetAllContacts(Guid organizationId);
        Task<Contact> GetAsync(int id);
        Task<ContactGroup> GetContactGroup(int id, Guid organizationId);
        Task<IEnumerable<Contact>> GetContactsByContactGroup(ContactGroup contactGroup, Guid organizationId);
        Task<Contact> UpdateContact(Contact contact, List<Channel> channels = null);
        Task<ContactGroup> UpdateContactGroup(ContactGroup group);
        Task AssignContactToUser(Contact contact, User assignee, User assigner = null);
        Task UnAssignContactToUser(Contact contact);
    }
}