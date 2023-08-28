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
using AskPam.Crm.Posts.Dtos;
using AskPam.Crm.Tags;

namespace AskPam.Crm.Posts
{
    [Authorize]
    [Route("api/[controller]")]
    public class PostsController : BaseController
    {
        private IRepository<Post> _postRepository;
        private IPostManager _postManager;
        private ITagsManager _tagManager;


        public PostsController(
            ICrmSession session,
            IMapper mapper,
            IRepository<Post> postRepository,
            IPostManager postManager,
            ITagsManager tagManager
        ) : base(session, mapper)
        {
            _postRepository = postRepository;
            _postManager = postManager;
            _tagManager = tagManager;
        }


        [HttpPost]
        [ProducesResponseType(typeof(PostDto), 200)]
        public async Task<IActionResult> CreatePost([FromBody]PostDto input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EnsureOrganization();


            var post = new Post(
                Session.OrganizationId.Value,
                input.Title,
                input.Description
            );

            post = await _postManager.CreatePost(post);


            if (input.Tags != null && input.Tags.Count() > 0)
                post.Tags = input.Tags.Select(t => new TagsRelation(t.Id, postId: post.Id)).ToList();

            return new ObjectResult(Mapper.Map<Post, PostDto>(post));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PostDto), 200)]
        public async Task<IActionResult> UpdatePost(long id, [FromBody]PostDto input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EnsureOrganization();

            var post = await _postManager.FindPostById(id, Session.OrganizationId.Value);

            post.Update(
                input.Title,
                input.Description
            );

            var tags = post.Tags;

            post = await _postManager.UpdatePost(post);

            if (input.Tags != null)
            {
                foreach (var tag in tags)
                {
                    if (!input.Tags.Any(t => t.Id == tag.TagId))
                        await _tagManager.Untag(tag.TagId, postId: input.Id);
                }

                if (input.Tags.Count() > 0)
                {

                    await _tagManager.Tag(input.Tags.Select(t => t.Id).ToList(), postId: input.Id);
                }
            }

            return new ObjectResult(Mapper.Map<Post, PostDto>(post));
        }

        [HttpPost("[action]")]
        [ProducesResponseType(typeof(PagedResultDto<PostDto>), 200)]
        public async Task<IActionResult> GetPosts([FromBody]PostListRequestDto input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EnsureOrganization();

            var query = _postRepository.GetAll()
                .Include(p => p.CreatedUser)
                .Include(p => p.Notes)
                 .Include(c => c.Tags)
                .ThenInclude(t => t.Tag)
                .Where(c => c.IsDeleted == false)
                .Where(c => c.OrganizationId == Session.OrganizationId);

            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                query = query.Where(l => l.Title.ToLower()
                    .Contains(input.Filter.ToLower())
                );
            }
            

            //if (!string.IsNullOrWhiteSpace(input.Sorting))
            //{
            //    query = query.OrderBy(input.Sorting);
            //}



            var totalCount = await query.CountAsync();
            var hasNext = (input.SkipCount + input.MaxResultCount) < totalCount;

            var posts = await query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .OrderByDescending(p => p.CreatedAt)
                .Select(t => Mapper.Map<Post, PostDto>(t))
                .ToListAsync();

            return new ObjectResult(
                new PagedResultDto<PostDto>(
                    totalCount,
                    posts,
                    hasNext
                )
            );
        }



        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PostDto), 200)]
        public async Task<IActionResult> GetPost(int id)
        {
            EnsureOrganization();

            var post = await _postManager.FindPostById(id, Session.OrganizationId.Value);

            return new ObjectResult(Mapper.Map<Post, PostDto>(post));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> DeletePost(int id)
        {
            EnsureOrganization();

            var post = await _postManager.FindPostById(id, Session.OrganizationId.Value);
            await _postManager.DeletePost(post);

            return Ok();
        }

    }
}
