using AskPam.Crm.Organizations;
using AskPam.Crm.Runtime.Session;
using AskPam.Domain.Repositories;
using AskPam.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AskPam.EntityFramework.Repositories;

namespace AskPam.Crm.Authorization
{
    public class RoleManager : IRoleManager
    {
        //public ISession Session { get; set; }
        private readonly IRepository<Role> _roleReposiotry;
        private readonly IUnitOfWork _unitOfWork;


        public RoleManager(IRepository<Role> roleReposiotry, IUnitOfWork unitOfWork)
        {
            _roleReposiotry = roleReposiotry;
            _unitOfWork = unitOfWork;
        }
        public async Task CreateStaticRoles(Guid organizationId)
        {
            var staticRoleDefinitions = new[]
            {
                new { RoleName = RolesName.Admin },
                new { RoleName = RolesName.User },
                new { RoleName = RolesName.Reader }
            };

            foreach (var staticRoleDefinition in staticRoleDefinitions)
            {
                var role = new Role
                {
                    OrganizationId = organizationId,
                    Name = staticRoleDefinition.RoleName,
                    DisplayName = staticRoleDefinition.RoleName,
                    IsStatic = true
                };

                await _roleReposiotry.InsertAsync(role);
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public virtual async Task AddUserToRole(Role role, User user, Organization organization)
        {
            role.Users.Add(new UserRole { RoleId = role.Id, UserId = user.Id, OrganizationId = organization.Id });
            await _roleReposiotry.UpdateAsync(role);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Role> FindByName(Organization org, string roleName)
        {
            return await _roleReposiotry.FirstOrDefaultAsync(r => r.Name == roleName && r.OrganizationId == org.Id);
        }
    }
}
