using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AskPam.Events
{
    public interface IEventHandler<T> where T : IEvent
    {
        Task Handle(T args);
    }
}
