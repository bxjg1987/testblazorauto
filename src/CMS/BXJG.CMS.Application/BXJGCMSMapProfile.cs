using AutoMapper;
using BXJG.CMS.Ad;
using BXJG.CMS.Column;
using BXJG.GeneralTree;
using System.Text.Json;
namespace BXJG.CMS
{
    public class BXJGCMSMapProfile : Profile
    {
        public BXJGCMSMapProfile()
        {
            #region 广告
            CreateMap<AdRecordEntity, FrontAdPositionControlEntityDto>()
               //.ForMember(c=>c.Ads,c=>c.Ignore())
               .ForMember(c => c.AdControlExtensionData, c => c.MapFrom(d => JsonSerializer.Deserialize<dynamic>(d.AdControl.ExtensionData, new JsonSerializerOptions())));
            CreateMap<AdRecordEntity, FrontAdRecordDto>();
            #endregion

            //按理说GeneralTreeMapProfile中已经配置了 .IncludeAllDerived() 就没必要这里IncludeBase
            //但这里可能是因为ColumnEntity<>是泛型的问题，经过测试必须加IncludeBase，
            //默认的通用字典和商城字典由于扩展的字典本身无泛型，因此没有这个要求
            CreateMap(typeof(ColumnEntity<>), typeof(ColumnDto)).IncludeBase(typeof(GeneralTreeEntity<>), typeof(GeneralTreeGetTreeNodeBaseDto<>));
        }
    }
}
