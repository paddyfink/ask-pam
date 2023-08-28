using AskPam.Crm.AI.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AskPam.Crm.AI
{
    public interface IQnAMakerService
    {
        Task<IEnumerable<QnAResult>> Ask(string question, Guid knowledgeBaseId);
        Task<Guid> CreateKnowledgeBase(string name, IEnumerable<QnAPair> qnAPairs);
        Task UpdateKnowledgeBase(Guid knowledgeBaseId, IEnumerable<QnAPair> newQnAPairs, IEnumerable<OldQnAPair> editQnAs, IEnumerable<QnAPair> delQnAs);
        Task Publish(Guid knowledgeBaseId);
    }
}
