using Abp.Domain.Entities;
using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.GoodsInfo.EFCore.Repository
{
    /// <summary>
    /// 物品信息仓储
    /// 你引用此模块时应提供一个实现类，并指明泛型参数TDbContext、TEntity的具体类型
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public abstract class GoodsInfoRepository<TDbContext, TEntity> : EfCoreRepositoryBase<TDbContext, TEntity, long>, IGoodsInfoRepository<TEntity>
        where TDbContext : DbContext
        where TEntity : class, IGoodsInfoEntity
    {
        public GoodsInfoRepository(IDbContextProvider<TDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
