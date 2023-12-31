using AutoMapper;
using ZLJ.BaseInfo.AssociatedCompany;
using Abp.Organizations;
using ZLJ.App.Common.OU;
using ZLJ.BaseInfo;
using ZLJ.BaseInfo.Post;
using ZLJ.App.Common.Post;
using ZLJ.App.Common.Role;
using ZLJ.BaseInfo.StaffInfo;
using BXJG.Utils.Localization;
using Abp.Extensions;
using Abp.Dependency;
using ZLJ.App.Common.Administrative;
using ZLJ.BaseInfo.Administrative;
using ZLJ.Application.Common.Share.OU;

namespace ZLJ.App.Common
{
    public partial class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            //角色
            CreateMap<ZLJ.Authorization.Roles.Role, RoleDto>();
            //岗位
            CreateMap<PostEntity, PostDto>().IncludeBase<ZLJ.Authorization.Roles.Role, RoleDto>();
            //员工
            CreateMap<StaffInfoEntity, ZLJ.App.Common.StaffInfo.Dto>();//.ForMember(c=>c.GenderText,c=>c.MapFrom(d=>d.Gender.ToLocalizationString()));

            CreateMap<AssociatedCompanyEntity, ZLJ.App.Common.AssociatedCompany.Dto>()
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
            CreateMap<ZLJ.Customer.CustomerOUEntity, Customer.OuDto>().ForMember(c=>c.Text,c=>c.MapFrom(d=>d.DisplayName));
        }
    }
}