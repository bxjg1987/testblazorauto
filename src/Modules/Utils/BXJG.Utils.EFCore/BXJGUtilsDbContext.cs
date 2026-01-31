using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Entities.Auditing;
using Abp.MultiTenancy;
using Abp.Organizations;
using Abp.Zero.EntityFrameworkCore;
using BXJG.Utils.DataPermission;
using BXJG.Utils.Feedback;
using BXJG.Utils.Metadata;
using BXJG.Utils.OU;
using BXJG.Utils.Tag;
using Microsoft.EntityFrameworkCore;

namespace BXJG.Utils.EFCore
{
    /// <summary>
    /// BXJG Utils 模块的 DbContext 基类
    /// </summary>
    /// <remarks>
    /// 注意：数据权限功能已改为通过仓储基类实现，不再使用全局过滤器。
    /// 在主项目的 DbContext 上使用 [AutoRepositoryTypes] 特性指定 BXJGUtilsEfCoreRepositoryBase 作为默认仓储实现。
    /// </remarks>
    public abstract class BXJGUtilsDbContext<TTenant, TRole, TUser, TSelf> : AbpZeroDbContext<TTenant, TRole, TUser, TSelf>
       where TTenant : AbpTenant<TUser> where TRole : AbpRole<TUser> where TUser : AbpUser<TUser> where TSelf : AbpZeroDbContext<TTenant, TRole, TUser, TSelf>
    {
        /// <summary>
        /// 数据权限
        /// </summary>
        public virtual DbSet<DataPermissionEntity> DataPermissions { get; set; }
        
        /// <summary>
        /// 留言反馈
        /// </summary>
        public virtual DbSet<FeedbackEntity> Feedbacks { get; set; }
        
        /// <summary>
        /// 实体标签
        /// </summary>
        public virtual DbSet<TagEntity> Tags { get; set; }
        
        /// <summary>
        /// 元数据
        /// </summary>
        public virtual DbSet<MetadataEntity> Metadatas { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected BXJGUtilsDbContext(DbContextOptions<TSelf> options) : base(options)
        {
        }
    }
}
