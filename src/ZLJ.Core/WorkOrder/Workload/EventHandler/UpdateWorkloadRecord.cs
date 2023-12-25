using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Events.Bus.Handlers;
using Abp.Timing;
using BXJG.WorkOrder.WorkOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.WorkOrder.Workload.EventHandler
{
    public class UpdateWorkloadRecord : IEventHandler<StatusChangeingEventData>, ITransientDependency
    {
        private readonly IRepository<WorkloadRecordEntity, Guid> _workloadRecordRepository;
        public UpdateWorkloadRecord(IRepository<WorkloadRecordEntity, Guid> workloadRecordRepository)
        {
            _workloadRecordRepository = workloadRecordRepository;
        }
        public async void HandleEvent(StatusChangeingEventData eventData)
        {
            if (eventData.Entity.Status == Status.Completed || eventData.Entity.Status == Status.ToBeConfirmed)
            {
                DateTime statisticsTime = Clock.Now.AddDays(1 - DateTime.Now.Day).Date;
                var staffInfoId = Convert.ToInt32(eventData.Entity.EmployeeId);
                var item = await _workloadRecordRepository.SingleAsync(p => p.StaffInfoId == staffInfoId && p.StatisticsTime == statisticsTime);
                if (eventData.Entity.Status == Status.Completed && eventData.Original < eventData.Entity.Status)
                {
                    item.ActualPoints = item.ActualPoints + (eventData.Entity as WorkOrderBaseEntity).Points.Value;
                }
                //目前我们的系统不考虑回退情况，因此下面的操作暂时可以忽略
                else if (eventData.Entity.Status == Status.ToBeConfirmed && eventData.Original > eventData.Entity.Status)
                {
                    item.ActualPoints = item.ActualPoints - (eventData.Entity as WorkOrderBaseEntity).Points.Value;
                }
                //_workloadRecordRepository.Update(item);
            }
        }
    }
}
