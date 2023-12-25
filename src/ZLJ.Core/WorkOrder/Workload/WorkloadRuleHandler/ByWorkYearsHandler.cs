using Abp.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.BaseInfo.StaffInfo;

namespace ZLJ.WorkOrder.Workload.WorkloadRuleHandler
{
    public class ByWorkYearsHandler : IWorkloadRuleHandler
    {
        public int GetStaffWorkloadPoints(StaffInfoEntity staffInfo, int globalWorkloadPoints, List<WorkloadRuleEntity> workloadRuleEntities)
        {
            int points = globalWorkloadPoints;

            int staffWorkYearByMonth = (Clock.Now.Year - staffInfo.CreationTime.Year) * 12 + (Clock.Now.Month - staffInfo.CreationTime.Month);
            //未配置对应的工作量规则数据,返回全局设置的工作量积分
            if (workloadRuleEntities.Count != 0)
            {
                List<int> workYearList = new List<int>();
                workYearList.Add(0);
                List<WorkYearsInterval> workYearsIntervals = new List<WorkYearsInterval>();
                foreach (var item in workloadRuleEntities)
                {

                    WorkYearsInterval workYearsInterval = new WorkYearsInterval();
                    workYearsInterval.Points = item.Points;
                    workYearsInterval.Max = Convert.ToInt32(item.RuleParams);
                    workYearsIntervals.Add(workYearsInterval);
                    workYearList.Add(workYearsInterval.Max);
                }
                workYearList.Sort();
                workYearsIntervals = workYearsIntervals.OrderBy(p => p.Max).ToList();
                for (int i = 0; i < workYearsIntervals.Count; i++)
                {
                    workYearsIntervals[i].Min = workYearList[i] * 12;
                    workYearsIntervals[i].Max = workYearsIntervals[i].Max * 12;
                }
                foreach (var item in workYearsIntervals)
                {
                    if (staffWorkYearByMonth > item.Min && staffWorkYearByMonth <= item.Max)
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
