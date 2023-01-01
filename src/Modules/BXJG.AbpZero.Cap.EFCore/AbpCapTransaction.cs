using Abp.Domain.Uow;
using DotNetCore.CAP.Transport;
using DotNetCore.CAP;
using Abp.EntityFrameworkCore.Uow;
using Microsoft.EntityFrameworkCore;
using Abp.Modules;
using Abp.Configuration.Startup;
using Microsoft.Extensions.DependencyInjection;

namespace BXJG.AbpZero.Cap.EFCore
{
    //参考：https://www.cnblogs.com/savorboard/p/cap-6-2.html#5137563

    public class AbpCapTransaction<TDbContext> : CapTransactionBase where TDbContext : DbContext
    {
        public AbpCapTransaction(IDispatcher dispatcher, IActiveUnitOfWork uow) : base(dispatcher)
        {
            Uow = uow as EfCoreUnitOfWork;
            Uow.Completed += Uow_Completed;
        }

        private void Uow_Completed(object? sender, EventArgs e)
        {
            Flush();
        }

        public EfCoreUnitOfWork Uow { get; private set; }

        public override object? DbTransaction => Uow.GetOrCreateDbContext<TDbContext>().Database.CurrentTransaction;

        //提交留给abp自动提交去做；回滚本来就是自动的

        public override void Commit()
        {
            // Uow.
            //Uow.Complete();
            //Flush();
        }

        public override Task CommitAsync(CancellationToken cancellationToken = default)
        {
            //  await Uow.CompleteAsync();

            //Flush();
            return Task.CompletedTask;
        }

        public override void Rollback()
        {
            //  Uow.Current.rol
            // Uow.Rollback();

        }

        public override Task RollbackAsync(CancellationToken cancellationToken = default)
        {

            //  throw new NotImplementedException();
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            Uow.Completed -= Uow_Completed;
            //Uow.Dispose();
            Uow = null;
        }
    }
    public class pzdx
    {
       

        internal Func<IDispatcher, IActiveUnitOfWork, ICapTransaction> wt;
        public void InitDbContext<T>() where T : DbContext
        {
            wt = (c,uow) => new AbpCapTransaction<T>(c,uow);
        }
    }

    public static class pzdxsdf
    {
        public static pzdx AbpCapEFCore(this IModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.AbpConfiguration.Get<pzdx>();
        }
    }

    [DependsOn(typeof(Abp.EntityFrameworkCore.AbpEntityFrameworkCoreModule))]
    public class BXJGAbpCapEFCoreModule : AbpModule
    {
        // override pr
        public override void PreInitialize()
        {
            IocManager.Register<pzdx>();
            IocManager.IocContainer.Kernel.ComponentCreated += Kernel_ComponentCreated;
        }

        private void Kernel_ComponentCreated(Castle.Core.ComponentModel model, object instance)
        {
            // base.Configuration
            if (instance is ICapPublisher publisher)
            {
                var temp = publisher.ServiceProvider.GetRequiredService<IDispatcher>();
                var uow = IocManager.Resolve<IActiveUnitOfWork>();
                publisher.Transaction.Value = Configuration.Modules.AbpCapEFCore().wt(temp, uow);
            }
        }

        public override void Shutdown()
        {
            IocManager.IocContainer.Kernel.ComponentCreated -= Kernel_ComponentCreated;
        }
    }
}