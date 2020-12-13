using Abp.Dependency;
using Abp.Modules;
using BXJG.Utils;
using BXJG.WeChat.Pay;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using BXJG.WeChat.MiniProgram;
namespace BXJG.WeChat
{
    [DependsOn(typeof(BXJGUtilsModule))]
    public class BXJGWeChatModule : AbpModule
    {
        public Func<string, IConfiguration> GetMiniProgramConfiguration { get; set; }
        public Action<MiniProgram.Option> ConfigMiniProgram { get; set; }

        public Func<string, IConfiguration> GetPayConfiguration { get; set; }
        public Action<Pay.Option> ConfigPay { get; set; }

        private readonly IConfiguration configuration;

        public BXJGWeChatModule(IConfiguration configuration)
        {
            this.configuration = configuration;
            GetMiniProgramConfiguration = key => configuration.GetSection(key);
            GetPayConfiguration = key => configuration.GetSection(key);
        }

        public override void PreInitialize()
        {
            //IocManager.Register<BXJGWeChartModuleConfig>();
            //Configuration.Modules.BXJGWeChat().GetPayConfiguration = key => configuration.GetSection(key);
            //Configuration.Modules.BXJGWeChat().GetMiniProgramConfiguration = key => configuration.GetSection(key);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());


            var wxRoot = configuration.GetSection(BXJGWeChatConst.RootConfigKey);

            IocManager.RegService(services =>
            {
                if (ConfigPay != null)
                    services.AddWXPayCore().AddWXPayHttpClient().Configure(ConfigPay);
                else
                {
                    var c = wxRoot.GetSection(Pay.Const.RootConfigKey);
                    if (c != null && c.GetValue<bool>("enable"))
                        services.AddWXPayCore().AddWXPayHttpClient().Configure<Pay.Option>(c);
                }


                if (ConfigMiniProgram != null)
                    services.AddWXMiniProgramHttpClient().Configure(ConfigMiniProgram);
                else
                {
                    var c = wxRoot.GetSection(MiniProgram.Const.RootConfigKey);
                    if (c != null && c.GetValue<bool>("enable"))
                        services.AddWXMiniProgramHttpClient().Configure<MiniProgram.Option>(c);
                }
            });
        }
    }
}
