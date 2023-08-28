using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using System.Linq;
using AutoMapper;
using AskPam.Domain.Repositories;
using AskPam.Crm.Runtime.Session;
using AskPam.Application.Dto;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using AskPam.Crm.Controllers.Libraries.Dtos;
using System;
using AskPam.Crm.Library.Dtos;
using AskPam.Crm.Tags;
using AskPam.Crm.Common;
using AskPam.Extensions;

namespace AskPam.Crm.Library
{
    [Authorize]
    [Route("api/[controller]")]
    public class LibraryController : BaseController
    {
        private readonly IRepository<LibraryItem> _libraryItemRepository;
        private readonly ILibraryManager _libraryManager;
        private readonly ITagsManager _tagManager;

        //private IPhoneNumberLookupService _phoneNumberLookupService;

        public LibraryController(
            ICrmSession session,
            IMapper mapper,
            IRepository<LibraryItem> libraryItemRepository,
            ILibraryManager libraryManager,
            ITagsManager tagManager
        ) : base(session, mapper)
        {
            _libraryItemRepository = libraryItemRepository;
            _libraryManager = libraryManager;
            _tagManager = tagManager;
        }

        #region LibraryItem
        [HttpPost]
        [ProducesResponseType(typeof(LibraryItemDto), 200)]
        public async Task<IActionResult> CreateLibraryItem([FromBody]LibraryItemDto input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EnsureOrganization();

            LibraryItemType? type = null;
            if (input.TypeValue.HasValue && input.TypeValue.Value > 0)
            {
                type = (LibraryItemType)input.TypeValue.Value;
            }

            var libraryItem = new LibraryItem(
                input.Name,
                Session.OrganizationId.Value,
                input.Email,
                input.Phone,
                input.Description,
                input.OpeningHours,
                input.Menu,
                input.Area,
                input.Price,
                input.Website,
                input.Fax,
                input.Address1,
                input.Address2,
                input.PostalCode,
                input.City,
                input.Province,
                input.Country,
                type,
                input.StartDate,
                input.EndDate,
                input.IsAllDay,
                subject: input.Subject
            );

            if (input.Tags != null && input.Tags.Count() > 0)
                libraryItem.TagsRelations = input.Tags.Select(t => new TagsRelation(t.Id, libraryItemId: libraryItem.Id)).ToList();

            libraryItem = await _libraryManager.CreateLibraryItem(libraryItem);

            return new ObjectResult(Mapper.Map<LibraryItem, LibraryItemDto>(libraryItem));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(LibraryItemDto), 200)]
        public async Task<IActionResult> UpdateLibraryItem(long id,[FromBody]LibraryItemDto input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EnsureOrganization();

            LibraryItemType? type = null;
            if (input.TypeValue.HasValue && input.TypeValue.Value > 0)
            {
                type = (LibraryItemType)input.TypeValue.Value;
            }

            var libraryItem = await _libraryManager.FindLibraryItemById(input.Id.Value, Session.OrganizationId.Value);

            libraryItem.Update(
                input.Name,
                input.Email,
                input.Phone,
                input.Description,
                input.OpeningHours,
                input.Menu,
                input.Area,
                input.Price,
                input.Website,
                input.Fax,
                input.Address1,
                input.Address2,
                input.PostalCode,
                input.City,
                input.Province,
                input.Country,
                type,
                input.StartDate,
                input.EndDate,
                input.IsAllDay,
                subject:input.Subject
            );

            var tags = libraryItem.TagsRelations;

            libraryItem = await _libraryManager.UpdateLibraryItem(libraryItem);

            if (input.Tags != null)
            {
                foreach (var tag in tags)
                {
                    if (!input.Tags.Any(t => t.Id == tag.TagId))
                        await _tagManager.Untag(tag.TagId, libraryItemId: libraryItem.Id);
                }

                if (input.Tags.Any())
                {

                    await _tagManager.Tag(input.Tags.Select(t => t.Id).ToList(), libraryItemId: libraryItem.Id);
                }
            }

            return new ObjectResult(Mapper.Map<LibraryItem, LibraryItemDto>(libraryItem));
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(PagedResultDto<LibraryItemListDto>), 200)]
        public async Task<IActionResult> GetLibraryItems([FromBody]LibraryItemListRequestDto input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EnsureOrganization();

            var query = _libraryItemRepository.GetAll()
                .Include(c => c.TagsRelations)
                .ThenInclude(t => t.Tag)
                .Where(c => c.IsDeleted == false)
                .Where(c => c.OrganizationId == Session.OrganizationId);

            var tags = new List<long>().ToArray();

            if (!input.Filter.IsNullOrEmpty())
            {
                tags = (await _tagManager.Search(input.Filter, Session.OrganizationId.Value)).Select(t => t.Id)
                    .ToArray();
            }

            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                query = query.Where(l =>
                    EF.Functions.Like(l.Name, $"%{input.Filter}%")
                    || l.TagsRelations.Any(tr => tags.Contains(tr.TagId)));
            }

            if (!string.IsNullOrWhiteSpace(input.Sorting))
            {
                query = query.OrderBy(input.Sorting);
            }

            if (input.LibraryTypeId.HasValue)
            {
                query = query.Where(l => l.Type == (LibraryItemType)input.LibraryTypeId.Value);
            }

            var totalCount = await query.CountAsync();
            var hasNext = (input.SkipCount + input.MaxResultCount) < totalCount;

            var libraryItems = await query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .Select(t => Mapper.Map<LibraryItem, LibraryItemListDto>(t))
                .ToListAsync();

            return new ObjectResult(
                new PagedResultDto<LibraryItemListDto>(
                    totalCount,
                    libraryItems,
                    hasNext
                )
            );
        }
                
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LibraryItemDto), 200)]
        public async Task<IActionResult> GetLibraryItem(int id)
        {
            EnsureOrganization();

            var libraryItem = await _libraryManager.FindLibraryItemById(id, Session.OrganizationId.Value);

            return new ObjectResult(Mapper.Map<LibraryItem, LibraryItemDto>(libraryItem));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> DeleteLibraryItem(int id)
        {
            EnsureOrganization();

            var libraryItem = await _libraryManager.FindLibraryItemById(id, Session.OrganizationId.Value);
            await _libraryManager.DeleteLibraryItem(libraryItem);

            return Ok();
        }
        #endregion

        #region LibraryItemType
        [HttpGet("types")]
        [ProducesResponseType(typeof(IEnumerable<LibraryItemTypeDto>), 200)]
        public IActionResult GetLibraryItemTypes()
        {
            var result = Enum.GetValues(typeof(LibraryItemType))
                .Cast<LibraryItemType>()
                .Select(i => new LibraryItemTypeDto()
                {
                    Id = (int)Convert.ChangeType(i, i.GetType()),
                    Name = i.ToString()
                })
                .OrderBy(t => t.Name);

            return new ObjectResult(result);
        }
        #endregion

        #region Private

        private Phone GetPhone(string number)
        {
            return new Phone(); //Need to implement PhoneLookupService
            //return string.IsNullOrEmpty(number)
            //    ? new Phone()
            //    : _phoneNumberLookupService.Format(number);
        }
        #endregion
    }
}
