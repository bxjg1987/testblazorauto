using AutoMapper;
using BXJG.GeneralTree;
using System.Text.Json;
namespace BXJG.GoodsInfo.Application.Common
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<GoodsInfoEntity, GoodsInfoDto>();
            CreateMap(typeof(CategoryEntity), typeof(CategoryTreeDto)).EntityToComboTree();
            CreateMap(typeof(CategoryEntity), typeof(CategoryComboboxDto)).EntityToCombobox();
        }
    }
}