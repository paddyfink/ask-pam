using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.Klik
{
      
    public class CallToAction
    {
        public string url { get; set; }
        public string label { get; set; }
        public string target { get; set; }
    }

    public class KlikTemplate
    {
        public string id { get; set; }
        public string name { get; set; }
        public string subject { get; set; }
        public string html { get; set; }
        public string text { get; set; }
        public string short_text { get; set; }
        public CallToAction call_to_action { get; set; }
    }
}
