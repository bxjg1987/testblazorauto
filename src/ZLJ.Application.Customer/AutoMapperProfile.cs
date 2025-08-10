using AutoMapper;
using BXJG.Utils.GeneralTree;
using Abp.Application.Services.Dto;
//using ZLJ.WorkOrder.RentOrderItemWorkOrder;
using ZLJ.Core.Authorization.Roles;
//using ZLJ.Rent.Redeliveries;
//using ZLJ.Rent.Redeliveries.Dto;
//using ZLJ.Application.WorkOrder.Workload.Dto;
//using ZLJ.WorkOrder.Workload;
//using ZLJ.Application.WorkOrder.Workload.WorkloadRecord.Dto;
using ZLJ.Application.Common.Administrative;
using ZLJ.Core.Authorization.Users;

using ZLJ.Application.Common.Users;
using Abp.Auditing;
using ZLJ.Core.BaseInfo.Post;
using ZLJ.Core.MultiTenancy;
using ZLJ.Core.Administrative;
using ZLJ.Core.BaseInfo;
using BXJG.Utils.Application.Share.GeneralTree;
using ZLJ.Application.Common.Share.OU;

namespace ZLJ.Application.Customer
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
            
        }
    }
}