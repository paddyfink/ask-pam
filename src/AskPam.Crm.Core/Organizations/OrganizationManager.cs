using AskPam.Crm.Authorization;
using AskPam.Crm.Conversations;
using AskPam.Events;
using AskPam.Crm.Organizations.events;
using AskPam.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AskPam.Crm.Configuration;
using AskPam.EntityFramework.Repositories;

namespace AskPam.Crm.Organizations
{
    public class OrganizationManager : IOrganizationManager
    {
        private readonly IRepository<Organization, Guid> _organizationRepository;
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IRoleManager _roleManager;
        private readonly IDomainEvents _domainEvents;
        private readonly IPostmarkService _emailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISettingManager _settingManager;

        public OrganizationManager(IPostmarkService emailService,
            IRepository<Organization, Guid> organizationRepository,
            IRoleManager roleManager,
             IDomainEvents domainEvents,
            IRepository<UserRole> userRoleRepository, IUnitOfWork unitOfWork, ISettingManager settingManager)
        {
            _organizationRepository = organizationRepository;
            _roleManager = roleManager;
            _domainEvents = domainEvents;
            _emailService = emailService;
            _userRoleRepository = userRoleRepository;
            _unitOfWork = unitOfWork;
            _settingManager = settingManager;
        }

        public async Task<Organization> FindByIdAsync(Guid organizationId)
        {
            if (organizationId == null)
            {
                throw new ArgumentNullException(nameof(organizationId));
            }
            return await _organizationRepository.FirstOrDefaultAsync(organizationId);
        }

        public async Task<Organization> CreateOrganization(Organization organization)
        {
            organization = await _organizationRepository.InsertAsync(organization);
            await _unitOfWork.SaveChangesAsync();

            await _roleManager.CreateStaticRoles(organization.Id);

            //await _domainEvents.RaiseAsync(new OrganizationCreated() { Orgnization = organization });

            return organization;
        }


        public async Task<Organization> AddEmailChannel(Organization org, string email, string name)
        {
            var id = await _emailService.AddEmailSenderAsync(email, name);
            await _settingManager.ChangeSettingForOrganizationAsync(AppSettingsNames.Email.PostmarkSenderId,id.ToString(),org.Id);
            await _settingManager.ChangeSettingForOrganizationAsync(AppSettingsNames.Email.SenderEmail, email, org.Id);
            await _settingManager.ChangeSettingForOrganizationAsync(AppSettingsNames.Email.SenderEmailName, name, org.Id);
            await _organizationRepository.UpdateAsync(org);
            await _unitOfWork.SaveChangesAsync();

            return org;
        }

        public async Task<IEnumerable<string>> GetOrganizationAdmins(Organization org)
        {
            var adminRole = await _roleManager.FindByName(org, RolesName.Admin);
            return await _userRoleRepository.GetAll()
                 .Where(ur => ur.OrganizationId == org.Id && ur.RoleId == adminRole.Id)
                 .Select(ur => ur.UserId)
                 .ToListAsync();
        }
    }
}
