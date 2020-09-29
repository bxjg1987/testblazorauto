using Abp.Domain.Repositories;
using BXJG.GeneralTree;
using BXJG.Shop.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Catalogue
{
    public class BXJGShopProductCategoryAppService : GeneralTreeAppServiceBase<ItemCategoryDto,
                                                                           ItemCategoryEditDto,
                                                                           ItemCategoryGetAllInput,
                                                                           ItemCategoryGetForSelectInput,
                                                                           ItemCategoryTreeNodeDto,
                                                                           ItemCategoryGetForSelectInput,
                                                                           ItemCategoryCombboxDto,
                                                                           GeneralTreeNodeMoveInput,
                                                                           ProductCategoryEntity,
                                                                           ProductCategoryManager>, IBXJGShopProductCategoryAppService
    {
        public BXJGShopProductCategoryAppService(IRepository<ProductCategoryEntity, long> ownRepository,
                                              ProductCategoryManager organizationUnitManager) : base(ownRepository,
                                                                                                  organizationUnitManager,
                                                                                                  BXJGShopPermissions.BXJGShopProductCategoryCreate,
                                                                                                  BXJGShopPermissions.BXJGShopProductCategoryUpdate,
                                                                                                  BXJGShopPermissions.BXJGShopProductCategoryDelete,
                                                                                                  BXJGShopPermissions.BXJGShopProductCategory)
        { }
    }
}
