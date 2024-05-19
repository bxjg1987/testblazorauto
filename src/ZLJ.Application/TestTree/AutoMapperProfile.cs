using AutoMapper;
using ZLJ.Application.Share.TestTree;
using ZLJ.Core.TestTree;

namespace ZLJ.Application.TestTree
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TestTreeEntity, TestTreeDto>();
            CreateMap<TestTreeEditDto, TestTreeEntity>();
            CreateMap<TestTreeCreateDto, TestTreeEntity>();
        }
    }
}