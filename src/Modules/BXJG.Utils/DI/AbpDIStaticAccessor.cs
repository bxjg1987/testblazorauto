using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BXJG.Utils.DI
{
    public static class AbpDIStaticAccessor
    {
        public static readonly AsyncLocal<IScopedIocResolver> iocResolver = new AsyncLocal<IScopedIocResolver> ();

       public static IScopedIocResolver IocResolver=> iocResolver.Value;  
    }
}
