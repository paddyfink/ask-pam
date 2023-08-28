using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.AI
{
    public class QnAResult
    {
        public string Answer { get; set; }
        public List<string> Questions { get; set; }
        public double Score { get; set; }
    }
}
