using AskPam.Crm.Tags;
using AskPam.Crm.Runtime.Session;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AskPam.Application.Dto;
using Microsoft.AspNetCore.Authorization;
using AskPam.Crm.Tags.Dtos;
using AskPam.Crm.Tags.Dto;
using AskPam.Domain.Repositories;

namespace AskPam.Crm.Tags
{
    [Authorize]
    [Route("api/[controller]")]
    public class TagsController : BaseController
    {
        private ITagsManager _tagsManager;
        private IRepository<Tag> _tagsRepository;
        private IRepository<TagsRelation> _tagsRelationRepository;

        public TagsController(
            ICrmSession session,
            IMapper mapper,
            ITagsManager tagsManager,
            IRepository<Tag> tagsRepository,
             IRepository<TagsRelation> tagsRelationRepository)
            : base(session, mapper)
        {
            _tagsManager = tagsManager;
            _tagsRepository = tagsRepository;
            _tagsRelationRepository = tagsRelationRepository;
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Tag([FromBody]TagRelationDto tagRelationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EnsureOrganization();

            var tag = await _tagsManager.FindByIdAsync(tagRelationDto.TagId, Session.OrganizationId.Value);

            await _tagsManager.Tag(tag, contactId: tagRelationDto.ContactId, libraryItemId: tagRelationDto.LibraryId, conversationId: tagRelationDto.ConversationId, messageId: tagRelationDto.MessageId);

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Untag([FromBody]TagRelationDto tagRelationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EnsureOrganization();

            var tag = await _tagsManager.FindByIdAsync(tagRelationDto.TagId, Session.OrganizationId.Value);
            await _tagsManager.Untag(tag.Id, tagRelationDto.ContactId, libraryItemId: tagRelationDto.LibraryId, conversationId: tagRelationDto.ConversationId, messageId: tagRelationDto.MessageId);

            return Ok();
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(IEnumerable<TagDto>), 200)]
        public async Task<IActionResult> GetAllTags()
        {
            EnsureOrganization();

            var result = await _tagsManager.GetAllTags(Session.OrganizationId.Value).ToListAsync();
            return new ObjectResult(result.Select(m => Mapper.Map<Tag, TagDto>(m)));
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(IEnumerable<TagDto>), 200)]
        public async Task<IActionResult> GetItemTags([FromBody]GetItemTagsRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EnsureOrganization();

            var result = await _tagsManager
                .GetItemTags(
                    Session.OrganizationId.Value,
                    request.ContactId,
                    request.LibraryId
                )
                .ToListAsync();

            return new ObjectResult(result.Select(m => Mapper.Map<Tag, TagDto>(m)));
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(PagedResultDto<TagListDto>), 200)]
        public async Task<IActionResult> GetAllTagsList([FromBody]TagListRequestDto input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EnsureOrganization();

            //var query = _tagsRepository.GetAll()
            //    .Where(t => t.OrganizationId == Session.OrganizationId)
            //    .Select(t => new TagListDto
            //    {
            //        Id = t.Id,
            //        Name = t.Name,
            //        FullName = t.FullName,
            //        CreatedAt = t.CreatedAt,
            //        Category = t.Category,
            //        ContactsCount = t.TagsRelations.Count(tr => tr.ContactId != null),
            //        //LibraryItemsCount = t.TagsRelations.Count(),
            //        //ConversationsCount = t.TagsRelations.Count()
            //    });

            //var query = from tag in _tagsRepository.GetAll()
            //            join tagRelation in _tagsRelationRepository.GetAll() on tag.Id equals tagRelation.TagId
            //            where tag.OrganizationId == Session.OrganizationId.Value
            //            group tag by new
            //            {
            //                Id = tag.Id,
            //                Name= tag.Name,
            //                FullName = tag.FullName,
            //                ContactsCount = tagRelation.w != null ? 1 : 0
            //            } into tags
            //            select new
            //            {
            //                id = tags.Key.i,
            //                count = tags.Sum(c => c.C}

            var query = _tagsManager.GetAllTagsRelations(Session.OrganizationId.Value)
                .GroupBy(t => t.Tag)
                .Select(t => new TagListDto()
                {
                    Id = t.Key.Id,
                    Name = t.Key.Name,
                    FullName = t.Key.FullName,
                    CreatedAt = t.Key.CreatedAt,
                    Category = t.Key.Category,
                    ContactsCount = t.Where(u => u.ContactId != null).GroupBy(g => g.ContactId).Count(),
                    LibraryItemsCount = t.Where(u => u.LibraryItemId != null).GroupBy(g => g.LibraryItemId).Count(),
                    ConversationsCount = t.Where(u => u.ConversationId != null).GroupBy(g => g.ConversationId).Count()
                })
                .Union(_tagsManager.GetAllTags(Session.OrganizationId.Value)
                    .Where(t => !t.TagsRelations.Any())
                    .Select(t => new TagListDto()
                    {
                        Id = t.Id,
                        Name = t.Name,
                        FullName = t.FullName,
                        CreatedAt = t.CreatedAt,
                        Category = t.Category,
                        ContactsCount = 0,
                        LibraryItemsCount = 0
                    })
                );

            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                query = query.Where(t => EF.Functions.Like("name", input.Filter) || EF.Functions.Like("category", input.Filter));
            }

            if (!string.IsNullOrWhiteSpace(input.Sorting))
            {
                query = query.OrderBy(input.Sorting);
            }

            var totalCount = await query.CountAsync();
            var hasNext = (input.SkipCount + input.MaxResultCount) < totalCount;

            var result = await query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToListAsync();

            return new ObjectResult(
                new PagedResultDto<TagListDto>(
                    totalCount,
                    result,
                    hasNext
                )
            );
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        public async Task<IActionResult> GetAllCategories()
        {
            EnsureOrganization();

            var result = await _tagsManager.GetAllCategories(Session.OrganizationId.Value).ToListAsync();
            return new ObjectResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            EnsureOrganization();

            var tag = await _tagsManager.FindByIdAsync(id, Session.OrganizationId.Value);
            await _tagsManager.DeleteTagAsync(tag);

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(typeof(TagDto), 200)]
        public async Task<IActionResult> CreateTag([FromBody]TagDto input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EnsureOrganization();

            var result = new Tag(
                input.Name,
                Session.OrganizationId.Value,
                input.Category
            );

            result = await _tagsManager.CreateTagAsync(result);

            return new ObjectResult(Mapper.Map<Tag, TagDto>(result));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TagDto), 200)]
        public async Task<IActionResult> UpdateTag(int id, [FromBody]TagDto input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EnsureOrganization();

            var tag = await _tagsManager.FindByIdAsync(id, Session.OrganizationId.Value);
            tag.Update(
                input.Name,
                input.Category
            );

            tag = await _tagsManager.UpdateTagAsync(tag, Session.OrganizationId.Value);

            return new ObjectResult(Mapper.Map<Tag, TagDto>(tag));
        }

    }
}
