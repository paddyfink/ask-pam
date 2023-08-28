using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AskPam.Crm.Common.Interfaces;
using AskPam.Domain.Services;

namespace AskPam.Crm.InternalNotes
{
    public interface INoteManager: IDomainService
    {
        Task<InternalNote> CreateInternalNote(InternalNote internalNote);
        Task DeleteInternalNote(InternalNote internalNote);
        Task<IEnumerable<InternalNote>> GetAllAsync(IMustHaveOrganization item);
        IQueryable<InternalNote> GetAllInternalNotes(Guid organizationId);
        Task<InternalNote> GetAsync(int id, Guid organizationId);
        Task<InternalNote> UpdateInternalNote(InternalNote internalNote);
    }
}