using System;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.MultiTenancy;
using ZLJ.EntityFrameworkCore.Seed.Host;
using ZLJ.EntityFrameworkCore.Seed.Tenants;
using BXJG.Shop.Seed;
using ZLJ.MultiTenancy;
using ZLJ.Authorization.Roles;
using ZLJ.Authorization.Users;

using BXJG.CMS.EFCore.Seed;
using BXJG.GeneralTree;
using BXJG.BaseInfo.EFCore.Seed;

namespace ZLJ.EntityFrameworkCore.Seed
{
    public static class SeedHelper
    {
        public static void SeedHostDb(IIocResolver iocResolver)
        {
            WithDbContext<ZLJDbContext>(iocResolver, SeedHostDb);
        }

        public static void SeedHostDb(ZLJDbContext context)
        {
            context.SuppressAutoSetTenantId = true;

            // Host seed
            new InitialHostDbBuilder(context).Create();

            // Default tenant seed (in host database).
            new DefaultTenantBuilder(context).Create();
            new TenantRoleAndUserBuilder(context, 1).Create();

            //以下内容应该放进DefaultTenantBuilder中
            new DefaultOrganizationUnit(context, 1).Create();
            new DefaultDataDictionaryBuilder(context, 1).Create();
            // new DefaultAdministrativeBuilder(context, 1).Create();

            //初始化基础信息模块中的数据
            new DefaultBXJGBaseInfoBuilder<Tenant, Role, User, ZLJDbContext>(context, 1).Create();

            //默认商城数据迁移
            var sc = new DefaultBXJGShopBuilder<Tenant, Role, User, ZLJDbContext>(context, 1);
            sc.Create();
            //cms演示数据
            new DefaultBXJGCMSBuilder<Tenant, Role, User, ZLJDbContext,GeneralTreeEntity>(context, 1).Create();
        }

        private static void WithDbContext<TDbContext>(IIocResolver iocResolver, Action<TDbContext> contextAction)
            where TDbContext : DbContext
        {
            using (var uowManager = iocResolver.ResolveAsDisposable<IUnitOfWorkManager>())
            {
                using (var uow = uowManager.Object.Begin(TransactionScopeOption.Suppress))
                {
                    var context = uowManager.Object.Current.GetDbContext<TDbContext>(MultiTenancySides.Host);

                    contextAction(context);

                    uow.Complete();
                }
            }
        }
    }
}
