using Abp.Domain.Repositories;
using BXJG.Utils.GeneralTree;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Application.Services.Dto;

using ZLJ.Application.Admin.Authorization.Permissions;
using BXJG.Utils.Application.Share.GeneralTree;
using ZLJ.Application.Share.Authorization.Permissions;
using ZLJ.Core.Administrative;
using BXJG.Common.Contracts;
using ZLJ.Application.Share.Administrative;
using ZLJ.Application.Admin;
using BXJG.Utils.Application.GeneralTree;

namespace ZLJ.Application.BaseInfo.Administrative
{
    // [AbpAuthorize]
    public class BXJGBaseInfoAdministrativeAppService : AdminTreeCrudBaseAppService<AdministrativeEntity,
                                                                                    AdministrativeDto,
                                                                                    AdministrativeEditDto,
                                                                                    AdministrativeEditDto,
                                                                                    GetAdministrativeInput>//, IBXJGBaseInfoAdministrativeAppService
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

        protected override string GetPermissionName => PermissionNames.BXJGBaseInfoAdministrative;
        protected override string CreatePermissionName => PermissionNames.BXJGBaseInfoAdministrativeCreate;
        protected override string UpdatePermissionName => PermissionNames.BXJGBaseInfoAdministrativeUpdate;
        protected override string DeletePermissionName => PermissionNames.BXJGBaseInfoAdministrativeDelete;
        //public override Task<AdministrativeDto> CreateAsync(AdministrativeEditDto input)
        //{
        //    return base.CreateAsync(input);
        //}

       // DataDictionaryAppService
    }
}