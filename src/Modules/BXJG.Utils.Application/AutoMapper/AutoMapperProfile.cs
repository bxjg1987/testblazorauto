using Abp.Notifications;
using AutoMapper;
using BXJG.Utils.Application.Share.Notification;
using BXJG.Utils.GeneralTree;

using System.Text.Json;

namespace BXJG.Utils.Application.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<NotificationDefinition, NotifyDefineDto>().ForMember(d => d.EntityType, d => d.MapFrom(c => c.EntityType.FullName));

            CreateMap<temp, MessageDto>();
        }
    }
}
