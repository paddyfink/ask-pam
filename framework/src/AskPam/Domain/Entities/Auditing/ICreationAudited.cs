using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Domain.Entities.Auditing
{
    /// <summary>
    /// This interface is implemented by entities that is wanted to store creation information (who and when created).
    /// Creation time and creator user are automatically set when saving <see cref="Entity"/> to database.
    /// </summary>
    public interface ICreationAudited
    {
        /// <summary>
        /// The creation time for this entity.
        /// </summary>
        DateTime? CreatedAt { get; set; }
        /// <summary>
        /// created user Id for this entity.
        /// </summary>
        string CreatedById { get; set; }
    }
}
