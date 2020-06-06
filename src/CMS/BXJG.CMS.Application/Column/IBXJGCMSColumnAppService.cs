using BXJG.GeneralTree;

namespace BXJG.CMS.Column
{
    public interface IBXJGCMSColumnAppService : IGeneralTreeAppServiceBase<
         ColumnDto,
         ColumnEditDto,
         GeneralTreeGetTreeInput,
         GeneralTreeGetForSelectInput,
         ColumnTreeNodeDto,
         GeneralTreeGetForSelectInput,
         ColumnCombboxDto,
         GeneralTreeNodeMoveInput>
    { }
}
