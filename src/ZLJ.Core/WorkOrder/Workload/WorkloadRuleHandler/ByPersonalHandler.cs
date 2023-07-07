using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.BaseInfo.StaffInfo;

namespace ZLJ.WorkOrder.Workload.WorkloadRuleHandler
{
    public class ByPersonalHandler : IWorkloadRuleHandler
    {
        public int GetStaffWorkloadPoints(StaffInfoEntity staffInfo, int globalWorkloadPoints, List<WorkloadRuleEntity> workloadRuleEntities)
        {
            int points = globalWorkloadPoints;
            //未配置对应的工作量规则数据,返回全局设置的工作量积分
            if (workloadRuleEntities.Count != 0)
            {
                foreach (var item in workloadRuleEntities)
                {
                    if (item.RuleParams.Contains(staffInfo.Id.ToString()))
                    {
                        points = item.Points;
                        break;
                    }
                }
            }
            return points;
        }
    }
}
