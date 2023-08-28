using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Domain.Entities
{
    public interface IAggregateRoot
    {
        Guid Uid { get; set; }
    }
}
