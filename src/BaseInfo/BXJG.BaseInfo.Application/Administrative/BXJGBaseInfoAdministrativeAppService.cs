using Abp.Domain.Repositories;
using BXJG.BaseInfo.Authorization;
using BXJG.GeneralTree;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZLJ.BaseInfo;

namespace ZLJ.BaseInfo.Administrative
{
    public class BXJGBaseInfoAdministrativeAppService : GeneralTreeAppServiceBase<
             AdministrativeDto,
             AdministrativeEditDto,
             GeneralTreeGetTreeInput,
             GeneralTreeGetForSelectInput,
             AdministrativeTreeNodeDto,
             GeneralTreeGetForSelectInput,
             AdministrativeCombboxDto,
             GeneralTreeNodeMoveInput,
             AdministrativeEntity,
             AdministrativeManager>, IBXJGBaseInfoAdministrativeAppService
    {
        public BXJGBaseInfoAdministrativeAppService(
            IRepository<AdministrativeEntity, long> repository,
            AdministrativeManager organizationUnitManager)
            : base(repository, organizationUnitManager)
        {
            base.createPermissionName = BXJGBaseInfoPermissionNames.BXJGBaseInfoAdministrativeCreate;
            base.updatePermissionName = BXJGBaseInfoPermissionNames.BXJGBaseInfoAdministrativeUpdate;
            base.deletePermissionName = BXJGBaseInfoPermissionNames.BXJGBaseInfoAdministrativeDelete;
            base.getPermissionName = BXJGBaseInfoPermissionNames.BXJGBaseInfoAdministrative;
        }
        //protected override void GetAllMap(AdministrativeEntity entity, AdministrativeDto dto)
        //{
        //    dto.Level = entity.Level;
        //}
        //protected override async Task<IList<AdministrativeCombboxDto>> ComboboxProjectionAsync(IQueryable<AdministrativeEntity> query)
        //{
        //   return await query
        //        .Select(c => new AdministrativeCombboxDto { 
        //            ExtDataString = c.ExtensionData,
        //            DisplayText = c.DisplayName, 
        //            Value = c.Id.ToString(),
        //            Level = c.Level })
        //        .ToListAsync();
        //}

        //protected override void ComboTreeMap(AdministrativeEntity entity, AdministrativeTreeNodeDto node)
        //{
        //    node.Level = entity.Level;
        //}
    }
}
