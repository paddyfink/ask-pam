using AskPam.Crm.Common.Interfaces;
using AskPam.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AskPam.Crm.AI.Entities
{
    public class QnAPair : Entity, IMustHaveOrganization, ISoftDelete
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public Guid OrganizationId { get; set; }
        public bool IsDeleted { get; set; }

        internal QnAPair()
        {

        }

        public QnAPair(string question, string answer, Guid organizationId)
        {
            Question = question;
            Answer = answer;
            OrganizationId = organizationId;
        }

        public void UpdateQuestion(string question)
        {
            Question = question;
        }

        public void UpdateAnswer(string answer)
        {
            Answer = answer;
        }
    }
}
