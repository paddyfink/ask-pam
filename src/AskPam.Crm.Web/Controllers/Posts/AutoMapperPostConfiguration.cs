using AskPam.Crm.Controllers.Libraries.Dtos;
using AskPam.Crm.Library;
using AskPam.Crm.Library.Dtos;
using AskPam.Crm.Posts.Dtos;
using AutoMapper;

namespace AskPam.Crm.Posts
{
    public class PostAutomapperProfile : Profile
    {
        public PostAutomapperProfile()
        {
            CreateMap<Post, PostDto>()
                .ForMember(
                    dest => dest.Tags,
                    opts => opts.MapFrom(src => src.Tags)
                )
                .ForMember(
                    dest => dest.CreatedAt,
                    opts => opts.MapFrom(src => src.CreatedAt.Value.ToLocalTime())
                );
        }
    }
}
