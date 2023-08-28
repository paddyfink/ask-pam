using AskPam.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Domain.Entities
{
    public class FullAuditedEntity : FullAuditedEntity<long>
    {

    }

    //[Serializable]
    public abstract class FullAuditedEntity<TPrimaryKey> : IEntity<TPrimaryKey>, IFullAudited
    {
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        public virtual TPrimaryKey Id { get; set; }

        public DateTime? CreatedAt { get; set; }
        public string CreatedById { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedById { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string DeletedById { get; set; }


    }
}
