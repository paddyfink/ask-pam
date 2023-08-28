using AskPam.Crm.Contacts;
using AskPam.Crm.InternalNotes.Dtos;
using AskPam.Crm.InternalNotes;
using AskPam.Crm.Runtime.Session;
using AskPam.Domain.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.InternalNotes
{
    [Authorize]
    [Route("api/[controller]")]
    public class NotesController : BaseController
    {
        private readonly INoteManager _internalNoteManager;
        private IContactManager _contactManager;
        private IRepository<InternalNote, long> _internalNoteRepository;

        public NotesController(
            ICrmSession session,
            IMapper mapper,
            INoteManager internalNoteManager,
            IRepository<InternalNote, long> internalNoteRepository,
            IContactManager contactManager
            ) : base(session, mapper)
        {
            _internalNoteManager = internalNoteManager;
            _internalNoteRepository = internalNoteRepository;
            _contactManager = contactManager;
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(IEnumerable<NoteDto>), 200)]
        public async Task<IActionResult> GetNotes([FromBody]GetNotesRequestDto request)
        {
            EnsureOrganization();            

            var result = await _internalNoteManager.GetAllInternalNotes(Session.OrganizationId.Value)
                .Where(n=>n.ContactId==request.ContactId)
                .Where(n=>n.PostId == request.PostId)
                .ToListAsync();

            return new ObjectResult(result.Select(r => Mapper.Map<InternalNote, NoteDto>(r)));
        }

        [HttpPost]
        [ProducesResponseType(typeof(NoteDto), 200)]
        public async Task<IActionResult> CreateNote([FromBody]NoteDto input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EnsureOrganization();
            

            var internalNote = new InternalNote(
                input.Comment,
                contactId: input.ContactId,
                postId:input.PostId
            );

            internalNote = await _internalNoteManager.CreateInternalNote(internalNote);

            return new ObjectResult(Mapper.Map<InternalNote, NoteDto>(internalNote));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            EnsureOrganization();

            var internalNote = await _internalNoteManager.GetAsync(id, Session.OrganizationId.Value);

            //TODO Manage if user is the owner or allowed Role 
            await _internalNoteManager.DeleteInternalNote(internalNote);

            return Ok();
        }
    }
}
