using Abp.Modules;
using System;
using BXJG.Common;
using Abp.Reflection.Extensions;
using Abp.Dependency;

namespace BXJG.Utils
{
    [DependsOn(typeof(BXJGUtilsModule))]
    public class BXJGUtilsWebModule :AbpModule
    {
        public override void Initialize()
        {
            base.IocManager.RegisterIfNot<IEnv, AspNetEnv>(DependencyLifeStyle.Singleton);
            IocManager.RegisterAssemblyByConvention(typeof(BXJGUtilsWebModule).GetAssembly());
        }
    }
}
