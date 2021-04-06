using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;

namespace BXJG.DynamicAssociateEntity
{
    public class DynamicAssociateEntityDefineManager : DomainService
    {
        protected readonly DynamicAssociateEntityConfiguration dynamicAssociateEntityConfiguration;

        public DynamicAssociateEntityDefineManager(DynamicAssociateEntityConfiguration dynamicAssociateEntityConfiguration)
        {
            this.dynamicAssociateEntityConfiguration = dynamicAssociateEntityConfiguration;
        }

        public IReadOnlyList<DynamicAssociateEntityDefine> Get(string key)
        {
            throw new NotImplementedException();
           // return dynamicAssociateEntityConfiguration.DynamicAssociateEntityDefines[key].AsReadOnly();
        }
    }
}
