using Abp.AutoMapper;
using Abp.Modules;
using BXJG.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

//[assembly: AbpBBException]

namespace ZLJ.Web.Blazor
{
    [DependsOn(typeof(BXJG.Utils.BXJGUtilsRCLModule))]
    public class AbpBlazorModule: AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            IocManager.RegService(services => {
                services.AddAntDesign();
            });
        }

    
    }
}
