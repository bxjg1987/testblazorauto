using BXJG.GeneralTree;
using ZLJ.BaseInfo;

namespace ZLJ.BaseInfo.Administrative
{
    public interface IBXJGBaseInfoAdministrativeAppService : IGeneralTreeAppServiceBase<
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
