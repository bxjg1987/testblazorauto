using BXJG.Utils.Application.Share.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.PSI.MasterData.Application.Share.Product
{
    /// <summary>
    /// 产品分类提供者应用服务接口
    /// 用于提供产品分类的树形数据选择功能
    /// </summary>
    public interface IProductCategoryProviderAppService : IGeneralTreeProviderBaseAppService<GeneralTreeGetForSelectInput,
                                                                                           ProductCategorySelectDto>
    {
    }
}
