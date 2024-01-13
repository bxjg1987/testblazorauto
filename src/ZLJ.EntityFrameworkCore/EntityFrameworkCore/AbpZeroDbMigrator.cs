using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Abp.MultiTenancy;
using Abp.Zero.EntityFrameworkCore;
using BXJG.Utils.GeneralTree;

using ZLJ.Core.Authorization.Roles;
using ZLJ.Core.Authorization.Users;
using ZLJ.EntityFrameworkCore.Seed;
using ZLJ.Core.MultiTenancy;

namespace ZLJ.EntityFrameworkCore
{
    public class AbpZeroDbMigrator : AbpZeroDbMigrator<ZLJDbContext>
    {
        public AbpZeroDbMigrator(
            IUnitOfWorkManager unitOfWorkManager,
            IDbPerTenantConnectionStringResolver connectionStringResolver,
            IDbContextResolver dbContextResolver)
            : base(
                unitOfWorkManager,
                connectionStringResolver,
                dbContextResolver)
        {
        }

        //public override void CreateOrMigrateForTenant(AbpTenantBase tenant)
        //{
        //    base.CreateOrMigrateForTenant(tenant, context => {
        //        new DefaultOrganizationUnit(context, tenant.Id).Create();
        //        new DefaultDataDictionaryBuilder(context, tenant.Id).Create();
        //        new DefaultAdministrativeBuilder(context, tenant.Id).Create();

        //        //默认商城数据迁移
        //        new DefaultBXJGShopBuilder<Tenant, Role, User, ZLJDbContext, AdministrativeEntity, GeneralTreeEntity>(context, tenant.Id).Create();
        //        //cms演示数据
        //        new DefaultBXJGCMSBuilder<Tenant, Role, User, ZLJDbContext, GeneralTreeEntity>(context, tenant.Id).Create();
        //    });
        //}
    }
}
