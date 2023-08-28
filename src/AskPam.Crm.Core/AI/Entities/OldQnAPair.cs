using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AskPam.Crm.AI.Entities
{
    public class OldQnAPair
    {
        public QnAPair Old { get; set; }
        public string NewQuestion { get; set; }
        public string NewAnswer { get; set; }

        public OldQnAPair(QnAPair old, string newQuestion, string newAnswer)
        {
            Old = old;
            NewQuestion = newQuestion;
            NewAnswer = newAnswer;
        }
    }
}
