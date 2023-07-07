using Abp.Configuration;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Threading;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Abp.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.App.Admin.BaseInfo.StaffInfo;
using ZLJ.WorkOrder.Workload;

namespace ZLJ.App.Admin.WorkOrder.Workload
{
    public class CreateWorkloadRecordMonthlyWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        private readonly IWorkloadManage _workloadManage;
        public CreateWorkloadRecordMonthlyWorker(AbpTimer timer,
            IWorkloadManage workloadManage) : base(timer)
        {
            Timer.Period = 24 * 60 * 60 * 1000;  //24小时

            _workloadManage = workloadManage;
        }
        /// <summary>
        /// 1.全局设置
        /// 2.工作量规则配置
        /// 3.员工设置岗位
        /// 4.员工如果设置的是维修岗,可以设置其工作量积分值(后续做)
        /// 5.工单对应积分值设置
        /// 6.显示员工所有工作量记录数据,并且可修改当月工作量的积分值
        /// 7.员工完成工单时，更新该员工当前月实际获得的积分值
        /// 8.app端根据工作量类型设置，显示已完成/应完成 的工单量或积分值
        /// </summary>
        protected override void DoWork()
        {
            if (Clock.Now.Day == 1) //每24小时执行一次，判断是每月的第一天 才执行
            {
                try
                {
                    AsyncHelper.RunSync(() => _workloadManage.CreateWorkloadRecordMonthly());
                }
                catch (Exception exception)
                {
                    Logger.Error(exception.Message, exception);
                }              
            }
        }
    }
}
