using AutoMapper;
using BXJG.Utils.Application.Share.GeneralTree;
using ZLJ.Application.Share.Administrative;
using ZLJ.Application.Share.MultiTenancy;
using ZLJ.Application.Share.Post;

namespace ZLJ.Admin.CoreRCL
{
    /// <summary>
    /// 后台管理端对象映射配置文件
    /// 具体的参考我们的对象映射文档
    /// </summary>
    public class AutoMpperProfile : AutoMapper.Profile
    {
        public AutoMpperProfile()
        {
            CreateMap<PostDto, PostEditDto>();
            CreateMap<DataDictionaryDto, DataDictionaryEditDto>();
            CreateMap<AdministrativeDto, AdministrativeEditDto>();
            CreateMap<TenantDto, EditTenantDto>();
        }
    }
}
