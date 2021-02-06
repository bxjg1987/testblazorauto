using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Timing;
using Abp;
using BXJG.Utils.Localization;
using BXJG.Utils.Enums;
using Abp.Threading.BackgroundWorkers;
using BXJG.Utils.File;
using BXJG.Common;
using Abp.Dependency;
using BXJG.Utils.DynamicProperty;
using System.Reflection;

namespace BXJG.Utils
{
    [DependsOn( typeof(BXJGUtilsModule))]
    public class BXJGUtilsApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            IocManager.Register(typeof(DynamicPropertyAppService<>), DependencyLifeStyle.Singleton);
        }
    }
}
