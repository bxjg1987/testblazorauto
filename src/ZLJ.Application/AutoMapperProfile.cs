using AutoMapper;
using BXJG.Utils.GeneralTree;
using Abp.Application.Services.Dto;
using ZLJ.Application.Share.AssociatedCompany;
using ZLJ.Application.BaseInfo.StaffInfo;
//using ZLJ.WorkOrder.RentOrderItemWorkOrder;
using ZLJ.Core.Authorization.Roles;
using ZLJ.Application.Roles.Dto;
//using ZLJ.Rent.Redeliveries;
//using ZLJ.Rent.Redeliveries.Dto;
//using ZLJ.Application.WorkOrder.Workload.Dto;
//using ZLJ.WorkOrder.Workload;
//using ZLJ.Application.WorkOrder.Workload.WorkloadRecord.Dto;
using ZLJ.Application.Common.Administrative;
using ZLJ.Core.Authorization.Users;

using ZLJ.Application.Common.Users;
using Abp.Auditing;
using ZLJ.Application.Share.Auditing;
using ZLJ.Application.Share.Roles;
using ZLJ.Application.Share.Post;
using ZLJ.Core.BaseInfo.Post;
using ZLJ.Core.MultiTenancy;
using ZLJ.Application.Share.MultiTenancy;
using ZLJ.Application.Share.Administrative;
using ZLJ.Core.Administrative;
using ZLJ.Application.Share.OU;
using ZLJ.Core.BaseInfo;
using BXJG.Utils.Application.Share.GeneralTree;
using ZLJ.Application.Common.Share.OU;
using ZLJ.Core.AssociatedCompany;

namespace ZLJ.Application
{
    /// <summary>
    /// 统一的automapper映射文件
    /// </summary>
    public partial class AutoMapperProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public AutoMapperProfile()
        {
            CreateMap<AuditLog, AuditLogListDto>();

            #region 组织单位

            //CreateMap<DataDictionaryEntity, DataDictionaryDto>();//.IncludeBase(typeof(GeneralTreeEntity<>), typeof(GeneralTreeGetTreeNodeBaseDto<>));
            //CreateMap<DataDictionaryDto, DataDictionaryEditDto>();
            //CreateMap<DataDictionaryEditDto, DataDictionaryEntity>();
            //CreateMap<DataDictionaryEntity, DataDictionaryForSelectDto>().EntityToComboTree();//.IncludeBase(typeof(GeneralTreeEntity<>), typeof(GeneralTreeNodeDto<>));
            //CreateMap<DataDictionaryEntity, GeneralTreeComboboxDto>().EntityToCombobox();


            //CreateMap<OUEditDto,Abp.Organizations.OrganizationUnit >().ForMember(c=>c.)
            CreateMap<OrganizationUnitEntity, OuDto>();
            CreateMap<OUEditDto, OrganizationUnitEntity>();
            #endregion

            #region 省市区县
            CreateMap<AdministrativeEditDto, AdministrativeEntity>();
            CreateMap<AdministrativeEntity, AdministrativeDto>();

            #endregion

            #region 员工档案
            CreateMap<StaffInfoCreateDto, StaffInfoEntity>().ForMember(c => c.AgeDays, c => c.Ignore())
                                                          .ForMember(c => c.AgeMonths, c => c.Ignore())
                                                          //.ForMember(c => c.AgeString, c => c.Ignore())
                                                          .ForMember(c => c.AgeYears, c => c.Ignore())
                                                          .IncludeBase<CreateUserDto, User>();
            CreateMap<CreateUserDto, ZLJ.Core.Authorization.Users.User>()
                //.ForMember(c => c.Id, c => c.MapFrom(d => d.UserId))
                .ForMember(c => c.Password, c => c.Ignore())
                //.ForMember(c => c.Name, c => c.MapFrom(d => d.Name))
                .ForMember(c => c.Surname, c => c.MapFrom(d => d.Name))
                .ForMember(c => c.FullName, c => c.MapFrom(d => d.Name));


            CreateMap<StaffInfoEditDto, StaffInfoEntity>().ForMember(c => c.AgeDays, c => c.Ignore())
                                                          .ForMember(c => c.AgeMonths, c => c.Ignore())
                                                          //.ForMember(c => c.AgeString, c => c.Ignore())
                                                          .ForMember(c => c.AgeYears, c => c.Ignore())
                                                          .IncludeBase<EditUserDto, User>();
            CreateMap<EditUserDto, ZLJ.Core.Authorization.Users.User>()
                //.ForMember(c => c.Id, c => c.MapFrom(d => d.UserId))
                .ForMember(c => c.Password, c => c.Ignore())
                //.ForMember(c => c.Name, c => c.MapFrom(d => d.Name))
                .ForMember(c => c.Surname, c => c.MapFrom(d => d.Name))
                .ForMember(c => c.FullName, c => c.MapFrom(d => d.Name));

            CreateMap<StaffInfoEntity, StaffInfoDto>().IncludeBase<User, UserDto>();
            //.ForMember(c => c.UserName, c => c.MapFrom(d => d.User.UserName))
            //.ForMember(c => c.EmailAddress, c => c.MapFrom(d => d.User.EmailAddress))
            //.ForMember(c => c.IsActive, c => c.MapFrom(d => d.User.IsActive))
            //.ForMember(c => c.Roles, c => c.Ignore())
            //.ForMember(c => c.PhoneNumber, c => c.MapFrom(d => d.User.PhoneNumber))
            #endregion

            #region 来往单位

            CreateMap<AssociatedCompanyEditDto, AssociatedCompanyEntity>();
            CreateMap<AssociatedCompanyEntity, AssociatedCompanyDto>();

            #endregion

            #region 角色
            CreateMap<Role, RoleRelationDto>();
            CreateMap<RoleEditDto, Role>();

            CreateMap<CreatePostDto, PostEntity>().IncludeBase<CreateRoleDto, Role>();
            CreateMap<Role, PostDto>().IncludeBase<Role, RoleDto>();
            CreateMap<PostEntity, PostDto>().IncludeBase<Role, RoleDto>();
            CreateMap<PostEditDto, PostEntity>().IncludeBase<RoleEditDto, Role>();
            #endregion

            #region 租户
            CreateMap<EditTenantDto,Tenant>();
            CreateMap<Tenant, TenantDto>();
            #endregion
        }
    }
}