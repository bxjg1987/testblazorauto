using Abp.Configuration;
using AutoMapper;
using BXJG.Utils.Application.Share.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Settings
{
    public abstract class AutoMapperProfile<TGroupDto, TDto> : Profile
        where TGroupDto : SettingDefinitionGroupDto<TGroupDto>
        where TDto : SettingDto<TGroupDto>
    {
        public AutoMapperProfile()
        {
            CreateMap<SettingDefinitionGroup, TGroupDto>().ForMember(x => x.Code, x => x.MapFrom(d => d.Name));
            CreateMap<SettingDefinition, TDto>().ForMember(x => x.CustomData, x => x.MapFrom(d => d.CustomData.ToDictionary()));
        }
    }

    public class AutoMapperProfile : AutoMapperProfile<SettingDefinitionGroupDto, SettingDto>
    {

    }
}
