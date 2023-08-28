using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.Klik
{
    
    public class Trigger
    {
        public string type { get; set; }
        public int date { get; set; }
    }

    public class Target
    {
        public string type { get; set; }
    }

    public class Transport
    {
        public string type { get; set; }
    }

    public class Template
    {
        public string id { get; set; }
    }

    public class KlikNotification
    {
        public Trigger trigger { get; set; }
        public Target target { get; set; }
        public List<Transport> transports { get; set; }
        public Template template { get; set; }
        public bool active { get; set; }
        public bool allow_duplicates { get; set; }
        public bool ignore_notification_settings { get; set; }
        public string category { get; set; }
        public string type { get; set; }
    }
}
