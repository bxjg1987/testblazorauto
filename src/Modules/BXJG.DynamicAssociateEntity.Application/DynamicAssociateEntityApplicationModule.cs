using Abp.Dependency;
using Abp.Modules;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BXJG.DynamicAssociateEntity
{
    [DependsOn(typeof(DynamicAssociateEntityModule))]
    public class DynamicAssociateEntityApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PostInitialize()
        {
            //var config = IocManager.Resolve<DynamicAssociateEntityDefineManager>();
            //foreach (var temp in config.Defines)
            //{
            //    IocManager.RegisterIfNot(temp.Value.ServiceType, DependencyLifeStyle.Transient);
            //}
        }
    }
}
