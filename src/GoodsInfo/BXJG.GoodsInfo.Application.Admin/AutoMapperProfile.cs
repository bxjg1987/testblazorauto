using AutoMapper;
using System.Text.Json;
namespace BXJG.GoodsInfo.Application.Admin
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap(typeof(GoodsInfoCategoryEditDto), typeof(GoodsInfoCategoryEntity));
            CreateMap(typeof(GoodsInfoCategoryEntity), typeof(GoodsInfoCategoryDto));
        }
    }
}
