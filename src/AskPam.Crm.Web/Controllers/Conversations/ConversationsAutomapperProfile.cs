using AskPam.Crm.Controllers.Conversations.Dtos;
using AskPam.Crm.Conversations.Dtos;
using AutoMapper;
using System.Linq;

namespace AskPam.Crm.Conversations
{
    public class ConversationsAutomapperProfile : Profile
    {
        public ConversationsAutomapperProfile()
        {
            string userId = null;

            CreateMap<Channel, ChannelDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.TypeId));

            CreateMap<DeliveryStatus, DeliveryStatusDto>()
                .ForMember(
                    dest => dest.ChannelType,
                    opt => opt.MapFrom(src => src.ChannelTypeId)
                );

            CreateMap<Message, MessageDto>()
               .Include<Email, EmailDto>()
                .ForMember(
                    dest => dest.Tags,
                    opts => opts.MapFrom(src => src.TagsRelations)
                )
               .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.MessageTypeId))
               .ForMember(dest => dest.ChannelType, opt => opt.MapFrom(src => src.ChannelTypeId))
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.MessageStatusId))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToLocalTime()));

            CreateMap<Email, EmailDto>()
               .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToLocalTime()));

            CreateMap<Conversation, ConversationDto>()
                .ForMember(dest => dest.Messages, opt => opt.Ignore())
                .ForMember(
                    dest => dest.Tags,
                    opts => opts.MapFrom(src => src.TagsRelations)
                )
                //.ForMember(dest => dest.IsStarred, opts => opts.MapFrom(src => src.StarRelations.Any(s => s.UserId == userId))); 
                .ForMember(dest => dest.IsStarred, opts => opts.Ignore());

            CreateMap<Attachment, AttachmentDto>()
                //Content in DTO is used when we send a message with attachment.
                .ForMember(dest => dest.Content, opt => opt.Ignore());

            CreateMap<Conversation, ConversationListDto>()
                .ForMember(
                    dest => dest.Tags,
                    opts => opts.MapFrom(src => src.TagsRelations)
                )
                .ForMember(dest => dest.LastMessage, opt => opt.MapFrom(r =>
                    r.Messages
                        .OrderByDescending(m => m.Date)
                        .Select(x => new MessageDto
                        {
                            Text = x.Text,
                            Date = x.Date.ToLocalTime(),
                            Author = x.Author,
                            AuthorId = x.AuthorId,
                            Avatar = x.Avatar
                        })
                        .FirstOrDefault()
                        ))
                 .ForMember(dest => dest.IsStarred, opts => opts.MapFrom(src => src.StarRelations.Any(s => s.UserId == userId)));
        }
    }
}
