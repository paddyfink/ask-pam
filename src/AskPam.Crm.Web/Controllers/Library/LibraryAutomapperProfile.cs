using AskPam.Crm.Controllers.Libraries.Dtos;
using AskPam.Crm.Library.Dtos;
using AutoMapper;

namespace AskPam.Crm.Library
{
    public class LibraryAutomapperProfile : Profile
    {
        public LibraryAutomapperProfile()
        {
            CreateMap<LibraryItem, LibraryItemDto>()
                .ForMember(
                    dest => dest.TypeValue,
                    opts => opts.MapFrom(src => src.Type.HasValue
                        ? (int?)src.Type.Value
                        : null)
                )
                 .ForMember(
                    dest => dest.Tags,
                    opts => opts.MapFrom(src => src.TagsRelations)
                );

            CreateMap<LibraryItem, LibraryItemListDto>()
                 .ForMember(
                    dest => dest.Tags,
                    opts => opts.MapFrom(src => src.TagsRelations)
                );
        }
    }
}
