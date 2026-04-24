using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BXJG.WeChat.Pay
{
    /// <summary>
    /// 微信支付平台证书定时刷新后台服务
    /// 定期检查证书是否即将过期并自动更新，替代原CertificateDefaultProvider构造函数中的new Task方式
    /// </summary>
    public class CertificateRefreshService : BackgroundService
    {
        private readonly CertificateDefaultProvider certificateProvider;
        private readonly ILogger<CertificateRefreshService> logger;

        public CertificateRefreshService(CertificateDefaultProvider certificateProvider,
                                         ILogger<CertificateRefreshService> logger)
        {
            this.certificateProvider = certificateProvider;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await certificateProvider.RefreshIfNeededAsync(stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "微信支付证书刷新服务异常");
                }

                try
                {
                    await Task.Delay(TimeSpan.FromHours(23), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }
    }
}
