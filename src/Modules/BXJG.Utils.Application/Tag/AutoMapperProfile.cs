using AutoMapper;
using BXJG.Utils.Application.Share.Files;
using BXJG.Utils.Application.Share.Tag;
using BXJG.Utils.Files;
using BXJG.Utils.Share.Files;
using BXJG.Utils.Share.Tag;
using System.Collections.Generic;
using System.Text.Json;
namespace BXJG.Utils.Application.Tag
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SelectableTagDto, TagDto>().ReverseMap();
        }
    }
}
