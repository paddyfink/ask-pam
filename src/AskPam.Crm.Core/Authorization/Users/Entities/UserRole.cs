using AskPam.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using AskPam.Crm.Organizations;

namespace AskPam.Crm.Authorization
{
    public class UserRole : Entity
    {
        /// <summary>
        /// Gets or sets the primary key of the user that is linked to a role.
        /// </summary>
        public virtual string UserId { get; set; }

        /// <summary>
        /// Gets or sets the primary key of the role that is linked to the user.
        /// </summary>
        public virtual long RoleId { get; set; }

        /// <summary>
        /// Organization's Id, if this role is a tenant-level role. Null, if not.
        /// </summary>
        public virtual Guid? OrganizationId { get; set; }
        //public virtual Organization Organization { get; set; }

        public bool Default { get; set; }
    }
}
