using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Domain.Entities.Auditing
{
    /// <summary>
    /// This interface ads <see cref="IDeletionAudited"/> to <see cref="IAudited"/> for a fully audited entity.
    /// </summary>
    public interface IFullAudited : IAudited, IDeletionAudited
    {
    }
}
