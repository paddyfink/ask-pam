using AskPam.Crm.Organizations;
using AskPam.Domain.Entities;
using AskPam.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;

namespace AskPam.Crm.Tags
{
    public class Tag : Entity, ICreationAudited, ISoftDelete
    {
        public string Name { get; private set; }
        public string Category { get; private set; }

        public string FullName => string.IsNullOrEmpty(Category) ? Name : $"{Name} : {Category}";

        //Settings
        public DateTime? CreatedAt { get; set; }
        public string CreatedById { get; set; }
        public bool IsDeleted { get; set; }


        //Table + Foreign Keys
        public Guid OrganizationId { get; private set; }
        public virtual Organization Organization { get; set; }
        public virtual ICollection<TagsRelation> TagsRelations { get; set; }

        internal Tag()
        {

        }
        public Tag(string name, Guid organizationId, string category = null)
        {
            Name = name;
            Category = category;
            OrganizationId = organizationId;
        }

        public void Update(string name, string category)
        {
            Name = name;
            Category = category;
        }

    }
}