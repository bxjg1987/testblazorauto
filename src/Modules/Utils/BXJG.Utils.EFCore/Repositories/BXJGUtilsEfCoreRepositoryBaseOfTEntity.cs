using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BXJG.Utils.EFCore.Repositories
{
    /// <summary>
    /// 支持数据权限的仓储基类（int 主键）
    /// </summary>
    /// <typeparam name="TDbContext">DbContext类型</typeparam>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public class BXJGUtilsEfCoreRepositoryBase<TDbContext, TEntity>
        : BXJGUtilsEfCoreRepositoryBase<TDbContext, TEntity, int>
        , IRepository<TEntity>
        where TDbContext : DbContext
        where TEntity : class, IEntity<int>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContextProvider">DbContext提供者</param>
        public BXJGUtilsEfCoreRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
