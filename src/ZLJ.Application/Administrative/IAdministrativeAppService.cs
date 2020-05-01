using BXJG.GeneralTree;
using BXJG.Shop.Common.Dto;
using ZLJ.BaseInfo;

namespace ZLJ.Administrative
{
    public interface IAdministrativeAppService : IGeneralTreeAppServiceBase<
        AdministrativeDto,
        AdministrativeEditDto,
        GeneralTreeGetTreeInput,
        GeneralTreeGetForSelectInput,
        AdministrativeTreeNodeDto,
        GeneralTreeGetForSelectInput,
        AdministrativeCombboxDto,
        GeneralTreeNodeMoveInput>
    { }
}
