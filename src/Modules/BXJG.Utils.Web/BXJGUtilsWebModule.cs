using Abp.Modules;
using System;
using BXJG.Common;
using Abp.Reflection.Extensions;
using Abp.Dependency;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Castle.Windsor.MsDependencyInjection;

namespace BXJG.Utils
{
    [DependsOn(typeof(BXJGUtilsModule),
               typeof(BXJGUtilsApplicationModule))]
    public class BXJGUtilsWebModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            IocManager.RegService(services => services.AddBXJGCommonWeb());
            //var services = new ServiceCollection();
            //services.AddBXJGCommonWeb();
            //IocManager.IocContainer.AddServices(services);
        }
    }
}
