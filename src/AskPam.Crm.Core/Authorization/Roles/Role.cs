using AskPam.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AskPam.Crm.Authorization
{
    public class Role : Entity
    {
        /// <summary>
        /// Organization's Id, if this role is a tenant-level role. Null, if not.
        /// </summary>
        public Guid? OrganizationId { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Display name of this role.
        /// </summary>
        [Required]
        [StringLength(64)]
        public virtual string DisplayName { get; set; }

        /// <summary>
        /// Is this a static role?
        /// Static roles can not be deleted, can not change their name.
        /// They can be used programmatically.
        /// </summary>
        public virtual bool IsStatic { get; set; }

        /// <summary>
        /// Is this role will be assigned to new users as default?
        /// </summary>
        public virtual bool IsDefault { get; set; }

        /// <summary>
        /// Navigation property for the users in this role.
        /// </summary>
        public virtual ICollection<UserRole> Users { get; } = new List<UserRole>();

        /// <summary>
        /// Creates a new <see cref="Role{TTenant,TUser} role.
        /// </summary>
        public Role()
        {
            Name = Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// Creates a new <see cref="AbpRole{TTenant,TUser}"/> object.
        /// </summary>
        /// <param name="accountId">TenantId or null (if this is not a tenant-level role)</param>
        /// <param name="displayName">Display name of the role</param>
        public Role(Guid? organizationId, string displayName)
            : this()
        {
            OrganizationId = organizationId;
            DisplayName = displayName;
        }

        /// <summary>
        /// Creates a new <see cref="AbpRole{TTenant,TUser}"/> object.
        /// </summary>
        /// <param name="accountId">TenantId or null (if this is not a tenant-level role)</param>
        /// <param name="name">Unique role name</param>
        /// <param name="displayName">Display name of the role</param>
        public Role(Guid organizatonId, string name, string displayName)
            : this(organizatonId, displayName)
        {
            Name = name;
        }

        public override string ToString()
        {
            return string.Format("[Role {0}, Name={1}]", Id, Name);
        }
    }
}
