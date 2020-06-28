using Abp.Domain.Repositories;
using BXJG.GeneralTree;
using BXJG.Shop.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Catalogue
{
    public class BXJGShopItemCategoryAppService : GeneralTreeAppServiceBase<ItemCategoryDto,
                                                                           ItemCategoryEditDto,
                                                                           ItemCategoryGetAllInput,
                                                                           ItemCategoryGetForSelectInput,
                                                                           ItemCategoryTreeNodeDto,
                                                                           ItemCategoryGetForSelectInput,
                                                                           ItemCategoryCombboxDto,
                                                                           GeneralTreeNodeMoveInput,
                                                                           ItemCategoryEntity,
                                                                           ItemCategoryManager>, IBXJGShopItemCategoryAppService
    {
        public BXJGShopItemCategoryAppService(IRepository<ItemCategoryEntity, long> ownRepository,
                                              ItemCategoryManager organizationUnitManager) : base(ownRepository,
                                                                                                  organizationUnitManager,
                                                                                                  BXJGShopPermissions.BXJGShopItemCategoryCreate,
                                                                                                  BXJGShopPermissions.BXJGShopItemCategoryUpdate,
                                                                                                  BXJGShopPermissions.BXJGShopItemCategoryDelete,
                                                                                                  BXJGShopPermissions.BXJGShopItemCategory)
        { }
    }
}
