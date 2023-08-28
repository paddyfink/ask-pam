using AskPam.Crm.Common.Interfaces;
using AskPam.Domain.Entities;
using AskPam.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AskPam.Crm.Contacts
{
    [Table("ContactGroups")]
    public class ContactGroup : Entity, IMustHaveOrganization, IFullAudited, ISoftDelete
    {
        [MaxLength(50)]
        [Required]
        public string Name { get; private set; } //Mandatory
        public virtual ICollection<Contact> Contacts { get; set; }

        //Settings
        public Guid OrganizationId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedById { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedById { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string DeletedById { get; set; }
        public bool IsDeleted { get; set; }

        internal ContactGroup(){}

        public ContactGroup(
            string name,
            Guid organizationId
        )
        {
            Name = name;
            OrganizationId = organizationId;
        }
        public void Update(string name)
        {
            Name = name;
        }
    }
}
