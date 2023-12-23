using Abp.AutoMapper;
using Abp.Modules;
using BXJG.Common;
using BXJG.Utils.Notification;
using BXJG.Utils.RCL;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils
{
    [DependsOn(typeof(BXJG.Utils.BXJGUtilsWebModule))]
    public class BXJGUtilsRCLModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Configuration.Notifications.Notifiers.Add<EventRealTimeNotifier>();
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            IocManager.RegService(services =>
            {
                // services.AddBXJGCommonRCL();
                //services.AddScoped<CircuitStateHandler>();
                //  services.AddScoped<CircuitHandler>(c=>c.GetRequiredService<CircuitStateHandler>());
                services.AddScoped<TrackingCircuitHandler>();
                services.AddScoped<CircuitHandler>(x => x.GetRequiredService<TrackingCircuitHandler>());
                services.AddScoped<IZhongjieProvider>(x => x.GetRequiredService<TrackingCircuitHandler>());
            });
        }
    }
}
