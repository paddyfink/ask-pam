using AskPam.Crm.Organizations.Dtos;
using AskPam.Crm.Organizations;
using AutoMapper;

namespace AskPam.Crm.Organizations
{
    public class OrganizationsAutomapperProfile : Profile
    {
        public OrganizationsAutomapperProfile()
        {
            CreateMap<Organization, OrganizationDto>();
        }

    }
}
