using Abp.Domain.Repositories;
using BXJG.CMS.Authorization;
using BXJG.GeneralTree;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BXJG.CMS.Column
{
    /// <summary>
    /// 后台管理管理CMS栏目的服务
    /// </summary>
    /// <typeparam name="TDataDictionary"></typeparam>
    public class BXJGCMSColumnAppService<TDataDictionary> : GeneralTreeAppServiceBase<ColumnDto,
                                                                                      ColumnEditDto,
                                                                                      GetAllInput,
                                                                                      GetForSelectInput,
                                                                                      ColumnTreeNodeDto,
                                                                                      GetForSelectInput,
                                                                                      ColumnCombboxDto,
                                                                                      GeneralTreeNodeMoveInput,
                                                                                      ColumnEntity<TDataDictionary>,
                                                                                      ColumnManager<TDataDictionary>>, IBXJGCMSColumnAppService
        where TDataDictionary : GeneralTreeEntity<TDataDictionary>
    {
        public BXJGCMSColumnAppService(IRepository<ColumnEntity<TDataDictionary>, long> repository, ColumnManager<TDataDictionary> organizationUnitManager)
            : base(repository,
                   organizationUnitManager,
                   BXJGCMSPermissions.ColumnCreate,
                   BXJGCMSPermissions.ColumnUpdate,
                   BXJGCMSPermissions.ColumnDelete,
                   BXJGCMSPermissions.Column)
        {
            //base.createPermissionName = BXJGCMSPermissions.ColumnCreate;
            //base.updatePermissionName = BXJGCMSPermissions.ColumnUpdate;
            //base.deletePermissionName = BXJGCMSPermissions.ColumnDelete;
            //base.getPermissionName = BXJGCMSPermissions.Column;
        }

        //protected override async Task<IList<ColumnCombboxDto>> ComboboxProjectionAsync(IQueryable<ColumnEntity<TDataDictionary>> query)
        //{
        //    var q = query.Select(c => new ColumnCombboxDto
        //    {
        //        ExtDataString = c.ExtensionData,
        //        DisplayText = c.DisplayName,
        //        Value = c.Id.ToString(),
        //        Icon = c.Icon,
        //        ColumnType = c.ColumnType,
        //        ContentTypeId = c.ContentTypeId
        //    });
        //    return await AsyncQueryableExecuter.ToListAsync(q);
        //}

        //protected override void ComboTreeMap(ColumnEntity<TDataDictionary> entity, ColumnTreeNodeDto node)
        //{
        //    node.Icon = entity.Icon;
        //    //node.IsSysDefine = entity.IsSysDefine;
        //    //node.IsTree = entity.IsTree;
        //}
    }
}
