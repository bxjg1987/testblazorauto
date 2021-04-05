using Abp.Configuration.Startup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.DynamicAssociateEntity
{
    /// <summary>
    /// 动态关联实体模块的扩展方法
    /// </summary>
    public static class DynamicAssociateEntityExtensions
    {
        /// <summary>
        /// 获取动态关联实体模块的配置对象
        /// </summary>
        /// <param name="moduleConfigurations"></param>
        /// <returns></returns>
        public static DynamicAssociateEntityConfiguration DynamicAssociateEntity(this IModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.AbpConfiguration.Get<DynamicAssociateEntityConfiguration>();
        }
    }
}
