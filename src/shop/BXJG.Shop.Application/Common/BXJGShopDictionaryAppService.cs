using Abp.Domain.Repositories;
using BXJG.GeneralTree;
using BXJG.Shop.Authorization;
using BXJG.Shop.Common.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BXJG.Shop.Common
{
    public class BXJGShopDictionaryAppService : GeneralTreeAppServiceBase<
             DictionaryDto,
             DictionaryEditDto,
             GeneralTreeGetTreeInput,
             GeneralTreeGetForSelectInput,
             DictionaryTreeNodeDto,
             GeneralTreeGetForSelectInput,
             DictionaryCombboxDto,
             GeneralTreeNodeMoveInput,
             BXJGShopDictionaryEntity,
             BXJGShopDictionaryManager>, IBXJGShopDictionaryAppService
    {
        public BXJGShopDictionaryAppService(
            IRepository<BXJGShopDictionaryEntity, long> repository,
            BXJGShopDictionaryManager organizationUnitManager)
            : base(repository, organizationUnitManager)
        {
            base.createPermissionName = BXJGShopPermissions.BXJGShopDictionaryCreate;
            base.updatePermissionName = BXJGShopPermissions.BXJGShopDictionaryUpdate;
            base.deletePermissionName = BXJGShopPermissions.BXJGShopDictionaryDelete;
            base.getPermissionName = BXJGShopPermissions.BXJGShopDictionary;
        }

        protected override async Task<IList<DictionaryCombboxDto>> GetNodesForSelectProjectionAsync(IQueryable<BXJGShopDictionaryEntity> query)
        {
            var q = query.Select(c => new DictionaryCombboxDto { ExtDataString = c.ExtensionData, DisplayText = c.DisplayName, Value = c.Id.ToString(), Icon = c.Icon });
            return await AsyncQueryableExecuter.ToListAsync(q);
        }

        protected override void OnGetTreeForSelectItem(BXJGShopDictionaryEntity entity, DictionaryTreeNodeDto node)
        {
            node.Icon = entity.Icon;
        }
    }
}
