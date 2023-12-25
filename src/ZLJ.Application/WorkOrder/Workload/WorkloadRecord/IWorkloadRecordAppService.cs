using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.App.Admin.WorkOrder.Workload.WorkloadRecord.Dto;

namespace ZLJ.App.Admin.WorkOrder.Workload.WorkloadRecord
{
    public interface IWorkloadRecordAppService : IApplicationService
    {
        /// <summary>
        /// 根据维修人员Id获取其当前月的工作量记录
        /// </summary>
        /// <param name="input">维修人员Id</param>
        /// <returns></returns>
        Task<WorkloadRecordDto> GetCurrentMonthRecord(EntityDto<long> input);
        /// <summary>
        /// 生成当月维修人员的工作量记录数据
        /// </summary>
        Task CreateWorkloadRecordMonthly();
    }
}
