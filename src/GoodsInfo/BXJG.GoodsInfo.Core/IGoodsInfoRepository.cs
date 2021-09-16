using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.GoodsInfo
{
    //模块调用方目前需要手动ioc注册IGoodsInfoRepository的实现类，且暴露服务为IGoodsInfoRepository
    //后期模块内部实现abp基于约定的自动注册，那时模块调用方只需要实现此接口接口

    // 目前固定死主键为long类型，若有需要在此基础上在抽象出一个接口，允许泛型的主键类型

    /// <summary>
    /// 抽象的物品仓储接口
    /// 推荐继承BXJG.GoodsInfo.EFCore.Repository.GoodsInfoRepository，它继承此接口
    /// 特殊情况你可以自己实现此接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IGoodsInfoRepository<TEntity> : IRepository<TEntity, long>
        where TEntity : class, IGoodsInfoEntity
    {
    }
}
