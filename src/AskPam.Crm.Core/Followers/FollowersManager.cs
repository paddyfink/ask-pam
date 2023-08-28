using AskPam.Crm.Authorization;
using AskPam.Crm.Authorization.Followers;
using AskPam.Crm.Conversations;
using AskPam.Crm.Conversations.Events;
using AskPam.Events;
using AskPam.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AskPam.EntityFramework.Repositories;

namespace AskPam.Crm.Followers
{
    public class FollowersManager : IFollowersManager
    {
        private readonly IRepository<FollowersRelation> _followersRelationRepository;
        private readonly IUserManager _userManager;
        private readonly IDomainEvents _domainEvents;
        private readonly IUnitOfWork _unitOfWork;

        public FollowersManager(
            IRepository<FollowersRelation> followersRelationRepository,
            IUserManager userManager,
            IDomainEvents domainEvents, IUnitOfWork unitOfWork)
        {
            _followersRelationRepository = followersRelationRepository;
            _userManager = userManager;
            _domainEvents = domainEvents;
            _unitOfWork = unitOfWork;
        }


        public async Task<bool> IsFollowing(User user, Conversation conversation)
        {
            return await _followersRelationRepository.GetAll()
                .Where(f => f.UserId == user.Id)
                .Where(f => f.ConversationId == conversation.Id)
                .AnyAsync();
        }

        public async Task<FollowersRelation> Follow(User user, Conversation conversation, bool notify = true)
        {
            var result = await _followersRelationRepository.InsertAsync(
                new FollowersRelation(
                    user.Id,
                    conversation.Id
                )
            );
            await _unitOfWork.SaveChangesAsync();

            if (notify)
                await _domainEvents.RaiseAsync(new ConversationFollowedEvent
                {
                    Conversation = conversation,
                    FollowersRelation = result
                }
                );

            return result;
        }

        public async Task Unfollow(User user, long conversationId)
        {
            var itemToDelete = await _followersRelationRepository.GetAll()
                .Where(i => i.UserId == user.Id)
                .Where(i => i.ConversationId == conversationId)
                .FirstOrDefaultAsync();

            if (itemToDelete != null)
            {
                await _followersRelationRepository.DeleteAsync(itemToDelete);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<User>> GetFollowers(long conversationId, Guid organizationId)
        {
            var result = await _followersRelationRepository.GetAll()
                .Where(f => f.ConversationId == conversationId)
                .Join(_userManager.GetAllUsers(organizationId),
                    f => f.UserId,
                    u => u.Id,
                    (f, u) => u
                )
                .ToListAsync();

            return result;
        }

        public IQueryable<FollowersRelation> GetFollowers()
        {
            var result = _followersRelationRepository.GetAll();

            return result;
        }


    }
}
