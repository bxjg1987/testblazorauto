using Abp.Dependency;
using Abp.Modules;
using BXJG.Utils;
using BXJG.WeChat.Pay;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace BXJG.WeChat
{
    [DependsOn(typeof(BXJGUtilsModule))]
    public class BXJGWeChatModule : AbpModule
    {
        private readonly IConfiguration configuration;

        public BXJGWeChatModule(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public override void PreInitialize()
        {
            IocManager.Register<BXJGWeChartModuleConfig>();
            Configuration.Modules.BXJGWeChat().GetConfiguration = key => configuration.GetSection(key);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            var cfg = Configuration.Modules.BXJGWeChat();
            var wxRoot = cfg.GetConfiguration(BXJGWeChatConst.RootConfigKey);

            IocManager.RegService(services =>
            {
                services.AddWXPayCore().AddWXPayHttpClient();
                if (cfg.ConfigPay != null)
                    services.Configure(cfg.ConfigPay);
                else
                    services.Configure<Option>(wxRoot.GetSection(Pay.Const.RootConfigKey));
            });
        }
    }
}
