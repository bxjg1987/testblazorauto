using AutoMapper;
using BXJG.CMS.Ad;
using BXJG.CMS.Column;
using System.Text.Json;
namespace BXJG.CMS
{
    public class BXJGCMSMapProfile : Profile
    {
        public BXJGCMSMapProfile()
        {
            #region ¹ã¸æ
            CreateMap<AdRecordEntity, FrontAdPositionControlEntityDto>()
               //.ForMember(c=>c.Ads,c=>c.Ignore())
               .ForMember(c => c.AdControlExtensionData, c => c.MapFrom(d => JsonSerializer.Deserialize<dynamic>(d.AdControl.ExtensionData, new JsonSerializerOptions())));
            CreateMap<AdRecordEntity, FrontAdRecordDto>();
            #endregion


            //CreateMap(typeof(ColumnEntity<>), typeof(ColumnDto))
            // .ForMember(c => c.ExtData, opt => opt.MapFrom(c => JsonSerializer.Deserialize<dynamic>(c.ExtensionData)))
            //    .ForMember(c => c.Children, opt => opt.Ignore());
        }
    }
}
