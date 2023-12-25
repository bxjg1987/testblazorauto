using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.EntityFrameworkCore;
using BXJG.Utils;
using BXJG.Utils.EFCore;
//using BXJG.Utils.EFCore.CAP;
//using DotNetCore.CAP;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using ZLJ.EntityFrameworkCore.Seed;

namespace ZLJ.EntityFrameworkCore
{
    [DependsOn(
        typeof(ZLJCoreModule),
        typeof(EFCoreModule),
        typeof(AbpZeroCoreEntityFrameworkCoreModule))]
    public class ZLJEntityFrameworkModule : AbpModule
    {
        /* Used it tests to skip dbcontext registration, in order to use in-memory database of EF Core */
        public bool SkipDbContextRegistration { get; set; }

        public bool SkipDbSeed { get; set; }

        public override void PreInitialize()
        {
            if (!SkipDbContextRegistration)
            {
                Configuration.Modules.AbpEfCore().AddDbContext<ZLJDbContext>(options =>
                {
                    if (options.ExistingConnection != null)
                    {
                        ZLJDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                    }
                    else
                    {
                        ZLJDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                    }
                });
            }

            Configuration.UnitOfWork.RegisterFilter("MayHaveCustomer", true);

          //  IocManager.IocContainer.Kernel.ComponentModelCreated += Kernel_ComponentModelCreated;
           // IocManager.IocContainer.Kernel.ComponentCreated += Kernel_ComponentCreated;
        }

        //private void Kernel_ComponentCreated(Castle.Core.ComponentModel model, object instance)
        //{
         
        //    if (instance is ICapPublisher temp)
        //    {
        //       // var temp = instance as ICapPublisher;

        //        temp.Transaction.Value = new System.Threading.AsyncLocal<ICapTransaction>(t=>  IocManager.Resolve<AbpCapTransaction>());
        //        temp.Transaction.Value.AutoCommit= true;
        //    }
        //}

        //private void Kernel_ComponentModelCreated(Castle.Core.ComponentModel model)
        //{
        //    if (model.ComponentName.Name.Contains("ICap", StringComparison.OrdinalIgnoreCase))
        //    {

        //    }
        //}

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ZLJEntityFrameworkModule).GetAssembly());
            //IocManager.Register<IEquipmentController, EquipmentController1>(Abp.Dependency.DependencyLifeStyle.Singleton);
        }

        public override void PostInitialize()
        {
            if (!SkipDbSeed)
            {
               SeedHelper.SeedHostDb(IocManager);
            }

            //IocManager.RegService(services => {
            //    services.Replace(new(typeof(IConfigureOptions<SqlServerOptions>), typeof(ssss), ServiceLifetime.Singleton));
            //});

            //IocManager.RegService(services =>
            //{
            //    //迁移时获取IConfiguration会报错，暂时这么处理，但非迁移的报错也会吃掉
            //    try
            //    {
            //        var cfg = IocManager.Resolve<IConfiguration>();
            //        services.Configure<EquipmentControlCenterConfig>(cfg.GetSection("txdl"));
            //        services.AddHttpClient(TXDLCoreConsts.HttpClientName,
            //           client =>
            //           {
            //               client.BaseAddress = new Uri(IocManager.Resolve<IOptionsMonitor<EquipmentControlCenterConfig>>().CurrentValue.ApiUrl);
            //           });
            //    }
            //    catch
            //    {

            //    }
              
            //    //services.PostConfigure<AppOptions>(opt =>
            //    //{
            //    //    if (opt.sbslzxpdsc == default)
            //    //        opt.sbslzxpdsc = 960000;

            //    //    if (opt.sbslzxrwjgsc == default)
            //    //        opt.sbslzxrwjgsc = 10000;

            //    //});
            //});
        }

        //public override void Shutdown()
        //{
        //    IocManager.IocContainer.Kernel.ComponentModelCreated -= Kernel_ComponentModelCreated;
        //    IocManager.IocContainer.Kernel.ComponentCreated -= Kernel_ComponentCreated;
        //}
    }
}
