using Abp.Configuration.Startup;
using Abp.Dependency;
using BXJG.Utils.Enums;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Utils
{
    public static class BXJGUtilsModuleExtensions
    {
        public static BXJGUtilsModuleConfig BXJGUtils(this IModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.AbpConfiguration.Get<BXJGUtilsModuleConfig>();
        }

        //public static BXJGUtilsModuleConfig AddEnum(this BXJGUtilsModuleConfig cfg, Type t,string name=default,  string locationSourceName= UtilsConsts.LocalizationSourceName)
        //{
        //    // if (cfg.Enums == null)
        //    //  cfg.Enums = new List<EnumConfigItem>();
        //    //  var sdf = cfg.GetHashCode();
        //    cfg.Enums.Add(new EnumLocalizationDefine( t, name,locationSourceName));
        //    return cfg;
        //}

        //我们写了一个库，它不依赖abp，因为希望这个库也可以给原生asp.net core程序使用，其它很多第三方库也是类似的
        //这样的库往往会扩展IServiceCollection来实现服务注册
        //我们在abpModule中无法直接获取IServiceCollection
        //abp在Startup中AddAbp时会调用WindsorRegistrationHelper.AddServices，它会把IServiceCollection中的服务注册到IWindsorContainer
        //我们也可以把自己的IServiceCollection通过同样的方式注册进去
        //abp也为我们提供了更简单的方式，参考：https://github.com/aspnetboilerplate/aspnetboilerplate/issues/3851#issuecomment-421882748
        public static void RegService(this IIocManager iocManager, Action<IServiceCollection> act)
        {
            var services = new ServiceCollection();
            act(services);
            iocManager.IocContainer.AddServices(services);
            //WindsorRegistrationHelper.AddServices(iocManager.IocContainer, services);
        }
    }
}
