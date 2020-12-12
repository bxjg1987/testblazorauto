using BXJG.WeChat.Pay;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WeChat
{
    /// <summary>
    /// 微信模块的abp配置对象
    /// </summary>
    public class BXJGWeChartModuleConfig
    {
        /// <summary>
        /// 获取微信模块关联的配置节点
        /// string：微信模块使用的顶级配置节点名称
        /// </summary>
        public Func<string, IConfiguration> GetConfiguration { get; set; }
        /// <summary>
        /// 配置微信支付的委托
        /// </summary>
        public Action<Pay.Option> ConfigPay { get; set; }
    }
}
