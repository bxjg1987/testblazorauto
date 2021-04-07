using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.DynamicAssociateEntity
{
    public interface IDynamicAssociateEntityService
    {
        Task<IEnumerable<object>> GetAllByIdsAsync(string parentId, params string[] ids);
        Task<IEnumerable<string>> GetIdsByKeywordAsync(string parentId, string keyword);
    }

    public interface IDynamicAssociateEntityService2
    {
        Task<PagedResultDto<object>> GetAllAsync(string parentId, string keyword, string sorting, int skip, int maxcount);
    }
}
