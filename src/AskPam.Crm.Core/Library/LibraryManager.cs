using AskPam.Exceptions;
using AskPam.Events;
using AskPam.Crm.Library.Events;
using AskPam.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using AskPam.Crm.Common;
using AskPam.EntityFramework.Repositories;

namespace AskPam.Crm.Library
{
    public class LibraryManager : ILibraryManager
    {
        private readonly IRepository<LibraryItem> _libraryRepository;
        private readonly IPhoneNumberLookupService _phoneNumberLookupService;
        private readonly IDomainEvents _domainEvents;
        private readonly IUnitOfWork _unitOfWork;

        public LibraryManager(
            IRepository<LibraryItem> libraryRepository, 
            IPhoneNumberLookupService phoneNumberLookupService,
            IDomainEvents domainEvents, IUnitOfWork unitOfWork)
        {
            _libraryRepository = libraryRepository;
            _phoneNumberLookupService = phoneNumberLookupService;
            _domainEvents = domainEvents;
            _unitOfWork = unitOfWork;
        }

        public async Task<LibraryItem> CreateLibraryItem(LibraryItem item)
        {
            //if (!string.IsNullOrEmpty(item.GooglePlaceId))
            //{
            //    var googlePlaceid = item.GooglePlaceId;

            //    var existingItems = _libraryRepository.FirstOrDefaultasync(c => c.GooglePlaceId == googlePlaceid && c.TenantId == AbpSession.TenantId);

            //    if (existingItems != null)
            //        throw new UserFriendlyException("This item already exists in your library ");
            //}         

            item = await ValidatePhoneNumber(item);
            item = await _libraryRepository.InsertAsync(item);
            await _unitOfWork.SaveChangesAsync();

            await _domainEvents.RaiseAsync(new LibraryCreatedEvent
                {
                    LibraryItem = item
                }
            );

            return item;
        }

        public async Task<LibraryItem> UpdateLibraryItem(LibraryItem item)
        {
            item = await ValidatePhoneNumber(item);
            item = await _libraryRepository.UpdateAsync(item);
            await _unitOfWork.SaveChangesAsync();
            //await EventBus.TriggerAsync(new LibraryItemUpdatedEventData() { Item = item });
            return item;
        }

        public async Task<LibraryItem> FindLibraryItemById(long id, Guid organizationId)
        {
            var result = await GetAllLibraryItems(organizationId)
                .Include(c => c.TagsRelations)
                .ThenInclude(t => t.Tag)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (result == null)
            {
                throw new ApiException("Library not found, maybe it was deleted.", HttpStatusCode.NotFound);
            }

            return result;
        }

        public async Task DeleteLibraryItem(LibraryItem item)
        {
            await _libraryRepository.DeleteAsync(item);
            await _unitOfWork.SaveChangesAsync();
        }

        public IQueryable<LibraryItem> GetAllLibraryItems(Guid organizationId)
        {
            return _libraryRepository.GetAll()
                .Where(n => n.OrganizationId == organizationId);
        }

        #region Private
        private async Task<LibraryItem> ValidatePhoneNumber(LibraryItem libraryItem)
        {
            if (!string.IsNullOrEmpty(libraryItem.Phone))
            {
                var phone = await _phoneNumberLookupService.Format(libraryItem.Phone);
                if (phone.Number == null)
                {
                    throw new ApiException("Phone is not valid", System.Net.HttpStatusCode.BadRequest);
                }
                libraryItem.SetPhoneCountryCode(phone.CountryCode);
                libraryItem.SetNationalPhone(phone.NationalFormat);
            }

            return libraryItem;
        }
        #endregion
    }
}
