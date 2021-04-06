using BXJG.DynamicAssociateEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.DynamicAssociateEntity
{
    public class DynamicAssociateProductService : IDynamicAssociateEntityService
    {
        public Task<IDictionary<string, object>> GetAllAsync(DynamicAssociateEntityDefine defines, string parentId, string keyword)
        {
            throw new NotImplementedException();
        }

        public Task<IDictionary<string, object>> GetAllByIdsAsync(DynamicAssociateEntityDefine defines, string parentId, params string[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetIdsByKeywordAsync(string parentId, string keyword)
        {
            throw new NotImplementedException();
        }
    }
}
