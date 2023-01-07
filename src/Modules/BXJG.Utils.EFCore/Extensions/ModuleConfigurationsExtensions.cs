using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using DotNetCore.CAP.Transport;
using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BXJG.Utils.EFCore;

namespace Abp.Configuration.Startup
{
    public static class ModuleConfigurationsExtensions
    {
        /// <summary>
        /// 获取配置abp cap集成模块的配置对象
        /// </summary>
        /// <param name="moduleConfigurations"></param>
        /// <returns></returns>
        public static BXJGEFCoreConfiguration AbpCapEFCore(this IModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.AbpConfiguration.Get<BXJGEFCoreConfiguration>();
        }
    }
}
