using AskPam.Crm.Authorization;
using AskPam.Crm.Organizations;
using AskPam.Domain.Entities;
using AskPam.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AskPam.Crm.Configuration
{
   public class Setting: Entity<long>, IAudited
    {
        /// <summary>
        /// Maximum length of the <see cref="Name"/> property.
        /// </summary>
        public const int MaxNameLength = 256;

        
        /// <summary>
        /// TenantId for this setting.
        /// TenantId is null if this setting is not Tenant level.
        /// </summary>
        public  Guid? OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
        /// <summary>
        /// UserId for this setting.
        /// UserId is null if this setting is not user level.
        /// </summary>
        public string UserId { get; set; }
        public virtual User User { get; set; }

        /// <summary>
        /// Unique name of the setting.
        /// </summary>
        [Required]
        [MaxLength(MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// Value of the setting.
        /// </summary>
        public string Value { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedById { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedById { get; set; }

        /// <summary>
        /// Creates a new <see cref="Setting"/> object.
        /// </summary>
        public Setting()
        {

        }

        /// <summary>
        /// Creates a new <see cref="Setting"/> object.
        /// </summary>
        /// <param name="tenantId">TenantId for this setting</param>
        /// <param name="userId">UserId for this setting</param>
        /// <param name="name">Unique name of the setting</param>
        /// <param name="value">Value of the setting</param>
        public Setting(Guid? organizationId, string userId, string name, string value)
        {
            OrganizationId = organizationId;
            UserId = userId;
            Name = name;
            Value = value;
        }
    }
}
