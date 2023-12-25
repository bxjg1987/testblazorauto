using Abp.Configuration.Startup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.txdl
{
    /// <summary>
    /// txdl配置对象
    /// </summary>
    public class EquipmentControlCenterConfig
    {
        public string ApiUrl { get; set; }
    }
    /// <summary>
    /// 扩展abp ModuleConfigurations 获取txdl的配置对象
    /// </summary>
    public static class TXDLCoreModuleConfigurationExtensions
    {
        public static EquipmentControlCenterConfig TXDLCore(this IModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.AbpConfiguration.Get<EquipmentControlCenterConfig>();
        }
    }
}
