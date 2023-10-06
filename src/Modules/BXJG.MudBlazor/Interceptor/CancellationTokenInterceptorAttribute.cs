using Abp.Dependency;
using MudBlazor;
using Rougamo;
using Rougamo.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.AbpMudBlazor.Interceptor
{
    /*
     * 只拦截异步方法
     * 在方法执行前，用context.Target作为key 映射一个 CancellationTokenSource
     * 然后从ioc中获取 CancellationTokenProvider,然后Use
     * 执行后释放上面的Use，和CancellationTokenSource
     */

    public class CancellationTokenInterceptorAttribute : ExMoAttribute
    {
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

        protected override void ExOnExit(MethodContext context)
        {
            // 处理异常并将返回值设置为newReturnValue，如果方法无返回值(void)，直接传入null即可
            (context.Datas[scopedServicesKey] as IScopedIocResolver)!.Dispose();
        }
    }
}