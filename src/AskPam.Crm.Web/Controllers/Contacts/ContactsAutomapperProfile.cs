using AskPam.Crm.Contacts;
using AskPam.Crm.Contacts.Dtos;
using AutoMapper;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace AskPam.Crm.Contacts
{
    public class ContactsAutomapperProfile : Profile
    {
        public ContactsAutomapperProfile()
        {
            //Contact
            CreateMap<Contact, ContactDto>()

                //.ForMember(
                //    dest => dest.PublicInfo,
                //    opts => opts.MapFrom(src => src.PublicInfoId.HasValue
                //        ? JObject.Parse(src.PublicInfo.Data)
                //        : null
                //    )
                //)
                .ForMember(
                    dest => dest.Data,
                    opt => opt.Ignore()
                )
                .ForMember(
                    dest => dest.CustomFields,
                    opts => opts.MapFrom(src => string.IsNullOrEmpty(src.CustomFields)
                        ? null
                        : JObject.Parse(src.CustomFields)
                    )
                )
                .ForMember(
                    dest => dest.PublicInfo,
                    opt => opt.Ignore()
                )
                .ForMember(
                    dest => dest.IsNew,
                    opts => opts.MapFrom(src => src.CreatedAt >= DateTime.UtcNow.AddDays(-1))
                )
                .ForMember(
                    dest => dest.Tags,
                    opts => opts.MapFrom(src => src.TagsRelations)
                )
                .ForMember(
                    dest => dest.Gender,
                    opts => opts.MapFrom(src => src.Gender.Value)
                )
                .ForMember(
                    dest => dest.MaritalStatus,
                    opts => opts.MapFrom(src => src.MaritalStatus.Value)
                )
                .ForMember(
                    dest => dest.MobilePhone,
                    opts => opts.MapFrom(src => src.MobilePhone.Number)
                )
                .ForMember(
                    dest => dest.MobilePhoneDisplay,
                    opts => opts.MapFrom(src => src.MobilePhone.NationalFormat)
                );

            CreateMap<Contact, ContactListDto>()
                .ForMember(
                    dest => dest.Tags,
                    opts => opts.MapFrom(src => src.TagsRelations)
                )
                .ForMember(
                    dest => dest.IsNew,
                    opts => opts.MapFrom(src => src.CreatedAt >= DateTime.UtcNow.AddDays(-1))
                );


            CreateMap<Contact, SimpleContactDto>();

            //Contact Group
            CreateMap<ContactGroup, ContactGroupDto>();
        }
    }
}
