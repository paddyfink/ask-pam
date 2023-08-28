using AskPam.Crm.Authorization.Followers;
using AskPam.Crm.Helpers;
using AskPam.Crm.Presence;
using AskPam.Domain.Entities;
using AskPam.Domain.Entities.Auditing;
using AskPam.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AskPam.Crm.Authorization
{
    public class User : Entity<string>, IFullAudited, ISoftDelete, IPassivable
    {
        //Personal
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string FullName { get; private set; }
        public string Email { get; private set; }
        public string Picture { get; private set; }
        [NotMapped]
        public string Password { get; private set; }
        public string PhoneNumber { get; private set; }
        public bool? EmailVerified { get; private set; }
        public string Signature { get; private set; }

        public string ExternalId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedById { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedById { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string DeletedById { get; set; }
        public bool IsDeleted { get; set; }

        [NotMapped]
        public dynamic AppMetadata { get; set; }

        [NotMapped]
        public dynamic UserMetadata { get; set; }

        // Tables + Foreign Keys
        public virtual ICollection<UserRole> Roles { get; } = new List<UserRole>();
        public virtual ICollection<ConnectedClient> ConnectedClients { get; set; }

        public virtual ICollection<FollowersRelation> Followers { get; } = new List<FollowersRelation>();
        public bool IsActive { get ; set; }

        public User() { }

        public User(
            string firstName,
            string lastName,
            string email
        )
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        public User(
            string id,
            string firstName,
            string lastName,
            string email,
            string picture = null,
            dynamic appMetadata = null,
            string phoneNumber = null
        )
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            if (picture != null) { Picture = picture; }
            if (appMetadata != null) { AppMetadata = appMetadata; }
            if (phoneNumber != null) { PhoneNumber = phoneNumber; }
        }

        internal void Update(
            string firstName,
            string lastName
        )
        {
            FirstName = firstName;
            LastName = lastName;
        }


        public void UpdateEmailSettings(string signature)
        {
            Signature = signature;
        }

        internal void UpdateProfilePicture(string picture)
        {
            Picture = picture;
        }

        internal void RemoveProfilePicture()
        {
            Picture = null;
        }

        public void SetRandomPassword()
        {
            Password = PasswordGenerator.GenerateRandomPassword();
        }

        public void SetPassword(string newPassword)
        {
            Password = newPassword;
        }
    }
}
