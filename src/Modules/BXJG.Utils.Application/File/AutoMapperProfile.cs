using AutoMapper;
using System.Text.Json;
namespace BXJG.Utils.File
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region ÎÄ¼þ
            CreateMap<FileResult, FileDto>();
            #endregion

            #region ¸½¼þAttachment
            CreateMap<AttachmentEntity, AttachmentDto>();
            //CreateMap<AttachmentEditDto, AttachmentEntity>().ForMember(c => c.ExtensionData, opt => opt.Ignore());
            #endregion
        }
    }
}
