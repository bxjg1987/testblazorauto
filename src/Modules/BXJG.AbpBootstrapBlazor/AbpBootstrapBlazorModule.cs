using Abp.AutoMapper;
using Abp.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.AbpBootstrapBlazor
{
    [DependsOn(typeof(BXJG.Utils.BXJGUtilsRCLModule))]
    public class AbpBootstrapBlazorModule: AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
