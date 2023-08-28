using AskPam.Domain.Services;
using System;
using System.Threading.Tasks;

namespace AskPam.Crm.Posts
{
    public interface IPostManager: IDomainService
    {
        Task<Post> CreatePost(Post post);
        Task DeletePost(Post post);
        Task<Post> FindPostById(long id, Guid organizationId);
        Task<Post> UpdatePost(Post post);


    }
}