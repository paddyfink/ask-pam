using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.Twilio.Entities
{
    public class TwilioLookUpResponse
    {
        public object caller_name { get; set; }
        public string country_code { get; set; }
        public string phone_number { get; set; }
        public string national_format { get; set; }
        public object carrier { get; set; }
        public object add_ons { get; set; }
        public string url { get; set; }
    }
}
