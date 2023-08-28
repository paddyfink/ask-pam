using AskPam.Domain.Entities;
using AskPam.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AskPam.Authorization
{
    public abstract class UserBase: Entity<string>, IFullAudited, ISoftDelete
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [NotMapped]
        public virtual string FullName => FirstName + " " + LastName;
        public string Email { get; set; }
        public string Picture { get; set; }
        [NotMapped]
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public bool? EmailVerified { get;  set; }

        [NotMapped]
        public dynamic AppMetadata { get; set; }

        [NotMapped]
        public dynamic UserMetadata { get; set; }

        public DateTime? CreatedAt { get; set; }
        public string CreatedById { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedById { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string DeletedById { get; set; }
        public bool IsDeleted { get; set; }
    }
}
