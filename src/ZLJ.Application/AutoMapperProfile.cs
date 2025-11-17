using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Organizations;
using Abp.Runtime.Session;
using AutoMapper;
using BXJG.Utils.Application.Share.User;
using BXJG.Utils.GeneralTree;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;


//using ZLJ.Rent.Redeliveries;
//using ZLJ.Rent.Redeliveries.Dto;
//using ZLJ.Application.WorkOrder.Workload.Dto;
//using ZLJ.WorkOrder.Workload;
//using ZLJ.Application.WorkOrder.Workload.WorkloadRecord.Dto;
using ZLJ.Application.Common.Administrative;
using ZLJ.Application.Common.Share.Roles;
using ZLJ.Application.Common.Users;
using ZLJ.Application.Roles.Dto;
using ZLJ.Application.Share.Administrative;
using ZLJ.Application.Share.AssociatedCompany;
using ZLJ.Application.Share.Auditing;

using ZLJ.Application.Share.MultiTenancy;
using ZLJ.Application.Share.OU;
using ZLJ.Application.Share.Post;
using ZLJ.Application.Share.Roles;
using ZLJ.Application.Share.StaffInfo;
using ZLJ.Core.Administrative;
using ZLJ.Core.AssociatedCompany;
//using ZLJ.WorkOrder.RentOrderItemWorkOrder;
using ZLJ.Core.Authorization.Roles;
using ZLJ.Core.Authorization.Users;
using ZLJ.Core.Localization;
using ZLJ.Core.MultiTenancy;
using ZLJ.Core.OU;

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



            #region 省市区县
            CreateMap<AdministrativeEditDto, AdministrativeEntity>();
            CreateMap<AdministrativeEntity, AdministrativeDto>();

            #endregion

            #region 租户
            CreateMap<EditTenantDto, Tenant>();
            CreateMap<Tenant, TenantDto>();
            #endregion
            #region 来往单位

            CreateMap<AssociatedCompanyEditDto, AssociatedCompanyEntity>();
            CreateMap<AssociatedCompanyEntity, AssociatedCompanyDto>();

            #endregion

            #region 角色
            CreateMap<Role, RoleRelationDto>();
            //CreateMap<RoleEditDto, Role>();

            //CreateMap<PostCreateDto, PostEntity>().IncludeBase<CreateRoleDto, Role>();
            //CreateMap<Role, PostDto>().IncludeBase<Role, RoleDto>();
            //CreateMap<PostEntity, PostDto>().IncludeBase<Role, RoleDto>();
            //CreateMap<PostEditDto, PostEntity>().IncludeBase<RoleEditDto, Role>();
            #endregion

        }
    }
  
}