using AutoMapper;
using BXJG.Utils.GeneralTree;
using Abp.Application.Services.Dto;
using ZLJ.App.Admin.BaseInfo.AssociatedCompany;
using ZLJ.App.Admin.BaseInfo.AssociatedCompany.Dto;
using ZLJ.App.Admin.BaseInfo.StaffInfo;
using ZLJ.App.Admin.BaseInfo.Administrative.Dto;
using ZLJ.App.Admin.BaseInfo.Administrative;
//using ZLJ.WorkOrder.RentOrderItemWorkOrder;
using ZLJ.Authorization.Roles;
using ZLJ.App.Admin.Roles.Dto;
using Newtonsoft.Json;
//using ZLJ.Rent.Redeliveries;
//using ZLJ.Rent.Redeliveries.Dto;
//using ZLJ.App.Admin.WorkOrder.Workload.Dto;
//using ZLJ.WorkOrder.Workload;
//using ZLJ.App.Admin.WorkOrder.Workload.WorkloadRecord.Dto;
using ZLJ.App.Common.Administrative;
using ZLJ.Authorization.Users;
using ZLJ.BaseInfo.Administrative;
using ZLJ.BaseInfo.AssociatedCompany;
using ZLJ.App.Common.Users;

namespace ZLJ.App.Admin
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


            #region 组织单位
            //CreateMap<OUEditDto,Abp.Organizations.OrganizationUnit >().ForMember(c=>c.)

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
            CreateMap<CreateUserDto, ZLJ.Authorization.Users.User>()
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
            CreateMap<EditUserDto, ZLJ.Authorization.Users.User>()
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
            #endregion


        }
    }
}