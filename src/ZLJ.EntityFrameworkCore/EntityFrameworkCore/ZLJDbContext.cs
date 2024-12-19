using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using ZLJ.Core.Authorization.Roles;
using ZLJ.Core.Authorization.Users;
using ZLJ.Core.MultiTenancy;
using ZLJ.Core.BaseInfo;
using ZLJ.Core.BaseInfo.AssociatedCompany;
using ZLJ.Core.BaseInfo.StaffInfo;

using System.Reflection;
//using ZLJ.WorkOrder.RentOrderItemWorkOrder;
using ZLJ.EntityFrameworkCore.EntityFrameworkCore.EFMap.BaseInfo;
//using BXJG.WorkOrder.EFMaps;
using BXJG.Utils.Files;
using BXJG.Utils.EFCore.EFMaps;
//using ZLJ.WorkOrder.Workload;
using System;
//using ZLJ.Rent.Redeliveries;
//using ZLJ.WorkOrder;
//using BXJG.WorkOrder.WorkOrder;
using BXJG.Utils.GeneralTree;
using ZLJ.Core.BaseInfo.Post;
using ZLJ.Core.Customer;
using Abp.Runtime.Session;
using System.Linq;
using Abp.Organizations;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Abp.Domain.Uow;
using ZLJ.Core.Administrative;
using Abp;
using System.Threading.Tasks;
using Abp.Dependency;
//using Masuit.Tools.Reflection;
using Castle.MicroKernel.Lifestyle.Scoped;
using BXJG.Utils;

namespace ZLJ.EntityFrameworkCore
{
    public partial class ZLJDbContext : AbpZeroDbContext<Tenant, Role, User, ZLJDbContext>
    {
        /* Define a DbSet for each entity of the application */

        #region 主模块
        public DbSet<PostEntity> BaseInfoPosts { get; set; }
        public DbSet<OrganizationUnitEntity> OrganizationUnitEntities { get; set; }
        public DbSet<DataDictionaryEntity> BXJGGeneralTreeEntities { get; set; }

        public DbSet<AdministrativeEntity> BXJGBaseInfoAdministratives { get; set; }

        public DbSet<StaffInfoEntity> BXJGBaseInfoStaffInfo { get; set; }

        public DbSet<AssociatedCompanyEntity> BXJGBaseInfoAssociatedCompany { get; set; }


        #endregion





        #region 通用附件
        public DbSet<AttachmentEntity> BXJGAttachments { get; set; }
        public DbSet<FileEntity> BXJGFiles { get; set; }
        #endregion

        //后期考虑实现动态DbSet简化实体注册

        public IPrincipalAccessor PrincipalAccessor { get; set; }
        //public IIocResolver IocResolver { get; set; }
        #region Customer
        /// <summary>
        /// 客户的部门
        /// </summary>
        public DbSet<CustomerOUEntity> CustomerOUEntities { get; set; }
        /// <summary>
        /// 客户处的员工
        /// </summary>
        public DbSet<CustomerStaffInfoEntity> CustomerStaffInfos { get; set; }
        /// <summary>
        /// 客户岗位（角色）
        /// </summary>
        public DbSet<CustomerRole> CustomerPosts { get; set; }

        protected long? CurrentCustomerId => PrincipalAccessor.GetValueByClaimType<long?>("customerId");// GetCurrentCustomerIdOrNull();
        //protected long? GetCurrentCustomerIdOrNull()
        //{
        //    var ddd = CurrentUnitOfWorkProvider?.Current?.Filters?.SingleOrDefault(c => c.FilterName == "MayHaveCustomer")?.FilterParameters;// ["customerId"];

        //    if (ddd != default)
        //    {
        //        if (ddd.TryGetValue("CustomerId", out var rr))
        //        {
        //            return Convert.ToInt64(rr);
        //        }
        //    }
        //    var userOuClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == "customerId");
        //    if (string.IsNullOrEmpty(userOuClaim?.Value))
        //    {
        //        return null;
        //    }

        //    return long.Parse(userOuClaim.Value);
        //}
        protected bool IsCustomerFilterEnabled => CurrentUnitOfWorkProvider?.Current?.IsFilterEnabled("MustHaveCustomer") == true;


        protected override bool ShouldFilterEntity<TEntity>(IMutableEntityType entityType)
        {
            //若实体父类不是抽象类，则当前方法不会被调用，F12查看base.ShouldFilterEntity
            //这个规则没有测试，是猜测的，因为User本身也是继承的，但它的全局数据过滤器可以用

            if (typeof(IMustHaveCustomer).IsAssignableFrom(typeof(TEntity)))
            {
                return true;
            }

            return base.ShouldFilterEntity<TEntity>(entityType);
        }
        
        protected override Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>(ModelBuilder modelBuilder)
        {
            //这些代码都是启动时执行的，并不是每次查询执行
            var expression = base.CreateFilterExpression<TEntity>(modelBuilder);

            if (typeof(IMustHaveCustomer).IsAssignableFrom(typeof(TEntity)))
            {
                Expression<Func<TEntity, bool>> mayHaveOUFilter = e => ((IMustHaveCustomer)e).CustomerId == CurrentCustomerId  == IsCustomerFilterEnabled;
                expression = expression == null ? mayHaveOUFilter : CombineExpressions(expression, mayHaveOUFilter);
            }

            return expression;

            //var expression = base.CreateFilterExpression<TEntity>();

            //if (IsCustomerFilterEnabled && typeof(IMustHaveCustomer).IsAssignableFrom(typeof(TEntity)))
            //{
            //    var tt = typeof(TEntity);
            //    Expression<Func<TEntity, bool>> mayHaveOUFilter = e => ((IMustHaveCustomer)e).CustomerId == CurrentCustomerId;
            //    expression = expression == null ? mayHaveOUFilter : CombineExpressions(expression, mayHaveOUFilter);
            //}

            //return expression;
        }


        #endregion
        public ZLJDbContext(DbContextOptions<ZLJDbContext> options)
            : base(options)
        {
            //ChangeTracker.Tracked += ChangeTracker_Tracked;
        }

        //IShouldInitialize没啥用了
        //实体内访问ioc准备使用rougamo提供的静态方式反访问ioc

        //private void ChangeTracker_Tracked(object sender, EntityTrackedEventArgs e)
        //{
        //    if (e.FromQuery)
        //    {
        //        if (e.Entry.Entity is IShouldInitialize entity)
        //        {
        //            //Debug.WriteLine($"实体跟着事件执行初始化：{this.GetType().FullName} {Id}");
        //            //Logger.Debug($"实体跟着事件执行初始化：{e.Entry.Entity.GetType().FullName}");
        //            entity.Initialize();
        //        }
        //        if (e.Entry.Entity is IHaveIoc entity1)
        //        {
        //            //Debug.WriteLine($"实体跟着事件执行初始化：{this.GetType().FullName} {Id}");
        //            //Logger.Debug($"实体跟着事件执行初始化：{e.Entry.Entity.GetType().FullName}");
        //            if (entity1.IocResolver == default)
        //                entity1.IocResolver = IocResolver;
        //        }
        //        /*
        //         * 实体类或聚合根中在此项目中允许写一部分逻辑
        //         * 这部分逻辑仅处理当前实体或聚合根的状态，但处理过程可能依赖某些服务,
        //         * 由于实体或聚合根无法注入服务（所以需要反模式，在实体或聚合根内部通过ioc获取），所以在跟踪实体时自动注入，未跟踪的实体由用户自己的代码注入。
        //         * 这属于框架级别的东东，所以可用这样写些奇奇怪怪的东西
        //         * 安全起见，使用IScopedIocResolver
        //         * 实体上的属性 通常 使用 
        //         * [NotMapped]
        //         * public IScopedIocResolver Ioc { get; set; }
        //         */
        //        //var pinfo = e.Entry.Entity.GetType().GetProperty("Ioc", BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
        //        //if (pinfo != default)
        //        //{
        //        //    //Debug.WriteLine($"实体跟着事件执行初始化：{this.GetType().FullName} {Id}");
        //        //    // Logger.Debug($"实体跟着事件执行初始化：{e.Entry.Entity.GetType().FullName}");
        //        //    // entity1.IocManager = this.IocManager; //.Initialize();
        //        //    //  IIocManagerAccessor
        //        //    //  IContainerAccessor
        //        //    //   IScopeAccessor
        //        //    // IScopeAccessor
        //        //    // IServiceProviderExAccessor
        //        //    // iaccessor
        //        //    //base.service
        //        //   //IIocManagerAccessor
        //        //    //e.Entry.Entity.SetFieldOrPropertyValue("")
        //        //    e.Entry.Entity.SetField(pinfo.Name, ScopedIocResolver);
        //        //}
        //    }
        //}
        //注销事件好像不是太有必要
        //public override void Dispose()
        //{
        //    //  if (ChangeTracker != null)
        //    try
        //    {
        //        ChangeTracker.Tracked -= ChangeTracker_Tracked;
        //    }
        //    catch { }
        //    base.Dispose();
        //}
        //public override ValueTask DisposeAsync()
        //{

        //    try
        //    {
        //        ChangeTracker.Tracked -= ChangeTracker_Tracked;
        //    }
        //    catch { }
        //    return base.DisposeAsync();
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.LogTo(sql =>
            //{ 

            //}, Microsoft.Extensions.Logging.LogLevel.Information);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);//这个必须加，否则报错
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly())
                        .ApplyConfigurationBXJGUtils();
            //.ApplyConfigurationBXJGWorkOrder();

            //   modelBuilder.ConfigProtect(base.AbpSession.ser)
            /* ef警告
             * 参考：https://stackoverflow.com/questions/62550667/validation-30000-no-type-specified-for-the-decimal-column
             * Microsoft.EntityFrameworkCore.Model.Validation[30000]
             * No store type was specified for the decimal property 'UnitPrice' on entity type 'OrderItemAddRecordEntity'. This will cause values to be silently truncated if they do not fit in the default precision and scale. Explicitly specify the SQL server column type that can accommodate all the values in 'OnModelCreating' using 'HasColumnType', specify precision and scale using 'HasPrecision', or configure a value converter using 'HasConversion'.
             */
            var decimalProps = modelBuilder.Model.GetEntityTypes()
                                                 .SelectMany(t => t.GetProperties())
                                                 .Where(p => (System.Nullable.GetUnderlyingType(p.ClrType) ?? p.ClrType) == typeof(decimal));

            foreach (var property in decimalProps)
            {
                property.SetPrecision(18);
                property.SetScale(2);
            }
        }

        /*
         * 2023-7-7日更新
         * 软删除只要不行inner join就没问题，由于此库作为基础库，还是保留软删除，你可以看下面的注释了解详细情况
         * 
         * 以下是2023-7-7日以前的备注
         * 为何屏蔽软删除？
         * 
         * 客户看多余无用的设备信息很烦，总想去删除它，由于我们使用的软删除，没有办法借用数据库的外键约束
         * 所以即使设备被引用了，客户依然可以软删除成功，当客户再去查看租赁合同时，由于软删除的全局过滤器导致查询不到数据
         * 最终导致数据不一致（虽然数据库上还是一致的，但软件体现出来不一致了）
         * 
         * 如果不借用数据库的外键约束，我们必须在删除设备信息时做大量判断，看它是否被某个地方引用了，软件迭代过程中
         * 可能有另外的功能也需要引用设备信息，所以得不停的去修改原来的删除设备的方法，做是否被引用的检查
         * 
         * 类似的，整个系统都存在这种问题，因此决定全局屏蔽软删除。
         * 
         * abp的FullAuditedEntity定义了很多有用的属性，因此我们的实体往往继承它，它同时也实现了ISoftDelete接口
         * 而默认情况下，abp判断只要实现了ISoftDelete，就会执行软删除，抽象的应用服务默认也是使用软删除方法。
         * 
         * 又想使用FullAuditedEntity，又想禁言软删除比较麻烦，经过上面的分析，我们也不太需要软删除，因此决定全局禁言软删除
         * 
         * 关于类似设备信息，客户想删除某些不用的，免得看着烦，就使用IPassxx接口，用启用禁用的方式来实现，基础类信息都应该提供这个功能
         */
        ////去雄注释，所有软删除将变成硬删除
        //protected override void CancelDeletionForSoftDelete(EntityEntry entry)
        //{
        //    //base.CancelDeletionForSoftDelete(entry);
        //}
    }
}