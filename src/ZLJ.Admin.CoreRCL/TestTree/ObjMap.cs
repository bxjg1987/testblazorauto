using AutoMapper;
using ZLJ.Application.Share.TestTree;

namespace ZLJ.Admin.CoreRCL.TestTree
{
    public class AutoMapperProfile : AutoMapper.Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TestTreeDto, TestTreeEditDto>();
        }
    }
}