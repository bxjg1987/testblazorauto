using Abp.Domain.Repositories;
using BXJG.CMS.Authorization;
using BXJG.GeneralTree;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BXJG.CMS.Column
{
    /// <summary>
    /// 后台管理管理CMS栏目的服务
    /// </summary>
    /// <typeparam name="TDataDictionary"></typeparam>
    public class BXJGCMSColumnAppService : GeneralTreeAppServiceBase<ColumnDto,
                                                                                      ColumnEditDto,
                                                                                      GetAllInput,
                                                                                      GetForSelectInput,
                                                                                      ColumnTreeNodeDto,
                                                                                      GetForSelectInput,
                                                                                      ColumnCombboxDto,
                                                                                      GeneralTreeNodeMoveInput,
                                                                                      ColumnEntity,
                                                                                      ColumnManager>, IBXJGCMSColumnAppService
        
    {
        public BXJGCMSColumnAppService(IRepository<ColumnEntity, long> repository, ColumnManager manager)
            : base(repository,
                   manager,
                   BXJGCMSPermissions.ColumnCreate,
                   BXJGCMSPermissions.ColumnUpdate,
                   BXJGCMSPermissions.ColumnDelete,
                   BXJGCMSPermissions.Column)
        { }

        protected override IQueryable<ColumnEntity> GetAllFiltered(GetAllInput q, string parentCode)
        {
            return base.GetAllFiltered(q, parentCode).Include(c=>c.ContentType);
        }
    }
}
