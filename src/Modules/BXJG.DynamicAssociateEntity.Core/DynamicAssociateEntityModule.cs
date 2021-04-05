
using Abp.Dependency;
using Abp.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.DynamicAssociateEntity
{
    public class DynamicAssociateEntityModule : AbpModule
    {
        public override void PreInitialize()
        {
            //注册此模块的配置对象，默认单例
            IocManager.Register<DynamicAssociateEntityConfiguration>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PostInitialize()
        {
            //var providers = IocManager.ResolveAll<IDynamicAssociateEntityDefineProvider>();
            var config = IocManager.Resolve<DynamicAssociateEntityConfiguration>();
            //考虑要不要去重，比如一个模块已注册了 关联到订单，另一个模块又注册了，它们的ServiceType属性一样，其它属性可能不同
            //但有可能A模块希望关联到订单明细，B模块只想关联到订单的情况
            //目前考虑是各模块保证不插入重复注册
            var context = new DynamicAssociateEntityDefineInitContext();
            foreach (var providerType in config.Providers)
            {
                using (var provider = CreateProvider(providerType))
                {
                    foreach (var item in provider.Object.GetDefines(context))
                    {
                        //RegisterServiceType(item.Value);
                        config.DynamicAssociateEntityDefines.Add(item.Key, item.Value);
                    }
                }
            }
        }

        private IDisposableDependencyObjectWrapper<IDynamicAssociateEntityDefineProvider> CreateProvider(Type providerType)
        {
            return IocManager.ResolveAsDisposable<IDynamicAssociateEntityDefineProvider>(providerType);
        }
    }
}
