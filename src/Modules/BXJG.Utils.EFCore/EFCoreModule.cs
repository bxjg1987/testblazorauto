using Abp.Modules;
using Abp.Reflection.Extensions;
using System;
using System.Reflection;
using Abp.Dependency;
using Abp.Configuration.Startup;
using Abp.EntityHistory;
using BXJG.Utils.EFCore.EntityHistory;
using Abp.Domain.Uow;
using DotNetCore.CAP.Transport;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Castle.Core;
using BXJG.Utils.EFCore.CAP;
using Abp.Application.Services;

namespace BXJG.Utils.EFCore
{
    [DependsOn(typeof(BXJGUtilsModule))]
    public class EFCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            // IocManager.Register<BXJGEFCoreConfiguration>();

            IocManager.IocContainer.Kernel.ComponentRegistered += (key, handler) =>
            {
                if (typeof(IApplicationService).IsAssignableFrom(handler.ComponentModel.Implementation))
                {
                    handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(AbpAsyncDeterminationInterceptor<AbpCapTranInterceptor>)));
                }
            };
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            IocManager.Register(typeof(AbpAsyncDeterminationInterceptor<AbpCapTranInterceptor>), DependencyLifeStyle.Transient);
        }

        public override void PostInitialize()
        {
            Configuration.ReplaceService<IEntityHistoryHelper, BXJGEntityHistoryHelper>(DependencyLifeStyle.Transient);
        }
    }
}
