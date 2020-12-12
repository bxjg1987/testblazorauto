using Abp.Dependency;
using Abp.Modules;
using BXJG.Utils;
using BXJG.WeChat.Pay;
using System;
using System.Reflection;

namespace BXJG.WeChat
{
    [DependsOn(typeof(BXJGUtilsModule))]
    public class BXJGWeChatModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            IocManager.Register<SecretHelper>(DependencyLifeStyle.Singleton); 
            IocManager.Register<IWXCertificateProvider, WXCertificateDefaultProvider>(DependencyLifeStyle.Singleton);
            IocManager.Register<PayServiceV3>(DependencyLifeStyle.Transient); 
        }
    }
}
