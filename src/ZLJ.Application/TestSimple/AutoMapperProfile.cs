using AutoMapper;
using ZLJ.Application.Share.TestSimple;
using ZLJ.Core.TestSimple;

namespace ZLJ.Application.TestSimple
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TestSimpleEntity, TestSimpleDto>();
            CreateMap<TestSimpleEditDto, TestSimpleEntity>();
            CreateMap<TestSimpleCreateDto, TestSimpleEntity>();
        }
    }
}