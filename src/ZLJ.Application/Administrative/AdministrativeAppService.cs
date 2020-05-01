using Abp.Domain.Repositories;
using BXJG.GeneralTree;
using BXJG.Shop.Authorization;
using BXJG.Shop.Common.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            //base.createPermissionName = BXJGShopPermissions.BXJGShopAdministrativeCreate;
            //base.updatePermissionName = BXJGShopPermissions.BXJGShopAdministrativeUpdate;
            //base.deletePermissionName = BXJGShopPermissions.BXJGShopAdministrativeDelete;
            //base.getPermissionName = BXJGShopPermissions.BXJGShopAdministrative;
        }

        protected override async Task<IList<AdministrativeCombboxDto>> GetNodesForSelectProjectionAsync(IQueryable<AdministrativeEntity> query)
        {
            var q = query.Select(c => new AdministrativeCombboxDto { ExtDataString = c.ExtensionData, DisplayText = c.DisplayName, Value = c.Id.ToString(), Level = c.Level });
            return await AsyncQueryableExecuter.ToListAsync(q);
        }

        protected override void OnGetTreeForSelectItem(AdministrativeEntity entity, AdministrativeTreeNodeDto node)
        {
            node.Level = entity.Level;
        }
    }
}
