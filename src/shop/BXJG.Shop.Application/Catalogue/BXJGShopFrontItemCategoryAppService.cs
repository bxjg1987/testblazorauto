using Abp.Domain.Repositories;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.Catalogue
{
    /// <summary>
    /// 前台用户获取商品分类信息的接口
    /// 不检查登录和授权的
    /// </summary>
    public class BXJGShopFrontItemCategoryAppService : UnAuthGeneralTreeAppServiceBase<ItemCategoryGetForSelectInput,
                                                                                       ItemCategoryTreeNodeDto,
                                                                                       ItemCategoryGetForSelectInput,
                                                                                       ItemCategoryCombboxDto,
                                                                                       ItemCategoryEntity,
                                                                                       ItemCategoryManager>, IBXJGShopFrontItemCategoryAppService
    {
        public BXJGShopFrontItemCategoryAppService(IRepository<ItemCategoryEntity, long> ownRepository,
                                                   ItemCategoryManager organizationUnitManager,
                                                   string allTextForSearch = "不限", 
                                                   string allTextForForm = "请选择") : base(ownRepository,
                                                                                            organizationUnitManager,
                                                                                            allTextForSearch,
                                                                                            allTextForForm)
        {
        }
    }
}
