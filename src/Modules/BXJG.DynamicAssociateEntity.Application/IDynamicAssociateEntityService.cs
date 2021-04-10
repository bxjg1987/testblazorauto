using Abp.Application.Services.Dto;
using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.DynamicAssociateEntity
{
    public interface IDynamicAssociateEntityService : ITransientDependency
    {
        Task<PagedResultDto<object>> GetAllAsync(string parentId, string keyword, string sorting, int skip, int maxcount);
        Task<IList<object>> GetAllNoPageAsync(string parentId, string keyword, string sorting);
        Task<IEnumerable<object>> GetAllByIdsAsync(params IEnumerable<object>[] ids);
        //Task<IEnumerable<string>> GetIdsByKeywordAsync(string parentId, string keyword);
    }
}
