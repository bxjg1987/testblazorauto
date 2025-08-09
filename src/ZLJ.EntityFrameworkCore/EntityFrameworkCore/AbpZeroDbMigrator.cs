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
        //abp默认的设计是首个租户和用户是需要迁移的，后续租户都是动态添加，可能需要用户自己设计管理员之类的
        //所以还是种子数据还是在领域服务或应用服务去处理
        //public override void CreateOrMigrateForTenant(AbpTenantBase tenant)
        //{
        //    //base.CreateOrMigrateForTenant(tenant, seedAction);
        //    CreateOrMigrate(tenant, db => {
        //        //共用种子数据builder
        //        new TenantRoleAndUserBuilder(db, tenant.Id, roleManagementConfig.StaticRoles);
        //        //其它种子数据还是在领域服务或应用服务去处理
        //    });
        //}

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
