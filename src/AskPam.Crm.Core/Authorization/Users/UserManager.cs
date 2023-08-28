using AskPam.Crm.Organizations;
using AskPam.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AskPam.Exceptions;
using Z.EntityFramework.Plus;
using AskPam.Extensions;
using AskPam.EntityFramework.Repositories;
using System.Net;
using AskPam.Crm.Configuration;

namespace AskPam.Crm.Authorization
{
    public class UsersManager : IUserManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IRepository<User, string> _userRepository;
        private readonly IRepository<Organization, Guid> _organizationRepository;
        private readonly IExternalUserService _userAuthService;
        private readonly ISettingManager _settingManager;

        public UsersManager(

            IExternalUserService userAuthService,
            IUnitOfWork unitOfWork, ISettingManager settingManager, IRepository<Organization, Guid> organizationRepository, IRepository<User, string> userRepository, IRepository<UserRole> userRoleRepository, IRepository<Role> roleRepository)
        {
            _unitOfWork = unitOfWork;
            _settingManager = settingManager;
            _organizationRepository = organizationRepository;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;

            _userAuthService = userAuthService;
        }

        public async Task<Token> AuthenticateUser(string email, string password)
        {
            return await _userAuthService.AuthenticateUser(email, password);
        }

        public async Task<User> GetUserInfo(string token)
        {
            return await _userAuthService.GetUserInfo(token);
        }

        public async Task AddToRole(User user, string roleName, Guid organizationId)
        {
            var role = await FindRoleAsync(roleName, organizationId);
            var userRole = await _userRoleRepository.FirstOrDefaultAsync(ur => ur.UserId == user.Id && ur.OrganizationId == organizationId);
            if (userRole != null)
            {
                userRole.RoleId = role.Id;
                await _userRoleRepository.UpdateAsync(userRole);
            }
            else
            {
                await _userRoleRepository.InsertAsync(new UserRole
                {
                    UserId = user.Id,
                    RoleId = role.Id,
                    OrganizationId = organizationId
                });
            }
            await _unitOfWork.SaveChangesAsync();
        }


        public async Task<User> CreateOrAddtoOrganization(string firstname, string lastname, string email, string roleName, Guid organizationId)
        {
            var role = await FindRoleAsync(roleName, organizationId);

            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }


            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(email, "Email can't be empty");
            }

            if (await _userRepository.GetAll().AnyAsync(u => u.Email == email && u.Roles.Any(o => o.OrganizationId == organizationId))) //TODO What will happen for deleted User?
            {
                throw new ApiException("User already belongs to the organization", HttpStatusCode.BadRequest);
            }

            var user = await FindByEmailAsync(email);

            if (user == null)
            {
                user = await _userAuthService.GetUser(email);

                if (user == null)
                {
                    if (string.IsNullOrEmpty(firstname))
                    {
                        throw new ArgumentNullException(firstname, "Firstname can't be empty");
                    }
                    if (string.IsNullOrEmpty(lastname))
                    {
                        throw new ArgumentNullException(lastname, "Lastname can't be empty");
                    }

                    user = await _userAuthService.CreateUser(firstname, lastname, email);
                }

                user = await _userRepository.InsertAsync(user);
                await _unitOfWork.SaveChangesAsync();
            }

            var userRole = new UserRole()
            {
                OrganizationId = organizationId,
                RoleId = role.Id,
                UserId = user.Id
            };
            userRole = await _userRoleRepository.InsertAsync(userRole);
            await _unitOfWork.SaveChangesAsync();



            user.Roles.Add(userRole);

            return user;
        }

        public async Task RemoveUSer(Guid organizationId, string userId)
        {
            var role = await _userRoleRepository.FirstOrDefaultAsync(ur => ur.OrganizationId == organizationId && ur.UserId == userId);

            if (role != null)
            {
                await _userRoleRepository.DeleteAsync(role);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task ForgotPassword(string email)
        {
            await _userAuthService.ForgotPassword(email);
        }

        public async Task<User> UpdateProfile(User user, string firstName, string lastName)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.Update(
                firstName,
                lastName
            );

            await _userAuthService.UpdateProfile(user);
            return await UpdateUser(user);
        }

        public async Task<User> UpdateProfilePicture(User user, string picture)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.UpdateProfilePicture(picture);
            await _userAuthService.UpdateProfilePicture(user);

            user = await UpdateUser(user);
            return user;
        }

        public async Task<User> ResetProfilePicture(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.RemoveProfilePicture();
            await _userAuthService.RemoveProfilePicture(user);
            var authUser = await _userAuthService.GetUser(user.Email);
            user.UpdateProfilePicture(authUser.Picture);

            user = await UpdateUser(user);
            return user;
        }

        public async Task<User> UpdateEmailSettings(User user, string signature)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.UpdateEmailSettings(signature);
            user = await UpdateUser(user);
            return user;
        }

        public async Task ChangePassword(User user, string password, Guid organisationId)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            await _userAuthService.ChangePassword(user.Id, password);
        }

        public virtual async Task<IEnumerable<string>> GetRolesAsync(string userId, Guid? organizationId = null)
        {
            if (userId.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var query = from userRole in _userRoleRepository.GetAll()
                        join role in _roleRepository.GetAll() on userRole.RoleId equals role.Id
                        where userRole.UserId.Equals(userId) && (userRole.OrganizationId.Equals(organizationId) || !userRole.OrganizationId.HasValue)
                        select role.Name;
            return await query.ToListAsync();
        }

        public virtual async Task<IEnumerable<Organization>> GetOrganizationsAsync(string userId)
        {
            if (userId.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var query = from userRole in _userRoleRepository.GetAll()
                        join organization in _organizationRepository.GetAll() on userRole.OrganizationId equals organization.Id
                        where userRole.UserId.Equals(userId) && organization.IsActive
                        select organization;
            return await query.ToListAsync();
        }

        public async Task<Organization> GetDefaultOrganizationAsync(string userId)
        {

            if (userId.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var query = from userRole in _userRoleRepository.GetAll()
                        join organization in _organizationRepository.GetAll() on userRole.OrganizationId equals organization.Id
                        where userRole.UserId.Equals(userId) && organization.IsActive
                        orderby userRole.Default descending
                        select organization;
            return await query.FirstOrDefaultAsync();
        }

        public async Task SetDefaultOrganizationAsync(string userId, Guid organizationId)
        {
            var userRole = await _userRoleRepository.GetAll()
              .Where(o => o.UserId == userId)
              .Where(o => o.OrganizationId == organizationId)
              .FirstOrDefaultAsync();

            if (userRole != null)
            {
                userRole.Default = true;

                await _userRoleRepository.UpdateAsync(userRole);
                await _unitOfWork.SaveChangesAsync();

                _userRoleRepository.GetAll()
                    .Where(x => x.UserId == userId && x.OrganizationId != organizationId)
                    .Update(x => new UserRole() { Default = false });
            }
        }

        /// <summary>
        /// Return a user role for the userId and roleId if it exists.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <param name="roleId">The role's id.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The user role if it exists.</returns>
        protected Task<UserRole> FindUserRoleAsync(string userId, int roleId)
        {
            return _userRoleRepository.GetAll().FirstOrDefaultAsync(u => u.UserId.Equals(userId) && u.RoleId.Equals(roleId));
        }

        /// <summary>
        /// Retrieves all users in the specified role.
        /// </summary>
        /// <param name="normalizedRoleName">The role whose users should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> contains a list of users, if any, that are in the specified role. 
        /// </returns>
        public virtual async Task<IList<User>> GetUsersInRoleAsync(Guid organizationId, string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException(nameof(roleName));
            }

            var role = await FindRoleAsync(roleName, organizationId);

            if (role != null)
            {
                var query = from userrole in _userRoleRepository.GetAll()
                            join user in GetAllUsers(organizationId) on userrole.UserId equals user.Id
                            where userrole.RoleId.Equals(role.Id)
                            select user;

                return await query.ToListAsync();
            }
            return new List<User>();
        }

        private async Task<Role> FindRoleAsync(string roleName, Guid organizationId)
        {
            return await _roleRepository.FirstOrDefaultAsync(r => r.Name == roleName && r.OrganizationId == organizationId);
        }

        public async Task<User> FindByIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }
            return await _userRepository.GetAll()
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }
            return await _userRepository.GetAll()
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public IQueryable<User> GetAllUsers(Guid organizationId)
        {
            var emailsToExclude = _settingManager
                .GetSettingValueForApplicationAsync(AppSettingsNames.Application.EmailsToExclude).Result.Split(',');

            return _userRepository.GetAll()
                .Where(u => !emailsToExclude.Contains(u.Email))
                .Where(u => u.Roles.Any(o => o.OrganizationId.Equals(organizationId)));
        }

        #region Roles

        public IQueryable<Role> GetAllRoles(Guid organizationId)
        {
            return _roleRepository.GetAll()
                .Where(r => r.OrganizationId == organizationId);
        }
        #endregion

        #region UserRoles

        public IQueryable<UserRole> GetAllUserRoles(Guid organizationId)
        {
            return _userRoleRepository.GetAll()
                .Where(ur => ur.OrganizationId == organizationId); ;
        }
        #endregion

        #region Private
        private async Task<User> UpdateUser(User user)
        {
            user = await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return user;
        }
        #endregion
    }
}
