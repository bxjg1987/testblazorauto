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
        public ITypeList<IDynamicAssociateEntityDefineProvider> DynamicAssociateEntityDefineProviders { get; } = new TypeList<IDynamicAssociateEntityDefineProvider>();

        public Func<DynamicAssociateEntityDefineGroupProviderContext, IDictionary<string, AssociateMapItem[]>> DynamicAssociateEntityDefineGroupProvider { get; set; }
        //public ITypeList<IDynamicAssociateEntityDefineGroupProvider> DynamicAssociateEntityDefineGroupProviders { get; } = new TypeList<IDynamicAssociateEntityDefineGroupProvider>();
    }

    public class AssociateMapItem
    {
        /// <summary>
        /// 关联的目标数据定义
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 关联粒度
        /// </summary>
        public AssociateGranularity AssociateGranularity { get; set; }
    }
}