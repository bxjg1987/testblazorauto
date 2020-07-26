using BXJG.GeneralTree;
using ZLJ.BaseInfo;

namespace ZLJ.BaseInfo.Administrative
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
