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

            //可能是因为泛型原因，必须调用EntityToDto
            CreateMap(typeof(ColumnEntity<>), typeof(ColumnDto)).EntityToDto();

            CreateMap(typeof(ColumnEntity<>), typeof(ColumnTreeNodeDto)).EntityToComboTree();
            CreateMap(typeof(ColumnEntity<>), typeof(ColumnCombboxDto)).EntityToCombobox();
        }
    }
}
