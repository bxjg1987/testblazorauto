using Abp.Modules;
using Abp.Reflection.Extensions;
using System;
using System.Reflection;

namespace BXJG.Utils.EFCore
{
    [DependsOn(typeof(BXJGUtilsModule))]
    public class EFCoreModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
