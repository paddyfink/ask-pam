using AskPam.Crm.Contacts.Events;
using AskPam.Events;
using AskPam.Crm.Organizations;
using AskPam.Domain.Repositories;
using AskPam.Extensions;
using System;
using System.Threading.Tasks;
using AskPam.EntityFramework.Repositories;

namespace AskPam.Crm.Contacts
{
    public class ContactHandler : IEventHandler<ContactCreated>
    {
        private readonly IFullContactService _fullContactService;
        private readonly IRepository<PublicInfo> _publicInforepo;
        private readonly IRepository<Organization, Guid> _organizationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ContactHandler(IFullContactService fullContactService, IRepository<PublicInfo> publicInforepo,
            IRepository<Organization, Guid> organizationRepository, IUnitOfWork unitOfWork)
        {
            _fullContactService = fullContactService;
            _publicInforepo = publicInforepo;
            _organizationRepository = organizationRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(ContactCreated args)
        {
            var organization = await _organizationRepository.GetAsync(args.Contact.OrganizationId);

            if (organization.FullContact)
            {
                var publicinfo = await _publicInforepo.FirstOrDefaultAsync(p => p.Email == args.Contact.EmailAddress);

                if (publicinfo == null)
                {
                    var json = await _fullContactService.Search(email: args.Contact.EmailAddress);
                    if (!json.IsNullOrEmpty())
                    {
                        publicinfo = await _publicInforepo.InsertAsync(new PublicInfo { Email = args.Contact.EmailAddress, Data = json });
                        await _unitOfWork.SaveChangesAsync();
                    }
                }

                //if (publicinfo != null)
                //{
                //    _contactRepo.Update(args.Contact.Id, c => c.PublicInfoId = publicinfo.Id);
                //    _contactRepo.SaveChanges();
                //}
            }
        }

    }
}
