using AskPam.Crm.Contacts;
using AskPam.Crm.Contacts.Dtos;
using AskPam.Crm.InternalNotes.Dtos;
using AutoMapper;
using System;

namespace AskPam.Crm.InternalNotes
{
    public class InternalNotesAutomapperProfile : Profile
    {
        public InternalNotesAutomapperProfile()
        {
            //Contact Group
            CreateMap<InternalNote, NoteDto>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.Value.ToLocalTime()));
        }
    }
}
