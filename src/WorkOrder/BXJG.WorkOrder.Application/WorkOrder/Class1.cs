using Abp.Dependency;
using BXJG.DynamicAssociateEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrder
{
    public class Class1 : BXJG.DynamicAssociateEntity.IDynamicAssociateEntityDefineProvider
    {
        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public Dictionary<string, List<DynamicAssociateEntityDefine>> GetDefines(DynamicAssociateEntityDefineInitContext context)
        {
            return new Dictionary<string, List<DynamicAssociateEntityDefine>>
            {
                {
                    "workOrder",
                    new List<DynamicAssociateEntityDefine>
                    {
                        new DynamicAssociateEntityDefine
                        {
                            Name = "product",
                            DisplayName=null, ServiceType=typeof(class2)
                        }
                    }
                }
            };
        }
    }

    public class class2 : IDynamicAssociateEntityService
    {
        public Task<dynamic> GetAllAsync(IReadOnlyList<DynamicAssociateEntityDefine> defines, string parentId, string keyword)
        {
            throw new NotImplementedException();
        }

        public Task<dynamic> GetAllByIdsAsync(IReadOnlyList<DynamicAssociateEntityDefine> defines, string parentId, params string[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetIdsByKeywordAsync(string parentId, string keyword)
        {
            throw new NotImplementedException();
        }
    }
}
