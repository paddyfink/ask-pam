using AskPam.Crm.Authorization;
using AskPam.Crm.Authorization.Followers;
using AskPam.Crm.Common.Interfaces;
using AskPam.Crm.InternalNotes;
using AskPam.Crm.Tags;
using AskPam.Domain.Entities;
using AskPam.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.Posts
{
    public class Post : Entity, IFullAudited, IMustHaveOrganization, ISoftDelete
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public virtual User CreatedUser { get; set; }
        public string CreatedById { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedById { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string DeletedById { get; set; }
        public Guid OrganizationId { get; set; }

        public virtual ICollection<FollowersRelation> Followers { get; set; }
        public virtual ICollection<InternalNote> Notes { get; set; }
        public virtual ICollection<TagsRelation> Tags { get; set; }

        public Post()
        {

        }

        public Post(Guid organizationId, string title, string description)
        {
            OrganizationId = organizationId;
            Title = title;
            Description = description;
        }

        public void Update(string title, string description)
        {
            Title = title;
            Description = description;
        }
    }
}
