using AutoMapper;
using ZLJ.Application.Share.TestSimple;

namespace ZLJ.Admin.CoreRCL.TestSimple
{
    public class AutoMapperProfile : AutoMapper.Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TestSimpleDto, TestSimpleEditDto>();
        }
    }
}