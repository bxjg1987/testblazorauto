using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.DynamicProperty
{
    public class DynamicPropertyDto
    {
        public string Name { get; set; }    
        public string DisplayName { get; set; }

        public object Value { get; set; }
    }

    /// <summary>
    /// 后台管理合同属性变更显示模型
    /// </summary>
    public class PropertyChangeRecordDto
    {
        /// <summary>
        /// 属性名
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// 属性显示名
        /// </summary>
        public string PropertyDisplayName { get; set; } //=> PropertyMap.GetDisplayName(PropertyName, !OrderItemId.HasValue);
        /// <summary>
        /// 原始值
        /// </summary>
        public string OriginalValue { get; set; }
        /// <summary>
        /// 当前值
        /// </summary>
        public string CurrentValue { get; set; }
        /// <summary>
        /// 差值
        /// </summary>
        public string DifferenceValue { get; set; }
    }
}
