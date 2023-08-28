using AskPam.Crm.Authorization;
using AskPam.Crm.Common.Interfaces;
using AskPam.Crm.Contacts;
using AskPam.Crm.Conversations;
using AskPam.Crm.Library;
using AskPam.Crm.Organizations;
using AskPam.Crm.Posts;
using AskPam.Domain.Entities;
using AskPam.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.InternalNotes
{
    public class InternalNote : Entity<long>, IMustHaveOrganization, IFullAudited, ISoftDelete
    {
        public string Comment { get; private set; }

        public virtual Organization Organization { get; set; }
        public Guid OrganizationId { get; set; }

        public virtual Contact Contact { get; set; }
        public int? ContactId { get; set; }

        public virtual LibraryItem Library { get; set; }
        public int? LibraryItemId { get; set; }

        public virtual Post Post { get; set; }
        public int? PostId { get; set; }


        public virtual User CreatedBy { get; set; }
        public string CreatedById { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedById { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string DeletedById { get; set; }
        public bool IsDeleted { get; set; }




        internal InternalNote()
        {

        }

        public InternalNote(string comment, int? contactId = null, int? postId = null, int? libraryId = null)
        {
            Comment = comment;
            ContactId = contactId;
            PostId = postId;
            LibraryItemId = libraryId;
        }

    }

}

