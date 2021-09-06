using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.GoodsInfo
{
    public interface IGoodsInfoExtensionRepository<TGoodsInfoExtensionEntity> : IRepository<TGoodsInfoExtensionEntity, long>
        where TGoodsInfoExtensionEntity : Entity<long>// TGoodsInfoExtensionEntity: GoodsInfoExtensionEntity
    {
    }
}
