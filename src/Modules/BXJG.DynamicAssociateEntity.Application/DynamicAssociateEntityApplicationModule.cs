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
            var config = IocManager.Resolve<DynamicAssociateEntityDefineManager>();
            foreach (var item in config.GroupedDefines)
            {
                foreach (var item1 in item.Value)
                {
                    var temp = item1;
                    while (temp!=null)
                    {
                        IocManager.RegisterIfNot(typeof(IDynamicAssociateEntityService), temp.ServiceType, DependencyLifeStyle.Transient);
                        temp = item1.Child;
                    }
                }
              
            }
        }
    }
}
