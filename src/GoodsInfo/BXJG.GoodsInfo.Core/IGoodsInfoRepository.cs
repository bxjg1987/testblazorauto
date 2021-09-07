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
    /// 推荐继承BXJG.GoodsInfo.EFCore.Repository.GoodsInfoRepository，它继承此接口
    /// 特殊情况你可以自己实现此接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IGoodsInfoRepository<TEntity> : IRepository<TEntity, long>
        where TEntity : Entity<long>
    {
    }
}
