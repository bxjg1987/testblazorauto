using Abp.Domain.Repositories;
using BXJG.Utils.GeneralTree;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using ZLJ.Application.Admin.BaseInfo.Administrative.Dto;
using ZLJ.Application.Admin.BaseInfo;
using Abp.Application.Services.Dto;
using BXJG.Common.Dto;
using ZLJ.Core.BaseInfo.Administrative;
using ZLJ.Application.Admin.Authorization.Permissions;
using BXJG.Utils.Application.Share.GeneralTree;
using ZLJ.Application.Share.Authorization.Permissions;

namespace ZLJ.Application.Admin.BaseInfo.Administrative
{
    // [AbpAuthorize]
    public class BXJGBaseInfoAdministrativeAppService : AdminTreeCrudBaseAppService<AdministrativeEntity,
                                                                                    AdministrativeDto,
                                                                                    AdministrativeEditDto,
                                                                                    AdministrativeEditDto,
                                                                                    GeneralTreeGetTreeInput,
                                                                                    BatchOperationInputLong>//, IBXJGBaseInfoAdministrativeAppService
    {
        //public BXJGBaseInfoAdministrativeAppService(
        //    IRepository<AdministrativeEntity, long> repository,
        //    GeneralTreeManager<AdministrativeEntity> organizationUnitManager)
        //    : base(repository, organizationUnitManager)
        //{
        //    createPermissionName = PermissionNames.BXJGBaseInfoAdministrativeCreate;
        //    updatePermissionName = PermissionNames.BXJGBaseInfoAdministrativeUpdate;
        //    deletePermissionName = PermissionNames.BXJGBaseInfoAdministrativeDelete;
        //    getPermissionName = PermissionNames.BXJGBaseInfoAdministrative;
        //}

        public override string getPermissionName => PermissionNames.BXJGBaseInfoAdministrative;
        public override string createPermissionName => PermissionNames.BXJGBaseInfoAdministrativeCreate;
        public override string updatePermissionName => PermissionNames.BXJGBaseInfoAdministrativeUpdate;
        public override string deletePermissionName => PermissionNames.BXJGBaseInfoAdministrativeDelete;
        //public override Task<AdministrativeDto> CreateAsync(AdministrativeEditDto input)
        //{
        //    return base.CreateAsync(input);
        //}
    }
}