using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.App.Admin.WorkOrder.Workload.WorkloadRecord.Dto;
using ZLJ.WorkOrder.Workload;

namespace ZLJ.App.Admin.WorkOrder.Workload.WorkloadRecord
{
    public class WorkloadRecordAppService : ZLJAppServiceBase, IWorkloadRecordAppService
    {
        private readonly IRepository<WorkloadRecordEntity, Guid> _workloadRecordRepository;
        private readonly IWorkloadManage _workloadManage;
        public WorkloadRecordAppService(IRepository<WorkloadRecordEntity, Guid> workloadRecordRepository, IWorkloadManage workloadManage)
        {
            _workloadRecordRepository = workloadRecordRepository;
            _workloadManage = workloadManage;
        }
        public async Task CreateWorkloadRecordMonthly()
        {
            await _workloadManage.CreateWorkloadRecordMonthly();
        }

        public async Task<WorkloadRecordDto> GetCurrentMonthRecord(EntityDto<long> input)
        {
            DateTime statisticsTime = Clock.Now.AddDays(1 - DateTime.Now.Day).Date;
            var item = await _workloadRecordRepository.FirstOrDefaultAsync(p => p.StaffInfoId == input.Id && p.StatisticsTime == statisticsTime);
            var dto = base.ObjectMapper.Map<WorkloadRecordDto>(item);
            return dto;
        }
    }
}
