using BXJG.Utils.Application.Share.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.PSI.MasterData.Application.Share.Product
{
    /// <summary>
    /// 商品分类应用服务接口
    /// </summary>
    public interface IProductCategoryAppService : IGeneralTreeBaseAppService<ProductCategoryDto,
                                                                           ProductCategoryEditDto,
                                                                           ProductCategoryEditDto,
                                                                           ProductCategoryCondition>
    {
    }
}
