using Abp.Domain.Repositories;
using BXJG.GeneralTree;
using BXJG.Shop.Authorization;
using BXJG.Shop.Common.Dto;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZLJ.Authorization;
using ZLJ.BaseInfo;

namespace ZLJ.Administrative
{
    public class AdministrativeAppService : GeneralTreeAppServiceBase<
             AdministrativeDto,
             AdministrativeEditDto,
             GeneralTreeGetTreeInput,
             GeneralTreeGetForSelectInput,
             AdministrativeTreeNodeDto,
             GeneralTreeGetForSelectInput,
             AdministrativeCombboxDto,
             GeneralTreeNodeMoveInput,
             AdministrativeEntity,
             AdministrativeManager>, IAdministrativeAppService
    {
        public AdministrativeAppService(
            IRepository<AdministrativeEntity, long> repository,
            AdministrativeManager organizationUnitManager)
            : base(repository, organizationUnitManager)
        {
            base.createPermissionName = PermissionNames.AdministratorBaseInfoAdministrativeAdd;
            base.updatePermissionName = PermissionNames.AdministratorBaseInfoAdministrativeUpdate;
            base.deletePermissionName = PermissionNames.AdministratorBaseInfoAdministrativeDelete;
            base.getPermissionName = PermissionNames.AdministratorBaseInfoAdministrative;
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
