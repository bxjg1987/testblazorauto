using AutoMapper;
using BXJG.Utils.Application.Share.GeneralTree;
using ZLJ.Application.Share.Administrative;
using ZLJ.Application.Share.AssociatedCompany;
using ZLJ.Application.Share.MultiTenancy;
using ZLJ.Application.Share.OU;
using ZLJ.Application.Share.Post;
using ZLJ.Application.Share.TestSimple;

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
            CreateMap<TestSimpleDto, TestSimpleEditDto>();

            CreateMap<AssociatedCompanyDto, AssociatedCompanyEditDto>();

            CreateMap<OUDto, OUEditDto>();
        }
    }
}
