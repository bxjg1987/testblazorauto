using Abp.Configuration.Startup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WeChat
{
    /// <summary>
    /// 微信模块相关扩展方法
    /// </summary>
    public static class BXJGWeChatExtensions
    {
        /// <summary>
        /// 获取微信模块配置对象
        /// </summary>
        /// <param name="moduleConfigurations"></param>
        /// <returns></returns>
        public static BXJGWeChartModuleConfig BXJGWeChat(this IModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.AbpConfiguration.Get<BXJGWeChartModuleConfig>();
        }
    }
}
