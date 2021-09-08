using Abp.Domain.Repositories;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.GoodsInfo
{
    public class GoodsInfoCategoryManager : GeneralTreeManager<GoodsInfoCategoryEntity>
    {
        public GoodsInfoCategoryManager(IRepository<GoodsInfoCategoryEntity, long> repository) : base(repository)
        {
        }
    }
}
