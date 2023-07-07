using Abp.Domain.Repositories;
using BXJG.Utils.GeneralTree;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using ZLJ.App.Admin.BaseInfo.Administrative.Dto;
using ZLJ.App.Admin.BaseInfo;
using ZLJ.Authorization;
using Abp.Application.Services.Dto;
using BXJG.Common.Dto;
using ZLJ.BaseInfo.Administrative;

namespace ZLJ.App.Admin.BaseInfo.Administrative
{
    [AbpAuthorize]
    public class BXJGBaseInfoAdministrativeAppService : GeneralTreeAppServiceBase<
        AdministrativeDto,
        AdministrativeEditDto,
        AdministrativeEditDto,
        BatchOperationInputLong,
        GeneralTreeGetTreeInput,
        EntityDto<long>,
        GeneralTreeNodeMoveInput,
        AdministrativeEntity,
        AdministrativeManager>, IBXJGBaseInfoAdministrativeAppService
    {
        public BXJGBaseInfoAdministrativeAppService(
            IRepository<AdministrativeEntity, long> repository,
            AdministrativeManager organizationUnitManager)
            : base(repository, organizationUnitManager)
        {
            createPermissionName = PermissionNames.BXJGBaseInfoAdministrativeCreate;
            updatePermissionName = PermissionNames.BXJGBaseInfoAdministrativeUpdate;
            deletePermissionName = PermissionNames.BXJGBaseInfoAdministrativeDelete;
            getPermissionName = PermissionNames.BXJGBaseInfoAdministrative;
        }
    }
}