using AskPam.Crm.Common;
using AskPam.Crm.Common.Dtos;
using AutoMapper;

namespace AskPam.Crm.Controllers.Common
{
    public class CommonAutoMapperProfile : Profile
    {
        public CommonAutoMapperProfile()
        {
            CreateMap<Address, AddressDto>();
            CreateMap<Phone, PhoneDto>();
        }
    }
}
