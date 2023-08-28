using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.AI.Entities
{
    public class AskResponse
    {
        public Answer[] answers { get; set; }
    }
    public class Answer
    {
        public string answer { get; set; }
        public List<string> questions { get; set; }
        public double score { get; set; }
    }
}
