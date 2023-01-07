using Abp.Domain.Uow;
using DotNetCore.CAP.Transport;
using DotNetCore.CAP;
using Abp.EntityFrameworkCore.Uow;
using Microsoft.EntityFrameworkCore;
using Abp.Modules;
using Abp.Configuration.Startup;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Policy;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace BXJG.Utils.EFCore.CAP
{

    /*
     * 参考：https://www.cnblogs.com/savorboard/p/cap-6-2.html#5137563
     * 由于cap要支持ef和数据库邓多种事务方式，因此定义了ICapTransaction对事务抽象
     * 
     * cap于业务系统的事务挂钩的核心思路如下：
     * ICapPublisher是单例的,通过AsyncLocal的方式存储当前事务对象
     * 业务系统的事务对象可以实现ICapTransaction，然后把这个对象赋值给ICapPublisher
     * cap提供的提交是由cap来做事务提交，但在abp中事务提交是自动的
     * 因此我们定义的事务对象AbpCapTransaction不处理提交和回滚相关操作
     * 而事务本身的提交肯定还是基于底层的事务对象的，这个是改由我们的业务系统来提交
     * 
     * cap官方文档有说明幂等性的问题，cap实现的最少一次，所以存在多次消费，幂等性需要订阅放自己处理
     * 因此猜测消费端只是消费和确认，但不保证消费与业务功能的幂等性，所以通常是 先消费（处理业务）然后确认（成功或是不都无所谓，大不了多消费一次）
     * 消费服务类实现ICapSubscribe，然后在方法上 [CapSubscribe(EventConstants.EVENT_NAME_CREATE_ORDER)]，可以试试不实现那个接口行不
     */

    /// <summary>
    /// 由于cap要支持ef和数据库邓多种事务方式，因此定义了ICapTransaction对事务抽象
    /// 
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public class AbpCapTransaction<TDbContext> : CapTransactionBase where TDbContext : DbContext
    {
        public AbpCapTransaction(IDispatcher dispatcher, IActiveUnitOfWork uow) : base(dispatcher)
        {
            //IUnitOfWorkManager ss;
            //ss.be

            //没必要做这个担心
            //因为最外层的uow提交，最终还是提交的数据库事务，还是基于数据库连接的事务
            //而这个数据库连接的事务已经于publisher的事务挂钩了。

            //IUnitOfWork temp = (IUnitOfWork)uow;
            //while (temp.Outer!=null)
            //{
            //    temp = temp.Outer;
            //}

            this.uow = uow as EfCoreUnitOfWork;
         

          // var ss = this.uow.Outer;
            //var tempOu = uow;
            //while (tempOu != null)
            //{
            //    tempOu = tempOu.Options
            //}

            this.uow.Completed += Uow_Completed;
        }

        private void Uow_Completed(object? sender, EventArgs e)
        {
            Flush();
            Dispose();
        }

        public EfCoreUnitOfWork uow { get; private set; }

        public override object? DbTransaction => uow.GetOrCreateDbContext<TDbContext>().Database.CurrentTransaction;

        //提交留给abp自动提交去做；回滚本来就是自动的

        public override void Commit()
        {
         throw new NotImplementedException();
        }

        public override Task CommitAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override void Rollback()
        {
            throw new NotImplementedException();
        }

        public override Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            uow.Completed -= Uow_Completed;
            //Uow.Dispose();
            uow = null;
            DbTransaction = null;
        }
    }
}