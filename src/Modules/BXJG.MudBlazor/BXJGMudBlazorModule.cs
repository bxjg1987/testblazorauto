using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Modules;
using BXJG.Utils;
using BXJG.Utils.CAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.MudBlazor
{
    [DependsOn(typeof(BXJGUtilsRCLModule))]
    public class BXJGMudBlazorModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            // IocManager.Register(typeof(AbpAsyncDeterminationInterceptor<AbpCapSubscriptInterceptor>), DependencyLifeStyle.Transient);
            //IocManager.Register(typeof(GeneralTreeManager<>), DependencyLifeStyle.Transient);
            //Configuration.ReplaceService(typeof(GeneralTreeManager<>), () => default,DependencyLifeStyle.Transient);

            //注册附件应用服务，它不实现abp的应用服务，所以不会生成动态webApi
            //IocManager.Register(typeof(AttachmentAppService<>), DependencyLifeStyle.Transient);
        }
    }
}
