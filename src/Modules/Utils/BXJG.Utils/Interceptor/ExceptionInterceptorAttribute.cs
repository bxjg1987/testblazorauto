using Abp.Dependency;
using Castle.Core.Logging;
using Rougamo;
using Rougamo.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
namespace BXJG.Utils.Interceptor
{
    /// <summary>
    /// 肉夹馍+abp的抽象拦截器
    /// </summary>
    public class AbpMoInterceptorAttribute: ExMoAttribute
    {
        const string scopedServicesKey = nameof(scopedServicesKey);// "scopedServices";
        const string loggerKey = nameof(loggerKey);



        protected override void ExOnEntry(MethodContext context)
        {
            var services = IocManager.Instance.CreateScope();
            var loggerFactory = services.Resolve<ILoggerFactory>();

            var logger = loggerFactory.Create(context.TargetType.FullName);


            context.Datas.Add(scopedServicesKey, services);
            context.Datas.Add(loggerKey, logger);

            //logger.Debug("全局异常拦截器执行了！");
        }

        protected override void ExOnExit(MethodContext context)
        {
            (context.Datas[loggerKey] as ILogger)!.Dispose();
            (context.Datas[scopedServicesKey] as IDisposable)?.Dispose();
        }
    }
}
