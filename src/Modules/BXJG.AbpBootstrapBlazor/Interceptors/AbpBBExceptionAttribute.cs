using Abp.Dependency;
using Abp.UI;
using Castle.Core.Logging;
using Rougamo;
using Rougamo.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.AbpBootstrapBlazor.Interceptors
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

    /// <summary>
    /// 基于abp和bootstrapblazor的全局异常处理拦截器
    /// </summary>
    public class AbpBBExceptionAttribute : MoAttribute
    {
        public override AccessFlags Flags => AccessFlags.Method;

        //public override Feature Features => Feature.Observe;//加了这个就不灵了，不晓得为啥

        const string scopedServicesKey = nameof(scopedServicesKey);// "scopedServices";
        const string loggerKey = nameof(loggerKey);
        const string snackbarKey = nameof(snackbarKey);
        //const string isIntercepKey = nameof(isIntercepKey);//是否拦截

        public override void OnEntry(MethodContext context)
        {
            var services = IocManager.Instance.CreateScope();
            context.Datas.Add(scopedServicesKey, services);

            var loggerFactory = services.Resolve<ILoggerFactory>();
            var logger = loggerFactory.Create(context.TargetType.FullName);

            context.Datas.Add(loggerKey, logger);

            var temp = context.Target.GetType().GetProperty("MessageService", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public).GetValue(context.Target);
            var snackbar = temp as MessageService;
            context.Datas.Add(snackbarKey, snackbar);

            //也可以定义个接口让组件实现
            //snackbar.Add($"全局异常拦截器执行前！context对象：{context.GetHashCode()} 线程id：{Thread.CurrentThread.ManagedThreadId} 方法：{ context.Method.Name }");
            //logger.Debug($"全局异常拦截器执行前！context对象：{context.GetHashCode()} 线程id：{Thread.CurrentThread.ManagedThreadId} 方法：{context.Method.Name}"  );
            //GloableStatic.Snackbar.Value.Add("全局异常拦截器执行前！context对象：" + context.GetHashCode());
        }

        public override void OnException(MethodContext context)
        {
            var snackbar = context.Datas[snackbarKey] as MessageService;
            //Task t;

            if (context.Exception is UserFriendlyException)
            {
                snackbar.Show(new MessageOption
                {
                    Content = $"错误！{context.Exception.Message}",
                    Color = Color.Danger,
                    ShowBorder = true,
                    ShowShadow = true
                });
            }
            else
            {
                var logger = context.Datas[loggerKey] as ILogger;
                logger.Error(@"{context.TargetType.FullName }.{context.Method.Name}" + context.Exception.StackTrace);

                //  (  context.Target as ComponentBase).tryinv

                snackbar.Show(new MessageOption
                {
                    Content = $"服务端发生未处理异常！请稍后重试，若多次失败，请联系系统管理员11。",
                    Color = Color.Danger,
                    ShowBorder = true,
                    ShowShadow = true
                });
                //snackbar.Add($"服务端发生未处理异常！请稍后重试，若多次失败，请联系系统管理员。", Severity.Error);
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
     // default(context.ret)
            context.HandledException(this, context.RealReturnType.GetDefaultValue());
        }

        public override void OnExit(MethodContext context)
        {
            (context.Datas[scopedServicesKey] as IScopedIocResolver)!.Dispose();
        }
    }
}