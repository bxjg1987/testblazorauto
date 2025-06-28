using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using ZLJ.EntityFrameworkCore;
using ZLJ.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace ZLJ.Web.Tests;

[DependsOn(
    typeof(ZLJWebMvcModule),
    typeof(AbpAspNetCoreTestBaseModule)
)]
public class ZLJWebTestModule : AbpModule
{
    public ZLJWebTestModule(ZLJEntityFrameworkModule abpProjectNameEntityFrameworkModule)
    {
        abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
    }

    public override void PreInitialize()
    {
        Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(ZLJWebTestModule).GetAssembly());
    }

    public override void PostInitialize()
    {
        IocManager.Resolve<ApplicationPartManager>()
            .AddApplicationPartsIfNotAddedBefore(typeof(ZLJWebMvcModule).Assembly);
    }
}