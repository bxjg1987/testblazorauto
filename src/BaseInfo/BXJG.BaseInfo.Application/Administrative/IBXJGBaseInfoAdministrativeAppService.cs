using Abp.Application.Services.Dto;
using BXJG.GeneralTree;
using ZLJ.BaseInfo;

namespace ZLJ.BaseInfo.Administrative
{
    public interface IBXJGBaseInfoAdministrativeAppService : IGeneralTreeAppServiceBase<
        AdministrativeDto,
        AdministrativeEditDto,
        AdministrativeEditDto,
        BXJG.Common.Dto.BatchOperationInputLong,
        GeneralTreeGetTreeInput,
        EntityDto<long>,
        GeneralTreeNodeMoveInput>
    { }
}
