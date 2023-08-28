using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Domain.Entities.Auditing
{
    /// <summary>
    /// This interface is implemented by entities that is wanted to store modification information (who and when modified lastly).
    /// Properties are automatically set when updating the <see cref="IEntity"/>.
    /// </summary>
    public interface IModificationAudited
    {
        /// <summary>
        /// The last modified time for this entity.
        /// </summary>
        DateTime? ModifiedAt { get; set; }
        /// <summary>
        /// Last modified user id for this entity.
        /// </summary>
        string ModifiedById { get; set; }
    }
}
