using AutoMapper;
using ZLJ.Application.Common.Share.TestSimple;
using ZLJ.Core.TestSimple;

namespace ZLJ.Application.Common.TestSimple
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            var cfg = CreateMap<TestSimpleEntity, TestSimpleProviderDto>();
        }
    }
}