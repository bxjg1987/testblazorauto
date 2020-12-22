using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.DynamicProperty
{
    /// <summary>
    /// 动态属性编辑模型
    /// </summary>
    public class DynamicPropertyEditDto
    {
        /// <summary>
        /// 动态属性名，如：Color
        /// </summary>
        [Required]
        [StringLength(50)]
        public string PropertyName { get; set; }
        /// <summary>
        /// 动态属性显示名，如：颜色
        /// </summary>
        [Required]
        [StringLength(50)]
        public string DisplayName { get; set; }
        /// <summary>
        /// 输入类型，可选值：COMBOBOX、CHECKBOX、SINGLE_LINE_INPUT
        /// </summary>
        [Required]
        [StringLength(50)]
        public string InputType { get; set; }
        /// <summary>
        /// 当输入类型为COMBOBOX的可选值列表，多个用英文逗号分隔，如：绿色,黄色
        /// </summary>
        public string PropertyValues { get; set; }
    }
    /// <summary>
    /// 动态属性查询模型
    /// </summary>
    public class DynamicPropertyDto
    {
        /// <summary>
        /// 动态实体属性id，它不说动态属性DynamicProperty.Id，而是DynamicEntityProperty.Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 动态属性名，如：Color
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// 动态属性显示名，如：颜色
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 输入类型，可选值：COMBOBOX、CHECKBOX、SINGLE_LINE_INPUT
        /// </summary>
        public string InputType { get; set; }
        //public string DynamicPropertyValues { get; set; }
        /// <summary>
        /// 当输入类型为COMBOBOX的可选值列表
        /// </summary>
        public IDictionary<long,string> PropertyValues { get; set; }
    }
}
