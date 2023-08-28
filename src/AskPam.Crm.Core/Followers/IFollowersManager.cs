using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AskPam.Crm.Authorization;
using AskPam.Crm.Authorization.Followers;
using AskPam.Crm.Conversations;
using AskPam.Domain.Services;

namespace AskPam.Crm.Followers
{
    public interface IFollowersManager: IDomainService
    {
        Task<FollowersRelation> Follow(User user, Conversation conversation, bool notify = true);
        IQueryable<FollowersRelation> GetFollowers();
        Task<IEnumerable<User>> GetFollowers(long conversationId, Guid organizationId);
        Task<bool> IsFollowing(User user, Conversation conversation);
        Task Unfollow(User user, long conversationId);
    }
}