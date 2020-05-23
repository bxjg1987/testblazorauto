using AutoMapper;
using BXJG.CMS.Ad;
using System.Text.Json;

namespace BXJG.CMS
{
    public class BXJGCMSMapProfile : Profile
    {
        public BXJGCMSMapProfile()
        {
            CreateMap<AdRecordEntity, FrontAdPositionDto>().ForMember(c => c.Controls, c => c.Ignore());
            CreateMap<AdRecordEntity, FrontAdControlDto>()
                .ForMember(c => c.AdControlType, c => c.MapFrom(d => d.AdControl.AdControlType))
                .ForMember(c => c.ExtensionData, c => c.MapFrom(d => JsonSerializer.Deserialize<dynamic>( d.AdControl.ExtensionData,new JsonSerializerOptions())));

            CreateMap<AdRecordEntity, FrontAdDto>().ForMember(c => c.RecordId, c => c.MapFrom(d=>d.Id));
        }
    }
}
