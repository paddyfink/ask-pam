using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AskPam.Events
{
    public interface IDomainEvents
    {
        //void Raise<T>(T args) where T : IEvent;
        Task RaiseAsync<T>(T args) where T : IEvent;
    }
}
