using AutoMapper;
using System.Text.Json;
namespace BXJG.GoodsInfo.Application.Admin
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap(typeof(CategoryEditDto), typeof(CategoryEntity));
            CreateMap(typeof(CategoryEntity), typeof(CategoryDto));
        }
    }
}
