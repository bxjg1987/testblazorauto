using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Threading;
using ZLJ.Rent.Order;
using Hangfire;
using System.Threading.Tasks;
using ZLJ.Rent.Settle;

namespace ZLJ.Jobs
{
    /// <summary>
    /// 自动生成结算单，自动抄表
    /// </summary>
    public class ScanWaitingSettleOrderJob : AsyncBackgroundJob<object>, ITransientDependency
    {
        private readonly OrderManager _orderManager;
        private readonly SettleManager _settleManager;
        public ScanWaitingSettleOrderJob(OrderManager orderManager, SettleManager settleManager)
        {
            _orderManager = orderManager;
            _settleManager = settleManager;
        }

        //[UnitOfWork]
        //public override void Execute(object args)
        //{
        //    AsyncHelper.RunSync(async () =>
        //    {
        //        var list = await _orderManager.GetAllWaitingSettleOrdersAsync();

        //        foreach (var order in list)
        //        {
        //            BackgroundJob.Enqueue<SettleOrderJob>(x => x.Execute(order.Id));
        //            Logger.Info($"增加任务：{order.Id}");
        //        }
        //    });
        //}
        [UnitOfWork]
        public override async Task ExecuteAsync(object args)
        {
            var list = await _orderManager.GetAllWaitingSettleOrdersAsync();

            foreach (var order in list)
            {
                //BackgroundJob.Enqueue<SettleOrderJob>(x => x.Execute(order.Id));
                //Logger.Info($"增加任务：{order.Id}");
                await _settleManager.CreateSettleAsync(order.Id);
                Logger.Info($"自动生成结算单{order.OrderNo}");
            }
        }
    }
}