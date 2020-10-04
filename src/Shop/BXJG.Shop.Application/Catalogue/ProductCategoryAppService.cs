using Abp.Domain.Repositories;
using BXJG.GeneralTree;
using BXJG.Shop.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Catalogue
{
    public class ProductCategoryAppService : GeneralTreeAppServiceBase<ProductCategoryDto,
                                                                           ProductCategoryEditDto,
                                                                           ProductCategoryGetAllInput,
                                                                           ProductCategoryGetForSelectInput,
                                                                           ProductCategoryTreeNodeDto,
                                                                           ProductCategoryGetForSelectInput,
                                                                           ProductCategoryCombboxDto,
                                                                           GeneralTreeNodeMoveInput,
                                                                           ProductCategoryEntity,
                                                                           ProductCategoryManager>, IProductCategoryAppService
    {
        public ProductCategoryAppService(IRepository<ProductCategoryEntity, long> ownRepository,
                                              ProductCategoryManager organizationUnitManager) : base(ownRepository,
                                                                                                  organizationUnitManager,
                                                                                                  PermissionNames.ProductCategoryCreate,
                                                                                                  PermissionNames.ProductCategoryUpdate,
                                                                                                  PermissionNames.ProductCategoryDelete,
                                                                                                  PermissionNames.ProductCategory)
        { }
    }
}
