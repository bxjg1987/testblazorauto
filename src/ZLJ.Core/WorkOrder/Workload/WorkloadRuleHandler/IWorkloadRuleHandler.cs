using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.BaseInfo.StaffInfo;

namespace ZLJ.WorkOrder.Workload.WorkloadRuleHandler
{
    public interface IWorkloadRuleHandler
    {
        int GetStaffWorkloadPoints(StaffInfoEntity staffInfo, int globalWorkloadPoints, List<WorkloadRuleEntity> workloadRuleEntities);
    }
}
