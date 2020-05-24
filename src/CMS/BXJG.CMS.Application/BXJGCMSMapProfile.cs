using AutoMapper;
using BXJG.CMS.Ad;
using System.Text.Json;

namespace BXJG.CMS
{
    public class BXJGCMSMapProfile : Profile
    {
        public BXJGCMSMapProfile()
        {
            CreateMap<AdRecordEntity, FrontAdPositionControlEntityDto>()
                //.ForMember(c=>c.Ads,c=>c.Ignore())
                .ForMember(c => c.AdControlExtensionData, c => c.MapFrom(d => JsonSerializer.Deserialize<dynamic>( d.AdControl.ExtensionData,new JsonSerializerOptions())));
            CreateMap<AdRecordEntity, FrontAdRecordDto>();
        }
    }
}
