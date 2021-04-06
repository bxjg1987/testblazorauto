using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.DynamicAssociateEntity
{
    public interface IDynamicAssociateEntityService
    {
        Task<IDictionary<string, object>> GetAllAsync(DynamicAssociateEntityDefine defines, string parentId, string keyword);
        Task<IDictionary<string, object>> GetAllByIdsAsync(DynamicAssociateEntityDefine defines, string parentId, params string[] ids);
        Task<IEnumerable<string>> GetIdsByKeywordAsync(string parentId, string keyword);
    }
}
