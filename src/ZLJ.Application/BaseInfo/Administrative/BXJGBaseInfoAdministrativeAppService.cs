using Abp.Domain.Repositories;
using BXJG.Utils.GeneralTree;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using ZLJ.App.Admin.BaseInfo.Administrative.Dto;
using ZLJ.App.Admin.BaseInfo;
using Abp.Application.Services.Dto;
using BXJG.Common.Dto;
using ZLJ.BaseInfo.Administrative;
using ZLJ.App.Admin.Authorization.Permissions;

namespace ZLJ.App.Admin.BaseInfo.Administrative
{
    // [AbpAuthorize]
    public class BXJGBaseInfoAdministrativeAppService : AdminTreeCrudBaseAppService<AdministrativeDto,
                                                                                    AdministrativeEditDto,
                                                                                    AdministrativeEditDto,
                                                                                    GeneralTreeGetTreeInput,
                                                                                    AdministrativeEntity>//, IBXJGBaseInfoAdministrativeAppService
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