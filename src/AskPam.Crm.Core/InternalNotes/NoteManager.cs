using AskPam.Exceptions;
using AskPam.Domain.Repositories;
using AskPam.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AskPam.Crm.Common.Interfaces;
using AskPam.Crm.Contacts;
using AskPam.Events;
using AskPam.Crm.Posts.Events;
using AskPam.Crm.Posts;
using System.Net;
using AskPam.EntityFramework.Repositories;

namespace AskPam.Crm.InternalNotes
{
    public class NoteManager : INoteManager
    {
        private readonly IRepository<InternalNote, long> _internalNoteRepository;
        private readonly IRepository<Post> _postRepository;
        private readonly IDomainEvents _domainEvents;
        private readonly IUnitOfWork _unitOfWork;
        public NoteManager(
            IRepository<InternalNote, long> internalNoteRepository,
            IDomainEvents domainEvents,
            IRepository<Post> postRepository, IUnitOfWork unitOfWork)
        {
            _internalNoteRepository = internalNoteRepository;
            _domainEvents = domainEvents;
            _postRepository = postRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<InternalNote>> GetAllAsync(IMustHaveOrganization item)
        {
            var query = GetAllInternalNotes(item.OrganizationId);

            if (item is Contact contact)
            {
                query = query.Where(n => n.ContactId == contact.Id);
            }

            var result = await query
                .OrderBy(n => n.CreatedAt)
                .ToListAsync();

            return result;
        }
        public async Task<InternalNote> GetAsync(int id, Guid organizationId)
        {
            var result = await GetAllInternalNotes(organizationId)
                .FirstOrDefaultAsync(n => n.Id == id);

            if (result == null)
            {
                throw new ApiException("Internal note not found, maybe it was deleted.", HttpStatusCode.NotFound);
            }

            return result;
        }

        public async Task<InternalNote> CreateInternalNote(InternalNote internalNote)
        {
            internalNote = await _internalNoteRepository.InsertAsync(internalNote);
            await _unitOfWork.SaveChangesAsync();

            if (internalNote.PostId.HasValue)
            {
                var post = await _postRepository.FirstOrDefaultAsync(internalNote.PostId.Value);
                await _domainEvents.RaiseAsync(new PostCommented { Post = post, Note = internalNote });
            }

            return internalNote;
        }

        public async Task DeleteInternalNote(InternalNote internalNote)
        {
            await _internalNoteRepository.DeleteAsync(internalNote);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<InternalNote> UpdateInternalNote(InternalNote internalNote)
        {
            internalNote = await _internalNoteRepository.UpdateAsync(internalNote);
            await _unitOfWork.SaveChangesAsync();
            return internalNote;
        }

        public IQueryable<InternalNote> GetAllInternalNotes(Guid organizationId)
        {
            return _internalNoteRepository.GetAll()
                .Include(n => n.CreatedBy)
                .Where(n => n.OrganizationId == organizationId);
        }
    }
}
