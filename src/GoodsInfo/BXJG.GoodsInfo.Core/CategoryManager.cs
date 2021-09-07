using Abp.Domain.Repositories;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.GoodsInfo
{
    public class CategoryManager : GeneralTreeManager<CategoryEntity>
    {
        public CategoryManager(IRepository<CategoryEntity, long> repository) : base(repository)
        {
        }
    }
}
