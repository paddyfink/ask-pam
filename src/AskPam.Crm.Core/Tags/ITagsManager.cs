using AskPam.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskPam.Crm.Tags
{
    public interface ITagsManager : IDomainService
    {
        Task<Tag> CreateTagAsync(Tag tag);
        Task<Tag> DeleteTagAsync(Tag tag);
        Task<Tag> FindByIdAsync(long id, Guid organizationId);
        Task<Tag> FindTagByName(string name, Guid organizationId);
        IQueryable<string> GetAllCategories(Guid organizationId);
        IQueryable<Tag> GetAllTags(Guid organizationId);
        IQueryable<TagsRelation> GetAllTagsRelations(Guid organizationId);
        IQueryable<Tag> GetItemTags(Guid organizationId, long? contactId = null, long? libraryItemId = null, long? postId = null, long? messageId = null);
        Task<IEnumerable<Tag>> Search(string name, Guid organizationId);
        Task Tag(IEnumerable<long> tags, long? contactId = null, long? libraryItemId = null, long? conversationId = null, long? postId = null, long? messageId = null);
        Task Tag(Tag tag, long? contactId = null, long? libraryItemId = null, long? conversationId = null, long? messageId = null);
        Task Untag(long tagId, long? contactId = null, long? libraryItemId = null, long? postId = null, long? conversationId = null, long? messageId = null);
        Task<Tag> UpdateTagAsync(Tag tag, Guid organizationId);
    }
}