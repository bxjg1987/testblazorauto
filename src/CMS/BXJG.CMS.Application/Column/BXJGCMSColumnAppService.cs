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
        public BXJGCMSColumnAppService(IRepository<ColumnEntity<TDataDictionary>, long> repository, ColumnManager<TDataDictionary> manager)
            : base(repository,
                   manager,
                   BXJGCMSPermissions.ColumnCreate,
                   BXJGCMSPermissions.ColumnUpdate,
                   BXJGCMSPermissions.ColumnDelete,
                   BXJGCMSPermissions.Column)
        { }

        protected override IQueryable<ColumnEntity<TDataDictionary>> GetAllFiltered(GetAllInput q, string parentCode)
        {
            return base.GetAllFiltered(q, parentCode).Include(c=>c.ContentType);
        }
    }
}
