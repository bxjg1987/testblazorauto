using BXJG.GeneralTree;
using BXJG.Shop.Common.Dto;

namespace BXJG.Shop.Common
{
    public interface IBXJGShopDictionaryAppService : IGeneralTreeAppServiceBase<
         DictionaryDto,
         DictionaryEditDto,
         GeneralTreeGetTreeInput,
         GeneralTreeGetForSelectInput,
         DictionaryTreeNodeDto,
         GeneralTreeGetForSelectInput,
         DictionaryCombboxDto,
         GeneralTreeNodeMoveInput>
    { }
}
