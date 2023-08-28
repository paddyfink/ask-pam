//using AskPam.Crm.Organizations;
//using AskPam.Domain.Entities;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace AskPam.Crm.Authorization
//{
//   public class UserOrganization : Entity
//    {
//        /// <summary>
//        /// Gets or sets the primary key of the user that is linked to a role.
//        /// </summary>
//        public virtual string UserId { get; set; }

//        public bool Default { get; set; }
//        /// <summary>
//        /// Account's Id, if this role is a tenant-level role. Null, if not.
//        /// </summary>
//        public virtual Guid OrganizationId { get; set; }
//        public virtual Organization Organization { get; set; }
//    }
//}
