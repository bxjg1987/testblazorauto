using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WeChat
{
    /*
     * abp模块中的配置对象通常是定义业务属性
     * 关于启动配置的东西没必要放在这个配置对象中
     * 但是按惯例，对模块进行配置我们习惯使用配置对象
     * 由于abp中的配置对象是单例，这些与启动配置相关的成员会常驻内存，是没有表的。
     * 因此最终决定将这部分启动配置相关代码移动到BXJGWeChatModule
     * 根本原因还是因为我们使用了asp.net core自带的选项模式，它与abp的配置对象模式重叠了
     */

    ///// <summary>
    ///// 微信模块的abp配置对象,它允许你在abp模块中对微信模块进行配置，当然你还是可以在Startup中配置
    ///// </summary>
    //public class BXJGWeChartModuleConfig
    //{
    ///// <summary>
    ///// 获取微信模块关联的配置节点的委托
    ///// string：微信模块使用的顶级配置节点名称
    ///// </summary>
    //public Func<string, IConfiguration> GetPayConfiguration { get; set; }
    ///// <summary>
    ///// 配置微信支付的委托
    ///// </summary>
    //public Action<Pay.Option> ConfigPay { get; set; }

    //public Func<string, IConfiguration> GetMiniProgramConfiguration { get; set; }
    //public Action<MiniProgram.Option> ConfigMiniProgram { get; set; }
    // }
}
