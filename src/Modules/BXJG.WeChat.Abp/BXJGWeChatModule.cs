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

        //本来它应该使用构造函数注入，但是abp.Migrator没有注入这个，我们去修改abp框架不太好，因此我们自己代码改变下，毕竟ZLJ.Web.Host执行是我们可以通过属性注入正常得到配置对象
        public IConfiguration Configuration { get; set; }

        public BXJGWeChatModule()
        {
            //this.Configuration = Configuration;
            GetMiniProgramConfiguration = key => Configuration.GetSection(key);
            GetPayConfiguration = key => Configuration.GetSection(key);
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


            var wxRoot = Configuration?.GetSection(BXJGWeChatConst.RootConfigKey);

            
            if (wxRoot == null)
                return;

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
