using AutoMapper;
using BXJG.CMS.Ad;
using System.Text.Json;

namespace BXJG.CMS
{
    public class BXJGCMSMapProfile : Profile
    {
        public BXJGCMSMapProfile()
        {
            CreateMap<AdPositionEntity, FrontAdPositionDto>();
            CreateMap<AdControlEntity, FrontAdControlDto>()
                .ForMember(c => c.ExtensionData, c => c.MapFrom(d => JsonSerializer.Deserialize<dynamic>( d.ExtensionData,new JsonSerializerOptions())));
            CreateMap<AdRecordEntity, FrontAdRecordDto>();
        }
    }
}
