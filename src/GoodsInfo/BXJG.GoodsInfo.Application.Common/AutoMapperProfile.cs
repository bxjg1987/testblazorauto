using AutoMapper;
using BXJG.GeneralTree;
using System.Text.Json;
namespace BXJG.GoodsInfo.Application.Common
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //automapper÷ß≥÷ºÃ≥–”≥…‰
            CreateMap<GoodsInfoEntity, GoodsInfoDto>().ForMember(c=>c.CategoryDisplayName,c=>c.MapFrom(d=>d.Category.DisplayName));
            //CreateMap(typeof(QueryTemp<>), typeof(GoodsInfoDto));
            CreateMap(typeof(GoodsInfoCategoryEntity), typeof(GoodsInfoCategoryTreeDto)).EntityToComboTree();
            CreateMap(typeof(GoodsInfoCategoryEntity), typeof(GoodsInfoCategoryComboboxDto)).EntityToCombobox();
        }
    }
}