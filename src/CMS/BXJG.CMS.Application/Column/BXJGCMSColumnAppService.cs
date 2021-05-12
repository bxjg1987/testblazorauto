using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using BXJG.CMS.Authorization;
using BXJG.Common.Dto;
using BXJG.GeneralTree;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BXJG.CMS.Column
{
  
    public class BXJGCMSColumnAppService : GeneralTreeAppServiceBase<ColumnDto,
                                                                     ColumnEditDto,
                                                                     ColumnEditDto,
                                                                     BatchOperationInputLong,
                                                                     GetAllInput,
                                                                     EntityDto<long>,
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
        protected override async ValueTask<IQueryable<ColumnEntity>> GetAllFilteredAsync(GetAllInput input, string parentCode, IDictionary<string, object> context = null)
        {
            var q = await base.GetAllFilteredAsync(input, parentCode, context);
            return q.Include(c => c.ContentType);
        }
    }
}
