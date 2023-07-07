using ZLJ.App.Admin.BaseInfo.Administrative.Dto;
using BXJG.Utils.GeneralTree;
using ZLJ.App.Admin.BaseInfo;
using BXJG.Common.Dto;
using Abp.Application.Services.Dto;

namespace ZLJ.App.Admin.BaseInfo.Administrative
{
    public interface IBXJGBaseInfoAdministrativeAppService : IGeneralTreeAppServiceBase<
        AdministrativeDto,
        AdministrativeEditDto,
        AdministrativeEditDto,
        BatchOperationInputLong,
        GeneralTreeGetTreeInput,
        EntityDto<long>,
        GeneralTreeNodeMoveInput>
    { }
}
