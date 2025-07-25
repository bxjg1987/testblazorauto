using Abp.Dependency;
using Abp.UI;
using AntDesign;
using Castle.Core.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rougamo;
using Rougamo.Context;
using Rougamo.Metadatas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.RCL.Interceptors
{
    /*
     * boostrapblazor默认的全局异常处理仅仅关注未处理的异常，它让应用程序不会崩溃，但它不会尝试恢复控件本身的状态
     * 这应该是合理的，不然它所有的组件事件处理都去加try  finaly  然后恢复状态太累了。
     * 
     * 所以这里使用aop方式定义异常处理拦截器，它记录错误日志，并提示用户。
     * 注意它不是全局异常，建议保留bb默认的全局异常处理方式，用来兜底。
     * 
     * aop框架：https://github.com/inversionhourglass/Rougamo
     * 
     */
    //public class AbpExceptionInterceptor1
    //{ 
    //    public static AsyncLocal<IServiceProvider> Services = new AsyncLocal<IServiceProvider>();
    //}
    /// <summary>
    /// 基于abp和bootstrapblazor的全局异常处理拦截器
    /// </summary>
    //[Pointcut(AccessFlags.Method | AccessFlags.Instance | AccessFlags.Public | AccessFlags.NonPublic | AccessFlags.InstancePublic | AccessFlags.InstanceNonPublic)]
    //[Advice(Feature.ExceptionHandle)]
    public class AbpExceptionInterceptorAttribute : MoAttribute
    {
        //经过测试，始终无法从路由中设置这个值
        //中间件设置是有效的
        //public static readonly AsyncLocal<IServiceProvider> Services = new AsyncLocal<IServiceProvider>();



        //IServiceProvider services => OperatingSystem.IsBrowser()?ServicesInBrower: AbpExceptionInterceptor1. Services.Value;

        //public override AccessFlags Flags => AccessFlags.Method | AccessFlags.Instance | AccessFlags.Public | AccessFlags.NonPublic | AccessFlags.InstancePublic | AccessFlags.InstanceNonPublic;
        //public override Feature Features => Feature.ExceptionHandle; // ;//| Feature.OnEntry; Feature.OnException

        /*
         * 省略访问修饰符标识拦截所有方法
         * 返回类型* 就是忽略
         * 继承于Microsoft.AspNetCore.Components.ComponentBase的所有子类
         * 的所有方法
         */

        //public override string? Pattern => "method(protected * BXJG.Utils.Components.AbpBaseComponent+.*(..))";

        //public override Feature Features => Feature.Observe;//加了这个就不灵了，不晓得为啥
        //ComponentBase
        //public override void OnEntry(MethodContext context)
        //{

        //    base.OnEntry(context);
        //}

        //const string scopedServicesKey = nameof(scopedServicesKey);// "scopedServices";
        //const string loggerKey = nameof(loggerKey);
        //const string snackbarKey = nameof(snackbarKey);
        //const string isIntercepKey = nameof(isIntercepKey);//是否拦截

        // public override void OnEntry(MethodContext context)
        // {
        //     var services = IocManager.Instance.CreateScope();
        //     context.Datas.Add(scopedServicesKey, services);

        //     var loggerFactory = services.Resolve<ILoggerFactory>();
        //     var logger = loggerFactory.Create(context.TargetType.FullName);

        //     context.Datas.Add(loggerKey, logger);

        //     var temp = context.Target.GetType().GetProperty("MessageService", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public).GetValue(context.Target);
        //     var snackbar = temp as MessageService;
        //     context.Datas.Add(snackbarKey, snackbar);
        //}

        public override void OnException(MethodContext context)
        {
            //经过测试，IServiceProvider不能从OwnComponentBase的ScopedService获取
            try
            {
                #region MyRegion
                var sddf = context.TargetType.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
                var tm = sddf.Where(x => x.PropertyType.IsImplementInterface<IServiceProvider>() && !x.Name.Equals("ScopedServices", StringComparison.OrdinalIgnoreCase)).Single();
                //if (tm == default)
                //     Console.WriteLine($"全局异常拦截失败！应用拦截器的组件中没有找到IServiceProvider类型的属性。");

                //允许后端报错，导致程序崩溃，方便开发阶段处理问题，另外最好在兜底的错误边界中记录日志

                //PropertyInfo tm;
                //if (mb.Count() > 1)
                //    tm = mb.Where(x => x.Name.Equals("services", StringComparison.OrdinalIgnoreCase)).Single();
                //else
                //    tm = mb.Single();

                var services = tm.GetValue(context.Target) as IServiceProvider;
                #endregion
                //Console.WriteLine($"全局异常拦截器中的ioc实例：{services.GetHashCode()}");

                Microsoft.Extensions.Logging.ILogger logger = services.GetRequiredService<Microsoft.Extensions.Logging.ILoggerFactory>().CreateLogger(context.TargetType);

                var snackbar = services.GetRequiredService<IMessageService>();


                logger.LogDebug("全局异常拦截器执行了...");
                var ex = context.Exception.GetBaseException();
                // var temp = context.Target.GetType().GetProperty("MessageService", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public).GetValue(context.Target);
                // var snackbar = temp as IMessageService;
                //Task t;
                //BXJG.Common.Extensions.us
                if (context.Exception is UserFriendlyException)
                {
                    //context.TargetType.InvokeMember("InvokeAsync", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, context.Target,new[]{ () =>
                    //{

                    //} });
                    snackbar.Error(ex.Message);
                }
                else
                {
                    var env = services.GetService<IHostEnvironment>();

                    logger.LogError($"{ex.Message}{Environment.NewLine}{context.Exception.StackTrace}"  );


                    if (env != default && !env.IsProduction())
                        snackbar.Error($"全局异常拦截：{ex.Message}{Environment.NewLine}{context.Exception.StackTrace}");
                    else
                        //  (  context.Target as ComponentBase).tryinv
                        snackbar.Error($"服务端发生未处理异常！请稍后重试，若多次失败，请联系系统管理员。{ex.Message}");

                    //snackbar.Add($"服务端发生未处理异常！请稍后重试，若多次失败，请联系系统管理员。", Severity.Error);
                }
            }
            catch (Exception ex)
            {
                //用户关闭页面时，获取服务会报错，导致程序崩溃，虽然页面布局有错误边界，程序还是会死掉。 防止程序崩溃，还是加个try
                //Console.WriteLine("全局异常拦截器崩溃！");
                //Console.WriteLine(ex.Message + DateTime.Now.ToString());
                //Console.WriteLine(ex.StackTrace);
            }
            //if (context.ReturnValue is Task xx)
            //{
            //    xx.ContinueWith(async cccc => await t);
            //}
            //else if (context.ReturnValue is ValueTask xx1)
            //{
            //    xx1.AsTask().ContinueWith(async cccc => await t);
            //}

            // 处理异常并将返回值设置为newReturnValue，如果方法无返回值(void)，直接传入null即可
      
            context.HandledException(this, context.ReturnType.GetDefaultValue());
        }

        //public override void OnExit(MethodContext context)
        //{
        //    (context.Datas[scopedServicesKey] as IScopedIocResolver)?.Dispose();
        //}
    }
}