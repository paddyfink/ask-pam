using AskPam.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.Presence
{
    public class ConnectedClientEvents : IEvent
    {
        public ConnectedClient Client { get; set; }
    }

    public class UserConnected : ConnectedClientEvents { }
    public class UserDisconnected : ConnectedClientEvents
    {
        public string connectionID { get; set; }

    }
    public class UserReconnected : ConnectedClientEvents { }
}
