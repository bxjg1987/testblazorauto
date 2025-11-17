using Abp.Application.Services.Dto;
using Abp.Auditing;
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
using ZLJ.Application.Common.Users;
using ZLJ.Application.Roles.Dto;
using ZLJ.Application.Share.Administrative;
using ZLJ.Application.Share.AssociatedCompany;
using ZLJ.Application.Share.Auditing;

using ZLJ.Application.Share.MultiTenancy;
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



namespace ZLJ.Application.StaffInfo
{
    /// <summary>
    /// 统一的automapper映射文件
    /// </summary>
    public  class AutoMapperProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public AutoMapperProfile()
        {
            #region 员工档案
            CreateMap<StaffInfoEditDto, StaffInfoEntity>().ForMember(c => c.AgeDays, c => c.Ignore())
                                                          .ForMember(c => c.AgeMonths, c => c.Ignore())
                                                          //.ForMember(c => c.AgeString, c => c.Ignore())
                                                          .ForMember(c => c.AgeYears, c => c.Ignore())
                                                          .IncludeBase<ZLJ.Application.Common.Share.User.UserEditDto, User>();

            CreateMap<StaffInfoCreateDto, StaffInfoEntity>().ForMember(c => c.AgeDays, c => c.Ignore())
                                                          .ForMember(c => c.AgeMonths, c => c.Ignore())
                                                          //.ForMember(c => c.AgeString, c => c.Ignore())
                                                          .ForMember(c => c.AgeYears, c => c.Ignore())
                                                          //.ForMember(x=>x.UserName,x=>x.MapFrom(d=>d.BaseDto.UserName))
                                                          .IncludeBase<ZLJ.Application.Common.Share.User.UserEditDto, User>();



            //CreateMap<UserEditDto, ZLJ.Core.Authorization.Users.User>()
            //    //.ForMember(c => c.Id, c => c.MapFrom(d => d.UserId))
            //    .ForMember(c => c.Password, c => c.Ignore())
            //    //.ForMember(c => c.Name, c => c.MapFrom(d => d.Name))
            //    .ForMember(c => c.Surname, c => c.MapFrom(d => d.Name))
            //    .ForMember(c => c.FullName, c => c.MapFrom(d => d.Name));

            CreateMap<StaffInfoEntity, StaffInfoDto>()/*.IncludeBase<User, ZLJ.Application.Common.Share.User.UserDto>()*/;
            //.ForMember(c => c.UserName, c => c.MapFrom(d => d.User.UserName))
            //.ForMember(c => c.EmailAddress, c => c.MapFrom(d => d.User.EmailAddress))
            //.ForMember(c => c.IsActive, c => c.MapFrom(d => d.User.IsActive))
            //.ForMember(c => c.Roles, c => c.Ignore())
            //.ForMember(c => c.PhoneNumber, c => c.MapFrom(d => d.User.PhoneNumber))
            #endregion
        }
    }
   
}