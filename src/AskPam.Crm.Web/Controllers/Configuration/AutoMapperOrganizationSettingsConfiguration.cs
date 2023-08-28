using AskPam.Crm.AI.Entities;
using AskPam.Crm.Configuration.Dtos;
using AskPam.Crm.Controllers.Configuration.Dtos;
using AutoMapper;

namespace AskPam.Crm.Configuration
{
    public class SettingsAutomapperProfile : Profile
    {
        public SettingsAutomapperProfile()
        {
            CreateMap<QnAPair, QnAPairDto>();
            CreateMap<AI.QnAResult, QnAMakerResultDto>();
            CreateMap<Setting, SettingDto>();
        }
    }
}
