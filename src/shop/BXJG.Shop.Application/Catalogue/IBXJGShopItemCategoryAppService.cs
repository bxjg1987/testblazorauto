using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Catalogue
{
    /// <summary>
    /// 后端管理对商品分类的操作
    /// </summary>
    public interface IBXJGShopItemCategoryAppService : IGeneralTreeAppServiceBase<ItemCategoryDto,
                                                                                  ItemCategoryEditDto,
                                                                                  ItemCategoryGetAllInput,
                                                                                  ItemCategoryGetForSelectInput,
                                                                                  ItemCategoryTreeNodeDto,
                                                                                  ItemCategoryGetForSelectInput,
                                                                                  ItemCategoryCombboxDto,
                                                                                  GeneralTreeNodeMoveInput>
    { }
}
