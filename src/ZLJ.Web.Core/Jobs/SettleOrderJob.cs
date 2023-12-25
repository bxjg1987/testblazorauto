using System;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Threading;
using ZLJ.Rent.Settle;

namespace ZLJ.Jobs
{
    public class SettleOrderJob// : IBackgroundJob<long>, ITransientDependency
    {
        //private readonly SettleManager _settleManager;

        //public SettleOrderJob(SettleManager settleManager)
        //{
        //    _settleManager = settleManager;
        //}

        //[UnitOfWork]
        //public virtual void Execute(long orderId)
        //{
        //    AsyncHelper.RunSync(async () =>
        //    {
        //        Console.WriteLine("开始");
        //        await _settleManager.CreateSettleAsync(orderId);
        //        Console.WriteLine("完成");
        //    });
        //}
    }
}