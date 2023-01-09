using Abp.Domain.Uow;
using BXJG.Utils;
using DotNetCore.CAP;
using DotNetCore.CAP.Transport;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Domain.Uow
{
    public static class BXJGUtilsUowExtensions
    {
        public static void DisableTenantFilter(this IActiveUnitOfWork uow)
        {
            uow.DisableFilter(AbpDataFilters.MayHaveTenant, AbpDataFilters.MustHaveTenant);
        }
        /// <summary>
        /// 将cap的当前事务于abp uow的当前事务关联
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="publisher"></param>
        public static void MountCapAbpEFCore(this IUnitOfWorkManager uowManager, ICapPublisher publisher)
        {
            uowManager.Current.MountCapAbpEFCore(publisher);
        }
        /// <summary>
        /// 将cap的当前事务于abp uow的当前事务关联
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="publisher"></param>
        public static void MountCapAbpEFCore(this IActiveUnitOfWork uow, ICapPublisher publisher)
        {
            var temp = publisher.ServiceProvider.GetRequiredService<IDispatcher>();
            //var uow = publisher.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
            publisher.Transaction.Value = publisher.ServiceProvider.GetRequiredService<BXJGUtilsModuleConfig>().wt(temp, uow);
            publisher.Transaction.Value.AutoCommit = true;
        }
    }


}
