using System;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.MultiTenancy;
using ZLJ.EntityFrameworkCore.Seed.Host;
using ZLJ.EntityFrameworkCore.Seed.Tenants;
using ZLJ.MultiTenancy;
using ZLJ.Authorization.Roles;
using ZLJ.Authorization.Users;
using BXJG.Utils.GeneralTree;
using ZLJ.EntityFrameworkCore.EntityFrameworkCore.Seed.BaseInfo;
using DocumentFormat.OpenXml.InkML;

namespace ZLJ.EntityFrameworkCore.Seed
{
    public static class SeedHelper
    {
        public static void SeedHostDb(IIocResolver iocResolver)
        {
            WithDbContext<ZLJDbContext>(iocResolver, SeedHostDb);
        }
        //ef项目和migrator项目都可能调用此方法，默认ef里的开关是关闭的，这样不用每次启动都来做迁移；migrator里依赖ef模块，所以也会将跳过设置位true，因为migrator会主动来调用此方法
        //这里主要是对户主数据库设置种子数据，租户的不应该在这里搞，虽然能
        //但调用方在迁移租户时，只处理有单独数据库连接的租户，所以可以认为这里是主机和共享租户的初始化都在这里
        public static void SeedHostDb(ZLJDbContext context)
        {
            context.SuppressAutoSetTenantId = true;

            // Host seed
            new InitialHostDbBuilder(context).Create();

            // Default tenant seed (in host database).
            new DefaultTenantBuilder(context).Create();


            new DefaultOrganizationUnit(context, 1).Create();


            //new DefaultDataDictionaryBuilder(context, 1).Create();
            // new DefaultAdministrativeBuilder(context, 1).Create();

            //初始化基础信息模块中的数据
            //new DefaultBXJGBaseInfoBuilder(context, 1).Create();
            new DefaultBXJGBaseInfoDataDictionaryBuilder(context, 1).Create();
            new DefaultBXJGBaseInfoAdministrativeBuilder(context, 1).Create();
            new DefaultBXJGBaseInfoAssociatedCompanyBuilder(context, 1).Create();
            //new DefaultPostBuilder(context, 1).Create();


            new TenantRoleAndUserBuilder(context, 1).Create();
            //new DefaultBXJGBaseInfoStaffInfoBuilder(context, 1).Create();


            //初始化工单模块中的数据
            //new DefaultWordOrderBuilder<Tenant, Role, User, ZLJDbContext>(context, 1).Create();
            //默认商城数据迁移
            // new DefaultBuilder<Tenant, Role, User, ZLJDbContext>(context, 1).Create();
            //cms演示数据
            //  new DefaultBXJGCMSBuilder<Tenant, Role, User, ZLJDbContext, GeneralTreeEntity>(context, 1).Create();

            //  new WorkOrderBuilder(context, 1).Create();
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