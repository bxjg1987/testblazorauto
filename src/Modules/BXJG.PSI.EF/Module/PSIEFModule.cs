using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Modules;
using Abp.Organizations;
using Abp.Reflection.Extensions;
using Abp.Runtime.Session;
using BXJG.Common.Contracts;
using BXJG.Utils;
using BXJG.Utils.EFCore;
using BXJG.Utils.Enums;
using BXJG.Utils.Interceptor;
using BXJG.Utils.Localization;
using BXJG.Utils.OU;
using BXJG.Utils.Settings;
using System.Reflection;

namespace BXJG.PSI
{
    [DependsOn(typeof(EFCoreModule), typeof(PSICoreModule))]
    public class PSIEFModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
