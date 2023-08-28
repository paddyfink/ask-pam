using AskPam.Exceptions;
using AskPam.Events;
using AskPam.Crm.Posts.Events;
using AskPam.Domain.Repositories;
using AskPam.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using AskPam.EntityFramework.Repositories;

namespace AskPam.Crm.Posts
{
    public class PostManager : IPostManager
    {
        private readonly IRepository<Post> _postRepository;
        private readonly IDomainEvents _domainEvents;
        private readonly IUnitOfWork _unitOfWork;

        public PostManager(
            IRepository<Post> postRepository,
            IDomainEvents domainEvents, IUnitOfWork unitOfWork)
        {
            _postRepository = postRepository;
            _domainEvents = domainEvents;
            _unitOfWork = unitOfWork;
        }

        public async Task<Post> CreatePost(Post post)
        {
            post = await _postRepository.InsertAsync(post);
            await _unitOfWork.SaveChangesAsync();

            await _domainEvents.RaiseAsync(new PostCreated { Post = post });

            return post;
        }

        public async Task<Post> UpdatePost(Post post)
        {
            post = await _postRepository.UpdateAsync(post);
            await _unitOfWork.SaveChangesAsync();
            await _domainEvents.RaiseAsync(new PostUpdated { Post = post });
            return post;
        }

        public async Task<Post> FindPostById(long id, Guid organizationId)
        {
            var result = await _postRepository.GetAll()
                .Include(p => p.CreatedUser)
                .Include(c => c.Tags)
                .ThenInclude(t => t.Tag)
                .Where(n => n.OrganizationId == organizationId)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (result == null)
            {
                throw new ApiException("Post not found, maybe it was deleted.", HttpStatusCode.NotFound);
            }

            return result;
        }

        public async Task DeletePost(Post post)
        {
            await _postRepository.DeleteAsync(post);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
