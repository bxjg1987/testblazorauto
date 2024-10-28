using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace BXJG.Common.DI
{
    /// <summary>
    /// 访问当前范围的ioc容器
    /// <para>普通的asp.net webapi/mvc/razorpage中，它提供当前请求范围的ioc容器</para>
    /// <para>blazor app server模式中，静态请求时同上，signalR连接时设置ioc容器，断开时清空；事件回调时默认是独立的ioc范围，会默认回到signalR的ioc范围</para>
    /// </summary>
    public class StaticDIAccessor
    {
        public static readonly AsyncLocal<IServiceProvider> _serviceProvider = new AsyncLocal<IServiceProvider>();

        public static IServiceProvider ServiceProvider => _serviceProvider.Value;
    }
}
