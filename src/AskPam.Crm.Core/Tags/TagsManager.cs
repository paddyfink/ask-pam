using AskPam.Exceptions;
using AskPam.Domain.Repositories;
using AskPam.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AskPam.EntityFramework.Repositories;
using System.Net;

namespace AskPam.Crm.Tags
{
    public class TagsManager : ITagsManager
    {
        private readonly IRepository<TagsRelation> _tagsRelationRepository;
        private readonly IRepository<Tag> _tagRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TagsManager(IUnitOfWork unitOfWork, IRepository<Tag> tagRepository, IRepository<TagsRelation> tagsRelationRepository)
        {
            _unitOfWork = unitOfWork;
            _tagRepository = tagRepository;
            _tagsRelationRepository = tagsRelationRepository;
        }

        #region Tags

        public async Task<Tag> CreateTagAsync(Tag tag)
        {
            await TagNameValidation(tag, tag.OrganizationId);

            tag = await _tagRepository.InsertAsync(tag);
            await _unitOfWork.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag> UpdateTagAsync(Tag tag, Guid organizationId)
        {
            await TagNameValidation(tag, organizationId);

            tag = await _tagRepository.UpdateAsync(tag);
            await _unitOfWork.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag> DeleteTagAsync(Tag tag)
        {
            await _tagRepository.DeleteAsync(tag);
            await _unitOfWork.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag> FindByIdAsync(long id, Guid organizationId)
        {
            var result = await GetAllTags(organizationId)
                .FirstAsync(t => t.Id == id);

            return result;
        }

        public IQueryable<Tag> GetAllTags(Guid organizationId)
        {
            return _tagRepository.GetAll()
                .Where(n => n.OrganizationId == organizationId);
        }

        public async Task<Tag> FindTagByName(string name, Guid organizationId)
        {
            var tag = await GetAllTags(organizationId)
                .Where(t => t.Name == name)
                .FirstOrDefaultAsync();

            return tag;
        }

        public async Task<IEnumerable<Tag>> Search(string name, Guid organizationId)
        {
            var tags = await GetAllTags(organizationId)
                .Where(t => t.Name.ToLower().Contains(name.ToLower()))
                .ToListAsync();

            return tags;
        }

        public IQueryable<string> GetAllCategories(Guid organizationId)
        {
            return GetAllTags(organizationId)
                .GroupBy(t => t.Category)
                .Select(t => t.Key);
        }

        #endregion

        #region TagsRelation
        public async Task Tag(Tag tag, long? contactId = null, long? libraryItemId = null, long? conversationId = null, long? messageId = null)
        {
            if (await _tagsRelationRepository.GetAll().AnyAsync(t =>
                t.TagId == tag.Id && t.ConversationId == conversationId && t.ContactId == contactId &&
                t.MessageId == messageId && t.LibraryItemId == libraryItemId))
            {
                return;
            }

            if (contactId.HasValue)
            {
                await _tagsRelationRepository.InsertAsync(
                    new TagsRelation(
                        tag.Id,
                        contactId: contactId
                    )
                );
            }

            if (libraryItemId.HasValue)
            {
                await _tagsRelationRepository.InsertAsync(
                    new TagsRelation(
                        tag.Id,
                        libraryItemId: libraryItemId
                    )
                );
            }

            if (conversationId.HasValue)
            {
                await _tagsRelationRepository.InsertAsync(
                    new TagsRelation(
                        tag.Id,
                        conversationId: conversationId
                    )
                );
            }

            if (messageId.HasValue)
            {
                await _tagsRelationRepository.InsertAsync(
                    new TagsRelation(
                        tag.Id,
                        messageId: messageId
                    )
                );
            }
            await _unitOfWork.SaveChangesAsync();

        }

        public async Task Tag(IEnumerable<long> tags, long? contactId = null, long? libraryItemId = null, long? conversationId = null, long? postId = null, long? messageId = null)
        {
            foreach (var tagId in tags)
            {

                if (await _tagsRelationRepository.GetAll().AnyAsync(t =>
                t.TagId == tagId && t.ConversationId == conversationId && t.ContactId == contactId &&
                t.MessageId == messageId && t.LibraryItemId == libraryItemId))
                {
                    continue;
                }

                if (contactId.HasValue)
                {

                    if (!_tagsRelationRepository.GetAll().Any(t => t.ContactId == contactId.Value && t.TagId == tagId))
                        await _tagsRelationRepository.InsertAsync(
                            new TagsRelation(
                                tagId,
                                contactId: contactId
                            )
                        );
                }

                if (libraryItemId.HasValue)
                {
                        await _tagsRelationRepository.InsertAsync(
                            new TagsRelation(
                                tagId,
                                libraryItemId: libraryItemId
                            )
                        );
                }

                if (conversationId.HasValue)
                {
                        await _tagsRelationRepository.InsertAsync(
                            new TagsRelation(
                                tagId,
                                conversationId: conversationId
                            )
                        );
                }

                if (postId.HasValue)
                {
                        await _tagsRelationRepository.InsertAsync(
                            new TagsRelation(
                                tagId,
                                postId: postId
                            )
                        );
                }

                if (messageId.HasValue)
                {
                        await _tagsRelationRepository.InsertAsync(
                            new TagsRelation(
                                tagId,
                                postId: postId
                            )
                        );
                }

                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task Untag(long tagId, long? contactId = null, long? libraryItemId = null, long? postId = null, long? conversationId = null, long? messageId = null)
        {


            if (contactId.HasValue)
            {
                var itemToDelete = await _tagsRelationRepository.GetAll()
                .Where(i => i.TagId == tagId && i.ContactId == contactId)
                .FirstAsync();
                await _tagsRelationRepository.DeleteAsync(itemToDelete);
            }

            if (libraryItemId.HasValue)
            {
                var itemToDelete = await _tagsRelationRepository.GetAll()
                .Where(i => i.TagId == tagId && i.LibraryItemId == libraryItemId)
                .FirstAsync();
                await _tagsRelationRepository.DeleteAsync(itemToDelete);
            }

            if (postId.HasValue)
            {
                await _tagsRelationRepository.InsertAsync(
                    new TagsRelation(
                        tagId,
                        postId: postId
                    )
                );
            }

            if (conversationId.HasValue)
            {
                var itemToDelete = await _tagsRelationRepository.GetAll()
                .Where(i => i.TagId == tagId && i.ConversationId == conversationId)
                .FirstAsync();
                await _tagsRelationRepository.DeleteAsync(itemToDelete);
            }

            if (messageId.HasValue)
            {
                var itemToDelete = await _tagsRelationRepository.GetAll()
                    .Where(i => i.TagId == tagId && i.MessageId == messageId)
                    .FirstAsync();
                await _tagsRelationRepository.DeleteAsync(itemToDelete);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public IQueryable<Tag> GetItemTags(Guid organizationId, long? contactId = null, long? libraryItemId = null, long? postId = null, long? messageId = null)
        {

            var query = _tagsRelationRepository.GetAll();

            if (contactId.HasValue)
            {
                query = query.Where(t => t.ContactId == contactId);
            }

            if (libraryItemId.HasValue)
            {
                query = query.Where(t => t.LibraryItemId == libraryItemId);
            }

            if (postId.HasValue)
            {
                query = query.Where(t => t.PostId == postId);
            }

            if (messageId.HasValue)
            {
                query = query.Where(t => t.MessageId == messageId);
            }

            var result = query.Join(GetAllTags(organizationId),
                   r => r.TagId,
                   t => t.Id,
                   (f, u) => u
               );

            return result;
        }

        public IQueryable<TagsRelation> GetAllTagsRelations(Guid organizationId)
        {
            return _tagsRelationRepository.GetAll()
                .Where(t => t.Tag.OrganizationId == organizationId);
        }

        #region Private

        private async Task TagNameValidation(Tag tag, Guid organisationId)
        {
            var query = GetAllTags(organisationId)
                .Where(g => g.Name == tag.Name);

            if (tag.Id != 0)
            { //If update
                query = query.Where(g => g.Id != tag.Id);
            }

            var existingGroup = await query
                .Where(g => g.Name.Equals(
                        tag.Name,
                        StringComparison.CurrentCultureIgnoreCase
                    )
                )
                .AnyAsync();

            if (existingGroup)
                throw new ApiException("There is already a tag with that name", HttpStatusCode.BadRequest);
        }
        #endregion
        #endregion

    }
}
