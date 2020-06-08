using AutoMapper;
using BXJG.CMS.Ad;
using BXJG.CMS.Article;
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

            #region 栏目
            CreateMap(typeof(ColumnEditDto), typeof(ColumnEntity<>)).DtoToEntity().ForMember("ContentType", opt => opt.Ignore());
            CreateMap(typeof(ColumnEntity<>), typeof(ColumnDto)).EntityToDto();//可能是因为泛型原因，必须调用EntityToDto
            CreateMap(typeof(ColumnEntity<>), typeof(ColumnTreeNodeDto)).EntityToComboTree();
            CreateMap(typeof(ColumnEntity<>), typeof(ColumnCombboxDto)).EntityToCombobox();
            #endregion

            #region 文章
            CreateMap(typeof(ArticleEditDto), typeof(ArticleEntity<>)).ForMember("Column", opt => opt.Ignore());
            CreateMap(typeof(ArticleEntity<>), typeof(ArticleDto));
            #endregion
        }
    }
}
