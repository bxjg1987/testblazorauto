using BXJG.GeneralTree;
using BXJG.Shop.Catalogue.Dto;

namespace BXJG.Shop.Catalogue
{
   public  interface IItemDynamicFieldAppService : IGeneralTreeAppServiceBase<
        ItemDynamicFieldDto,
        ItemDynamicFieldEditDto,
        GeneralTreeGetTreeInput,
        GeneralTreeGetForSelectInput,
        ItemDynamicFieldTreeNodeDto,
        GeneralTreeGetForSelectInput,
        GeneralTreeComboboxDto,
        GeneralTreeNodeMoveInput>
    { }
}
