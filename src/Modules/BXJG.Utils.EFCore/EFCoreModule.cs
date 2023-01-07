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
using BXJG.Utils.EFCore.CAP;

namespace BXJG.Utils.EFCore
{
    [DependsOn(typeof(BXJGUtilsModule))]
    public class EFCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<BXJGEFCoreConfiguration>();
            //引入cap动态解析连接字符串的方式会提示事务隔离级别报错，
            //参考：https://github.com/aspnetboilerplate/aspnetboilerplate/issues/4538
            //https://www.cnblogs.com/luckstar007/p/10949811.html
            Configuration.UnitOfWork.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PostInitialize()
        {
            Configuration.ReplaceService<IEntityHistoryHelper, BXJGEntityHistoryHelper>(DependencyLifeStyle.Transient);
        }
    }
}
