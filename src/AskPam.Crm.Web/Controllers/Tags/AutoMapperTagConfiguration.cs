using AutoMapper;
using AskPam.Crm.Tags;
using AskPam.Crm.Tags.Dtos;

namespace AskPam.Crm.Authorization.Organizations
{
    public class TagsAutomapperProfile : Profile
    {
        public TagsAutomapperProfile()
        {
            CreateMap<Tag, TagDto>();
            CreateMap<TagsRelation, TagDto>()
                .ForMember(
                    dest => dest.Id,
                    opts => opts.MapFrom(src => src.TagId)
                )
                .ForMember(
                    dest => dest.Name,
                    opts => opts.MapFrom(src => src.Tag.Name)
                )
                .ForMember(
                    dest => dest.Category,
                    opts => opts.MapFrom(src => src.Tag.Category)
                );
        }

    }
}
