using Abp.Domain.Repositories;
using BXJG.GeneralTree;
using BXJG.Shop.Authorization;
using BXJG.Shop.Catalogue.Dto;

namespace BXJG.Shop.Catalogue
{
    public class ItemDynamicFieldAppService : GeneralTreeAppServiceBase<
             ItemDynamicFieldDto,
             ItemDynamicFieldEditDto,
             GeneralTreeGetTreeInput,
             GeneralTreeGetForSelectInput,
             ItemDynamicFieldTreeNodeDto,
             GeneralTreeGetForSelectInput,
             GeneralTreeComboboxDto,
             GeneralTreeNodeMoveInput,
             ItemDynamicFieldEntity,
             ItemDynamicFieldManager>, IItemDynamicFieldAppService
    {
        public ItemDynamicFieldAppService(
            IRepository<ItemDynamicFieldEntity, long> repository,
            ItemDynamicFieldManager organizationUnitManager)
            : base(repository, organizationUnitManager)
        {
            base.createPermissionName = BXJGShopPermissions.BXJGShopCatalogueItemDynamicFieldCreate;
            base.updatePermissionName = BXJGShopPermissions.BXJGShopCatalogueItemDynamicFieldUpdate;
            base.deletePermissionName = BXJGShopPermissions.BXJGShopCatalogueItemDynamicFieldDelete;
            base.getPermissionName = BXJGShopPermissions.BXJGShopCatalogueItemDynamicField;
        }
    }
}
