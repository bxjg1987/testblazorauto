using Abp.Configuration.Startup;
using System.Collections.Concurrent;

namespace ZLJ.App.Common
{
    /*
     * 目前项目有 后台管理、员工端、客户服务平台
     * 把它们看成独立的应用(App)，最终Host到主项目上，
     * 它们各自有自己的UI和Application层，但共享Core和其它基础设施
     * 应用信息只有主框架和后台管理端需要访问，其它应用自己知道自己是谁，但它们要有一个中心注册点
     * 所以应用的定义要么在CommonApplication中，要么在Web.Core中
     * 由于不是所有的应用都有web层，所以最终决定放CommonApplication中
     * 应用信息定义不需要持久化，因为不可能动态添加应用，不同应用有不同配置时使用settings
     * 但注意：不是所有信息都应该使用settings，比如不同应用可能有自己的登陆视图，这个没必要搞到settings里
     * 
     * 本质上应用容器配置就是个单例，但还是按abp的模块化配置方式来吧
     */

    ///// <summary>
    ///// 应用容器
    ///// </summary>
    //public class Apps: ConcurrentDictionary<string, AppInfo>
    //{ 
    
    //}

    /// <summary>
    /// 应用定义
    /// </summary>
    public class AppInfo
    {
        public string Key { get; set; }
        public string DisplayName { get; set; }

        //虽然应用层不应该提供ui层的东西，但加个更方便，就当这是个泛型字段吧
        //或者在应用层不设置这个值，而是留到ui层自己去设置

        /// <summary>
        /// 若应用有web，可以指定登陆视图，不指定则使用默认的
        /// </summary>
        public string LoginViewName { get; set; }

        //还可以指定些强类型
    }

    //public static class MyModuleConfigurationExtensions
    //{
    //    public static MyModuleConfig MyModule(this IModuleConfigurations moduleConfigurations)
    //    {
    //        return moduleConfigurations.AbpConfiguration.Get<MyModuleConfig>();
    //    }
    //}
}
