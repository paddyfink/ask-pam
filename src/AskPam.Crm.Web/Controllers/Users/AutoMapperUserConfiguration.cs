using AskPam.Crm.Users.Dtos;
using AskPam.Crm.Users;
using AutoMapper;

namespace AskPam.Crm.Authorization.Users
{
    public class UserAutomapperProfile : Profile
    {
        public UserAutomapperProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(c => c.Role, e => e.Ignore());
            CreateMap<Role, UserRoleDto>();
        }

    }
}
