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
        /// <summary>
        /// 初始化全局的、可动态关联的数据定义的提供器
        /// </summary>
        public ITypeList<IDynamicAssociateEntityDefineProvider> DynamicAssociateEntityDefineProviders { get; } = new TypeList<IDynamicAssociateEntityDefineProvider>();
        /// <summary>
        /// 配置允许哪些数据可以动态关联到哪些数据的映射关系
        /// </summary>
        public Func<DynamicAssociateEntityDefineGroupProviderContext, IDictionary<string, AssociateMapItem[]>> DynamicAssociateEntityDefineGroupProvider { get; set; }
    }

    public class AssociateMapItem
    {
        /// <summary>
        /// 关联的目标数据定义的名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 关联粒度，所有行统一的关联到一种目标数据类型；或每一行关联到不同的目标数据类型；
        /// </summary>
        public AssociateGranularity AssociateGranularity { get; set; }
        /// <summary>
        /// 是否必须关联
        /// </summary>
        public bool Required { get; set; }
    }
}