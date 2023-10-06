using Abp.Dependency;
using Castle.Core.Logging;
using MudBlazor;
using Rougamo;
using Rougamo.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
namespace BXJG.AbpMudBlazor.Interceptor
{

    /*
     * 只关注ui部分的拦截
     * blazor server中 控件的事件不一定在主线程，所以AsyncLocal传递数据行不通
     */

    public class ExceptionInterceptorAttribute: ExMoAttribute
    {
        const string scopedServicesKey = nameof(scopedServicesKey);// "scopedServices";
        const string loggerKey = nameof(loggerKey);   
        const string snackbarKey = nameof(snackbarKey);
        protected override void ExOnEntry(MethodContext context)
        {
            var services = IocManager.Instance.CreateScope();
            var loggerFactory = services.Resolve<ILoggerFactory>();

            var logger = loggerFactory.Create(context.TargetType.FullName);

            var temp = context.TargetType.GetProperty("Snackbar", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public).GetValue(context.Target);
            var snackbar = temp as ISnackbar;

            context.Datas.Add(scopedServicesKey, services);
            context.Datas.Add(loggerKey, logger);
            context.Datas.Add(snackbarKey, snackbar);
            //也可以定义个接口让组件实现


            snackbar.Add($"全局异常拦截器执行前！context对象：{context.GetHashCode()} 线程id：{Thread.CurrentThread.ManagedThreadId}");

            logger.Debug($"全局异常拦截器执行前！context对象：{context.GetHashCode()} 线程id：{Thread.CurrentThread.ManagedThreadId}"  );
           // GloableStatic.Snackbar.Value.Add("全局异常拦截器执行前！context对象：" + context.GetHashCode());
        }

        protected override void ExOnExit(MethodContext context)
        {
            var logger = context.Datas[loggerKey] as ILogger;
            logger.Debug($"全局异常拦截器执行结束！context对象：{context.GetHashCode()} 线程id：{Thread.CurrentThread.ManagedThreadId}");
           
            logger.Debug(Environment.NewLine);

            var snackbar = context.Datas[snackbarKey] as ISnackbar;
            snackbar.Add($"全局异常拦截器执行后！context对象：{context.GetHashCode()} 线程id：{Thread.CurrentThread.ManagedThreadId}");


            (context.Datas[scopedServicesKey] as IScopedIocResolver)!.Dispose();
        }
    }
}