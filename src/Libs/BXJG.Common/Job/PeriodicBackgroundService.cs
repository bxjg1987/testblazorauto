using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BXJG.Common.Job
{
    /// <summary>
    /// 周期性任务
    /// </summary>
    [Obsolete("推荐使用abp的workder或.net最新的PeriodicTimer")]
    public abstract class PeriodicBackgroundService : BackgroundService
    {
        protected virtual int Interval => 5000;

        protected ILogger Logger;

        public PeriodicBackgroundService(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int jb = 5;//连续记录错误日志太多有问题，使用此参数确保少记录日志
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(Interval, stoppingToken);

                try
                {
                    await ExecuteCoreAsyc(stoppingToken);
                    jb = 5;
                }
                catch (Exception ex)
                {
                    await Task.Delay(Interval*jb, stoppingToken);//防止记录日志太多
                    jb++;
                    Logger.LogError(ex, $"周期性任务{GetType()}执行失败！");
                }
            }
        }
        protected abstract Task ExecuteCoreAsyc(CancellationToken stoppingToken);
    }
}
