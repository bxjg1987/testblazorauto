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
            #region 文件
            CreateMap<FileResult, FileDto>();
            #endregion

            #region 附件Attachment
            //CreateMap<AttachmentEntity, AttachmentDto>().ForMember(c => c.ExtensionData, c => c.MapFrom(en => Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string,object>>( en.ExtensionData)));
            //CreateMap<AttachmentEntity, AttachmentDto>().MapExtensionData();
            CreateMap<AttachmentEntity, AttachmentDto>();//扩展属性已在BXJG.Utils模块中统一配置了映射

            //CreateMap(typeof(AttachmentEntity), typeof(AttachmentDto)).ForMember("", e => e.ConvertUsing);
            //CreateMap<AttachmentEditDto, AttachmentEntity>().ForMember(c => c.ExtensionData, opt => opt.Ignore.());
            #endregion
        }
    }
}
