using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.EntityFrameworkCore;
using ZLJ.EntityFrameworkCore.Seed;

namespace ZLJ.EntityFrameworkCore
{
    [DependsOn(
        typeof(ZLJCoreModule), 
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
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ZLJEntityFrameworkModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            if (!SkipDbSeed)
            {
               SeedHelper.SeedHostDb(IocManager);
            }
        }
    }
}
