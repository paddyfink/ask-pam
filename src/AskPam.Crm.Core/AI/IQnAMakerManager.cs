using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AskPam.Crm.AI.Entities;
using AskPam.Domain.Services;

namespace AskPam.Crm.AI
{
    public interface IQnAMakerManager: IDomainService
    {
        Task<QnAResult> Ask(string question, Guid organizationId);
        Task<IEnumerable<QnAPair>> FindbyIds(IEnumerable<long> ids, Guid organizationId);
        IQueryable<QnAPair> GetAllQnAPairs(Guid organizationId);
        Task<IEnumerable<QnAPair>> SaveKnowledgeBase(IEnumerable<QnAPair> newQnAs, IEnumerable<OldQnAPair> editQnAs, IEnumerable<QnAPair> delQnAs, Guid organizationId);
    }
}