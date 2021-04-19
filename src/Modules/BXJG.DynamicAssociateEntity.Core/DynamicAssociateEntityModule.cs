
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
            IocManager.Resolve<DynamicAssociateEntityDefineManager>().Initialize();
        }
    }
}
