using Abp.Dependency;
using Abp.Extensions;
using Abp.Organizations;
using AutoMapper;
using BXJG.Utils.Application.Share.OU;
using BXJG.Utils.Application.Share.Session;
using BXJG.Utils.Localization;
using ZLJ.Application.Common.Administrative;
using ZLJ.Application.Common.OU;
using ZLJ.Application.Common.Post;
using ZLJ.Application.Common.Role;
using ZLJ.Application.Common.Share.Kehu;
using ZLJ.Application.Common.Share.OU;
using ZLJ.Application.Common.Share.Post;
using ZLJ.Application.Common.Share.Roles;

using ZLJ.Core.Administrative;
using ZLJ.Core.AssociatedCompany;
using ZLJ.Core.Authorization.Users;
using ZLJ.Core.BaseInfo.Post;


using ZLJ.Core.MultiTenancy;
using ZLJ.Core.OU;



namespace ZLJ.Application.Common
{
    public partial class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region 角色
            ////角色
            //CreateMap<ZLJ.Core.Authorization.Roles.Role, RoleForSelectDto>();
            //角色
            //CreateMap<ZLJ.Core.Authorization.Roles.Role, RoleDto>();
            //CreateMap<ZLJ.Core.Authorization.Roles.Role, ZLJ.Application.Common.Share.Roles.RoleForSelectDto>().IncludeBaseSelectRole();

            #endregion

            #region 岗位
            //岗位
            CreateMap<PostEntity, PostForSelectDto>().IncludeBase<ZLJ.Core.Authorization.Roles.Role, RoleForSelectDto>();

            //岗位
            //CreateMap<PostEntity, PostDto>().IncludeBase<ZLJ.Core.Authorization.Roles.Role, RoleDto>();
            #endregion



            ////员工
            //CreateMap<StaffInfoEntity, ZLJ.Application.Common.StaffInfo.Dto>();//.ForMember(c=>c.GenderText,c=>c.MapFrom(d=>d.Gender.ToLocalizationString()));

            CreateMap<AssociatedCompanyEntity, KehuDto>();


            CreateMap<OrganizationUnit, OUSelectDto>().ForMember(c => c.IconCls, c => c.MapFrom(c => "ou"))
                                                .ForMember(c => c.Id, c => c.MapFrom(c => c.Id.ToString()))
                                                .ForMember(c => c.Text, c => c.MapFrom(c => c.DisplayName))
                                                .ForMember(c => c.ParentId, c => c.MapFrom(c => c.ParentId));
            CreateMap<OrganizationUnitEntity, OUSelectDto>().IncludeBase<OrganizationUnit, OUSelectDto>();

            CreateMap<AdministrativeEntity, AdministrativeTreeNodeDto>().EntityToComboTree();
            CreateMap<AdministrativeEntity, AdministrativeComboboxItemDto>().EntityToCombobox();
            CreateMap<ZLJ.Core.Customer.CustomerOUEntity, Customer.OuDto>().ForMember(c => c.Text, c => c.MapFrom(d => d.DisplayName));

            CreateMap<Tenant, TenantLoginInfoDto>();
            CreateMap<User, UserLoginInfoDto>();

         
        }
    }
}