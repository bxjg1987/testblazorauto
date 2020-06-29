using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Catalogue
{
    /// <summary>
    /// 前端用户对商品分类的功能
    /// </summary>
    public interface IBXJGShopFrontItemCategoryAppService : IUnAuthGeneralTreeAppServiceBase<
        ItemCategoryGetForSelectInput,
        ItemCategoryTreeNodeDto,
        ItemCategoryGetForSelectInput,
        ItemCategoryCombboxDto>
    { }
}
