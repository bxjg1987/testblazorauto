using Abp.Modules;
using ZLJ.Configuration;
using Abp.Hangfire;
using Abp.Threading.BackgroundWorkers;
//using ZLJ.App.Admin.WorkOrder.Workload;
using BXJG.Utils;
using ZLJ.EntityFrameworkCore;
using Abp.AspNetCore.SignalR;
using Abp.AspNetCore;
using ZLJ.App.Admin;
//using ZLJ.App.Employee;
using Medallion.Threading.SqlServer;
using Medallion.Threading;
using Yitter.IdGenerator;
using Microsoft.IdentityModel.Tokens;
using ZLJ.Authentication.JwtBearer;
using Abp.AspNetCore.Configuration;
//using BXJG.WorkOrder.EmployeeApplication;
//using BXJG.WorkOrder;
using Castle.MicroKernel.Resolvers;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Abp.Runtime.Session;
using Abp.Zero.Configuration;
using System.Reflection;
using Abp.Reflection.Extensions;
using Microsoft.AspNetCore.Components.Server.Circuits;
using BXJG.Common;

namespace ZLJ.Web.HostBlazor.Startup
{
    [DependsOn(typeof(ZLJWebCoreModule),typeof(BXJGUtilsRCLModule))]
    public class ZLJWebHostModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;


        public ZLJWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }
        public override void PreInitialize()
        {

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ZLJWebHostModule).GetAssembly());

            //IocManager.RegService(services =>
            //{
              
            //});
        }
        public override void PostInitialize()
        {
            //  IocManager.Resolve<ApplicationPartManager>().AddApplicationPartsIfNotAddedBefore(Assembly.GetExecutingAssembly());

            var workManager = IocManager.Resolve<IBackgroundWorkerManager>();

            //workManager.Add(IocManager.Resolve<CreateWorkloadRecordMonthlyWorker>());
        }
    }
}
