using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BXJG.Utils.DI
{
    /// <summary>
    /// 提供访问当前范围的ioc容器的点
    /// </summary>
    public static class AbpDIStaticAccessor
    {
        public static readonly AsyncLocal<IScopedIocResolver> _resolver = new AsyncLocal<IScopedIocResolver> ();
        /// <summary>
        /// 获取当前范围的ioc容器解析器
        /// </summary>
       public static IScopedIocResolver IocResolver => _resolver.Value;  
    }
}
