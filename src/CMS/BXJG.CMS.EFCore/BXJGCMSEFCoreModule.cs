using Abp.Modules;
using Abp.Reflection.Extensions;
using System;
using System.Reflection;

namespace BXJG.CMS.EFCore
{
    [DependsOn(
     typeof(BXJGCMSCoreModule))]
    public class BXJGCMSEFCoreModule : AbpModule
    {
        public static Assembly GetAssembly()
        {
            return typeof(BXJGCMSEFCoreModule).GetAssembly();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(GetAssembly());
        }
    }
}
