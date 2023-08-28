using AskPam.Crm.Authorization.Dtos;
using AskPam.Crm.Controllers.Authorization.Dtos;
using AskPam.Crm.Controllers.Libraries.Dtos;
using AskPam.Crm.Library;
using AutoMapper;

namespace AskPam.Crm.Authorization.Library
{
    public class AccountAutomapperProfile : Profile
    {
        public AccountAutomapperProfile()
        {
            CreateMap<User, ProfileDto>();
            CreateMap<User, ProfilePictureDto>();
            CreateMap<User, EmailSettingsDto>();            
        }
    }
}
