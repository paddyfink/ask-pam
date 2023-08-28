using AskPam.Domain.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.Library
{
    public interface ILibraryManager: IDomainService
    {
        Task<LibraryItem> CreateLibraryItem(LibraryItem item);
        Task DeleteLibraryItem(LibraryItem item);
        Task<LibraryItem> FindLibraryItemById(long id, Guid organizationId);
        IQueryable<LibraryItem> GetAllLibraryItems(Guid organizationId);
        Task<LibraryItem> UpdateLibraryItem(LibraryItem item);
    }
}