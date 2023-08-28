using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Events
{
    public abstract class AudidtedEvent : IEvent
    {
        public string Type { get { return this.GetType().Name; } }

        public DateTime Created { get; private set; }

        public Dictionary<string, Object> Args { get; private set; }

        public AudidtedEvent()
        {
            this.Created = DateTime.UtcNow;
            this.Args = new Dictionary<string, Object>();
        }

        public abstract void Flatten();
    }
}
