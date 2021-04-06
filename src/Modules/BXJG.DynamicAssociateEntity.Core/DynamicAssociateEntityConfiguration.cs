using Abp.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.DynamicAssociateEntity
{
    /// <summary>
    /// 动态关联实体配置类
    /// </summary>
    public class DynamicAssociateEntityConfiguration
    {
        public Func<IEnumerable<DynamicAssociateEntityDefine>> DynamicAssociateEntityDefineProvider { get; set; }

        public ITypeList<IDynamicAssociateEntityDefineGroupProvider> DynamicAssociateEntityDefineGroupProviders { get; } = new TypeList<IDynamicAssociateEntityDefineGroupProvider>();
       
    }
}