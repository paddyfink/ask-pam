using AskPam.Exceptions;
using AskPam.Domain.Repositories;
using AskPam.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace AskPam.Crm.Contacts
{
    public class ContactCreationPolicy : IDomainService
    {
        private readonly IRepository<Contact> _contactsRepository;

        public ContactCreationPolicy(IRepository<Contact> contactsRepository)
        {
            _contactsRepository = contactsRepository;
        }

        private async Task CheckExternalIdUnicity(Contact contact)
        {
            if (!string.IsNullOrEmpty(contact.ExternalId))
            {
                var isMatch =  await _contactsRepository
                    .GetAll()
                    .AnyAsync(c => c.Id != contact.Id && c.ExternalId == contact.ExternalId && c.OrganizationId == contact.OrganizationId && c.IsDeleted == false);


                if (isMatch)
                    throw new ApiException($"There is already a contact with External ID : {contact.ExternalId}", HttpStatusCode.BadRequest);
            }
        }
    }
}
