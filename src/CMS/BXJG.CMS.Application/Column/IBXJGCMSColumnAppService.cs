using BXJG.GeneralTree;

namespace BXJG.CMS.Column
{
    public interface IBXJGCMSColumnAppService : IGeneralTreeAppServiceBase<
         ColumnDto,
         ColumnEditDto,
         GetAllInput,
         GetForSelectInput,
         ColumnTreeNodeDto,
         GetForSelectInput,
         ColumnCombboxDto,
         GeneralTreeNodeMoveInput>
    { }
}
