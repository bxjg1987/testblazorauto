using AutoMapper;
using ZLJ.Application.Common.Share.TestTree;
using ZLJ.Core.TestTree;

namespace ZLJ.Application.Common.TestTree
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            var cfg = CreateMap<TestTreeEntity, TestTreeProviderDto>();
            cfg.EntityToComboTree();
        }
    }
}