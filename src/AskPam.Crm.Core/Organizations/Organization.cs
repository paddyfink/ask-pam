using AskPam.Crm.Authorization;
using AskPam.Domain.Entities;
using AskPam.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AskPam.Crm.Organizations
{
    public class Organization : Entity<Guid>, IFullAudited, ISoftDelete, IPassivable
    {
        public string Name { get; set; }
       
        public OrganizationType? Type { get; set; }
        //public string PostmarkSenderId { get; set; }
        //public bool IsSpfVerified { get; set; }
        //public bool IsDkimVerified { get; set; }
        //public string SmoochAppId { get; set; }
        //public string SmoochAppToken { get; set; }
        public string ImageUrl { get; set; }
        public bool FullContact { get; set; }
        public bool Klik { get; set; }
        public bool BrainDates { get; set; }
        public bool Stay22 { get; set; }

        public virtual ICollection<UserRole> Users { get; } = new List<UserRole>();
        public DateTime? CreatedAt { get; set; }
        public string CreatedById { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedById { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string DeletedById { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

        public Organization()
        {

        }

        public Organization(string name)
        {
            Name = name;
            IsActive = true;
        }
    }
}
