using Abp.Dependency;
using Abp.UI;
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


    public class ExceptionInterceptorAttribute : ExMoAttribute
    {
        public override AccessFlags Flags => AccessFlags.InstanceNonPublic | AccessFlags.InstancePublic;

        const string scopedServicesKey = nameof(scopedServicesKey);// "scopedServices";
        const string loggerKey = nameof(loggerKey);
        const string snackbarKey = nameof(snackbarKey);
        //const string isIntercepKey = nameof(isIntercepKey);//是否拦截

        protected override void ExOnEntry(MethodContext context)
        {
            var services = IocManager.Instance.CreateScope();
            context.Datas.Add(scopedServicesKey, services);


            var loggerFactory = services.Resolve<ILoggerFactory>();
            var logger = loggerFactory.Create(context.TargetType.FullName);

            context.Datas.Add(loggerKey, logger);

            var temp = context.TargetType.GetProperty("Snackbar", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public).GetValue(context.Target);
            var snackbar = temp as ISnackbar;
            context.Datas.Add(snackbarKey, snackbar);

            //也可以定义个接口让组件实现
            //snackbar.Add($"全局异常拦截器执行前！context对象：{context.GetHashCode()} 线程id：{Thread.CurrentThread.ManagedThreadId} 方法：{ context.Method.Name }");
            //logger.Debug($"全局异常拦截器执行前！context对象：{context.GetHashCode()} 线程id：{Thread.CurrentThread.ManagedThreadId} 方法：{context.Method.Name}"  );
            // GloableStatic.Snackbar.Value.Add("全局异常拦截器执行前！context对象：" + context.GetHashCode());
        }
        protected override void ExOnException(MethodContext context)
        {
            var snackbar = context.Datas[snackbarKey] as ISnackbar;
            if (context.Exception is UserFriendlyException)
            {
                snackbar.Add($"错误！{context.Exception.Message}", Severity.Error);
            }
            else
            {
                var logger = context.Datas[loggerKey] as ILogger;
                logger.Error(@" {context.TargetType.FullName }.{context.Method.Name}" + context.Exception.StackTrace);
                snackbar.Add($"服务端发生未处理异常！请稍后重试，若多次失败，请联系系统管理员。", Severity.Error);
            }

            // 处理异常并将返回值设置为newReturnValue，如果方法无返回值(void)，直接传入null即可
            context.HandledException(this, null);
        }
        protected override void ExOnExit(MethodContext context)
        {
            (context.Datas[scopedServicesKey] as IScopedIocResolver)!.Dispose();
        }

    }
}