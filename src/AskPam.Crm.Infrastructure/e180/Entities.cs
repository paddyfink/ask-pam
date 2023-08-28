using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.e180
{
    public class Statistics
    {
        public int offers { get; set; }
        public int requests { get; set; }
        public int confirmed { get; set; }
        public int pendings { get; set; }
    }

    public class Result
    {
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string url { get; set; }
        public string ticket_id { get; set; }
        public Statistics statistics { get; set; }
    }

    public class BraindatesSummary
    {
        public int start_index { get; set; }
        public int count { get; set; }
        public string next { get; set; }
        public List<Result> results { get; set; }
        public int page { get; set; }
        public object previous { get; set; }
        public int end_index { get; set; }
    }
}
