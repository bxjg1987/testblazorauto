using Abp.Dependency;
using Abp.Modules;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BXJG.DynamicAssociateEntity
{
    public class DynamicAssociateEntityApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
        public override void PostInitialize()
        {
            var config = IocManager.Resolve<DynamicAssociateEntityConfiguration>();
            foreach (var item in config.DynamicAssociateEntityDefines)
            {
                RegisterServiceType(item.Value);
            }
        }

        private void RegisterServiceType(IEnumerable<DynamicAssociateEntityDefine> dynamicAssociateEntityDefines)
        {
            if (dynamicAssociateEntityDefines != null)
            {
                foreach (var item in dynamicAssociateEntityDefines)
                {
                    IocManager.RegisterIfNot(typeof(IDynamicAssociateEntityService), item.ServiceType, DependencyLifeStyle.Transient);
                    RegisterServiceType(item.Children);
                }
            }
        }
    }
}
