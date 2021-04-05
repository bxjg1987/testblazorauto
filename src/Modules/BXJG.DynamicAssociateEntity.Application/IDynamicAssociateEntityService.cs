using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.DynamicAssociateEntity
{
    public interface IDynamicAssociateEntityService
    {
        Task<dynamic> GetAllAsync(IReadOnlyList<DynamicAssociateEntityDefine> defines, string parentId, string keyword);
        Task<dynamic> GetAllByIdsAsync(IReadOnlyList<DynamicAssociateEntityDefine> defines, string parentId, params string[] ids);
        Task<IEnumerable<string>> GetIdsByKeywordAsync(string parentId, string keyword);
    }
}
