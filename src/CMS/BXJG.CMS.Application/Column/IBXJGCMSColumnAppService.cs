using Abp.Application.Services.Dto;
using BXJG.Common.Dto;
using BXJG.GeneralTree;

namespace BXJG.CMS.Column
{
    public interface IBXJGCMSColumnAppService : IGeneralTreeAppServiceBase<
         ColumnDto,
         ColumnEditDto,
         ColumnEditDto,
         BatchOperationInputLong,
         GetAllInput,
         EntityDto<long>,
         GeneralTreeNodeMoveInput>
    { }
}
