using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Domain.Entities.Auditing
{
    /// <summary>
    /// This interface is implemented by entities which wanted to store deletion information (who and when deleted).
    /// </summary>
    public interface IDeletionAudited
    {
        /// <summary>
        /// The deletion time for this entity.
        /// </summary>
        DateTime? DeletedAt { get; set; }
        /// <summary>
        /// Which user deleted this entity?
        /// </summary>
        string DeletedById { get; set; }
    }
}
