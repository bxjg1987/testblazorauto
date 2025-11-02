using Abp.Configuration;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Application.Common.Share.Settings;

namespace ZLJ.Application.Common.Settings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SettingDefinitionGroup, SettingDefinitionGroupDto>().ForMember(x=>x.Code,x=>x.MapFrom(d=>d.Name));
            CreateMap<SettingDefinition, SettingDto>().ForMember(x=>x.CustomData,x=>x.MapFrom(d=>d.CustomData.ToDictionary()));
        }
    }
}
