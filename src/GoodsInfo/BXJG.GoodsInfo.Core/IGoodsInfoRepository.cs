using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.GoodsInfo
{
    /// <summary>
    /// 基础物品类型的仓储接口
    /// 你应该继承BXJG.GoodsInfo.EFCore.Repository.GoodsInfoRepository<TDbContext>
    /// </summary>
    public interface IGoodsInfoRepository<TEntity> : IRepository<TEntity, long>
        where TEntity : Entity<long>
    {
    }
}
