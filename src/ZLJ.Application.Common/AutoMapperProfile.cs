using AutoMapper;
using ZLJ.Core.BaseInfo.AssociatedCompany;
using Abp.Organizations;
using ZLJ.Application.Common.OU;
using ZLJ.Core.BaseInfo;
using ZLJ.Core.BaseInfo.Post;
using ZLJ.Application.Common.Post;
using ZLJ.Application.Common.Role;
using ZLJ.Core.BaseInfo.StaffInfo;
using BXJG.Utils.Localization;
using Abp.Extensions;
using Abp.Dependency;
using ZLJ.Application.Common.Administrative;

using ZLJ.Application.Common.Share.OU;
using ZLJ.Core.MultiTenancy;
using ZLJ.Core.Authorization.Users;
using BXJG.Utils.Application.Share.Session;
using ZLJ.Core.Administrative;

namespace ZLJ.Application.Common
{
    public partial class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            //角色
            CreateMap<ZLJ.Core.Authorization.Roles.Role, RoleDto>();
            //岗位
            CreateMap<PostEntity, PostDto>().IncludeBase<ZLJ.Core.Authorization.Roles.Role, RoleDto>();
            //员工
            CreateMap<StaffInfoEntity, ZLJ.Application.Common.StaffInfo.Dto>();//.ForMember(c=>c.GenderText,c=>c.MapFrom(d=>d.Gender.ToLocalizationString()));

            CreateMap<AssociatedCompanyEntity, ZLJ.Application.Common.AssociatedCompany.Dto>()
                .ForMember(x => x.Value, o => o.MapFrom(m => m.Id.ToString()))
                .ForMember(x => x.DisplayText, o => o.MapFrom(m => m.Name));


            CreateMap<OrganizationUnit, OuDto>().ForMember(c => c.IconCls, c => c.MapFrom(c => "ou"))
                                                .ForMember(c => c.Id, c => c.MapFrom(c => c.Id.ToString()))
                                                .ForMember(c => c.Text, c => c.MapFrom(c => c.DisplayName))
                                                .ForMember(c => c.ParentId, c => c.MapFrom(c => c.ParentId));
            CreateMap<OrganizationUnitEntity, OuDto>().IncludeBase<OrganizationUnit, OuDto>()
                                                      .ForMember(c => c.OUTypeText, c => c.MapFrom(c => c.OUType));

            CreateMap<AdministrativeEntity, AdministrativeTreeNodeDto>().EntityToComboTree();
            CreateMap<AdministrativeEntity, AdministrativeComboboxItemDto>().EntityToCombobox();
            CreateMap<ZLJ.Core.Customer.CustomerOUEntity, Customer.OuDto>().ForMember(c=>c.Text,c=>c.MapFrom(d=>d.DisplayName));

            CreateMap<Tenant, TenantLoginInfoDto>();
            CreateMap<User, UserLoginInfoDto>();
        }
    }
}