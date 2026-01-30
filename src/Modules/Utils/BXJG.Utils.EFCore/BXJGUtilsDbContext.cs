using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Entities.Auditing;
using Abp.MultiTenancy;
using Abp.Organizations;
using Abp.Specifications;
using Abp.Zero.EntityFrameworkCore;
using BXJG.Utils.DataPermission;
using BXJG.Utils.Feedback;
using BXJG.Utils.Metadata;
using BXJG.Utils.OU;
using BXJG.Utils.Share.DataPermission;
using BXJG.Utils.Tag;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.EFCore
{
    //用这种方式 会污染 模板项目，可能人家有自己的父类，这样强迫另一个项目必须继承BXJGUtilsDbContext不好
    //但这里只是提供默认的，你任然可以继承之前的，然后手动添加DbSet<FeedbackEntity> 
    public abstract class BXJGUtilsDbContext<TTenant, TRole, TUser, TSelf> : AbpZeroDbContext<TTenant, TRole, TUser, TSelf>
   where TTenant : AbpTenant<TUser> where TRole : AbpRole<TUser> where TUser : AbpUser<TUser> where TSelf : AbpZeroDbContext<TTenant, TRole, TUser, TSelf>
    {
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
        /// 当前数据权限过滤器是否是开启的
        /// </summary>
        protected virtual bool IsDataPermissionFilterEnabled => CurrentUnitOfWorkProvider?.Current?.IsFilterEnabled(DataPermissionConsts.DataPermission) == true;

        protected BXJGUtilsDbContext(DbContextOptions<TSelf> options) : base(options)
        {
        }

        protected override bool ShouldFilterEntity<TEntity>(IMutableEntityType entityType)
        {
            Type t = typeof(TEntity);

            //这里还有仔细思考优化

            //if (CurrentUnitOfWorkProvider.Current.Items.TryGetValue(DataPermissionConsts.DataPermission, out var sdf))
            //{
            //    var dp = sdf as Dictionary<string, DataPermissionDto>;
            //    var gt = dp[t.FullName].GrantType;
            //    if (gt == DataPermissionGrantType.All)
            //        return false;
            //    //if (gt == DataPermissionGrantType.Rejected)
            //    //    return true; //throw new Exception("");

            //    //if (gt == DataPermissionGrantType.OnlyMe)
            //}

            if (typeof(IMayHaveOrganizationUnit).IsAssignableFrom(t) || typeof(IMustHaveOrganizationUnit).IsAssignableFrom(t) || typeof(ICreationAudited).IsAssignableFrom(t))
            {
                return true;
            }

            return base.ShouldFilterEntity<TEntity>(entityType);
        }
        protected override Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>(ModelBuilder modelBuilder)
        {
            var expression = base.CreateFilterExpression<TEntity>(modelBuilder);

            //此方法仅在启动时调用，所以要把所有数据访问写进表达式

            if (IsDataPermissionFilterEnabled&&CurrentUnitOfWorkProvider.Current.Items.TryGetValue(DataPermissionConsts.DataPermission, out var sdf))
            {
                var dp = sdf as Dictionary<string,DataPermissionDto>;
                if (dp!=null&& dp.TryGetValue(typeof(TEntity).FullName, out  DataPermissionDto sj)) {
                    if (sj.GrantType == DataPermissionGrantType.All)
                    {
                    }
                    else if (sj.GrantType == DataPermissionGrantType.Rejected)
                    {
                        return x => false;
                    }
                    else if (sj.GrantType == DataPermissionGrantType.OnlyMe)
                    {
                        Expression<Func<TEntity, bool>> mayHaveOUFilter = e => (((ICreationAudited)e).CreatorUserId == AbpSession.UserId);
                        expression = expression == null ? mayHaveOUFilter : CombineExpressions(expression, mayHaveOUFilter);
                    }
                    else {
                        Expression<Func<TEntity, bool>> mayHaveOUFilter=e=>true;
                        if (sj.OrganizationUnitIds!=null&&sj.OrganizationUnitIds.Any())
                        {
                            mayHaveOUFilter = e => sj.OrganizationUnitIds.Any(c=>c.Equals(((IMayHaveOrganizationUnit)e).OrganizationUnitId));
                        }
                        if (sj.OrganizationUnitCodes != null) {
                            foreach (var code in sj.OrganizationUnitCodes)
                            {
                                mayHaveOUFilter = mayHaveOUFilter.Or(e => ((IOU)e).OrganizationUnit.Code.StartsWith(code));
                            }
                        }
                        expression = expression == null ? mayHaveOUFilter : CombineExpressions(expression, mayHaveOUFilter);
                    }
                }
                //if (typeof(IMayHaveOrganizationUnit).IsAssignableFrom(typeof(TEntity)))
                //{
                //    Expression<Func<TEntity, bool>> mayHaveOUFilter = e => ((IMayHaveOrganizationUnit)e).OrganizationUnitId == CurrentOUId || (((IMayHaveOrganizationUnit)e).OrganizationUnitId == CurrentOUId) == IsOUFilterEnabled;
                //    expression = expression == null ? mayHaveOUFilter : CombineExpressions(expression, mayHaveOUFilter);
                //}
            }
            return expression;
        }
    }
}
