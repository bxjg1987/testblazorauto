using AutoMapper;
using BXJG.Utils.Application.Share.Files;
using BXJG.Utils.Files;
using BXJG.Utils.Share.Files;
using System.Collections.Generic;
using System.Text.Json;
namespace BXJG.Utils.Application.File
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<FileEntity, FileDto>().ForMember(x => x.ExtensionData, x => x.Ignore());
            CreateMap<AttachmentEntity, AttachmentDto>();
        }
    }
}
