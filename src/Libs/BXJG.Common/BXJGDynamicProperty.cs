using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Common
{
    /// <summary>
    /// 不需要用户输入的动态属性
    /// </summary>
    public class BXJGDynamicPropertyWithoutInput
    {
        public BXJGDynamicPropertyWithoutInput() { }
        public BXJGDynamicPropertyWithoutInput(string name,
                                               string displayName,
                                               Type type,
                                               IReadOnlyDictionary<string, object> additionalData = default,
                                               string displayFormatter = default,
                                               int precision = 2)
        {
            Name = name;
            DisplayName = displayName;
            Type = type;
            AdditionalData = additionalData;
            DisplayFormatter = displayFormatter;
            Precision = precision;
        }

        public string Name { get; init; }

        public string DisplayName { get; init; }

        public Type Type { get; init; }
        public string DisplayFormatter { get; init; }

        public int Precision { get; init; }
        public IReadOnlyDictionary<string, object> AdditionalData { get; init; }
    }
    public class BXJGDynamicProperty //: DynamicObject
    {
        public BXJGDynamicProperty(string name,
                                   string displayName,
                                   string inputType = "text",
                                   string displayFormatter = "yyyy-MM-dd HH:mm:ss",
                                   int decimalPlaces = 2,
                                   IDictionary<string, object> values = default,
                                   ICollection<ValidationAttribute> validators = default,
                                   IDictionary<string, object> additionalData = default,
                                   object defaultValue = null)
        {
            Name = name;
            DisplayName = displayName;
            InputType = inputType;
            DisplayFormatter = displayFormatter;
            Precision = decimalPlaces;
            Values = new ReadOnlyDictionary<string, object>(values ?? new Dictionary<string, object>());
            Validators = validators?.ToList() ?? new List<ValidationAttribute>();
            AdditionalData = new ReadOnlyDictionary<string, object>(additionalData ?? new Dictionary<string, object>());
            DefaultValue = defaultValue;
        }

        public string Name { get; init; }

        public string DisplayName { get; init; }

        public string InputType { get; init; }

        public string DisplayFormatter { get; init; }

        public int Precision { get; init; }

        public IReadOnlyDictionary<string, object> Values { get; init; }

        public bool MultipleSelect { get; init; }

        public IReadOnlyCollection<ValidationAttribute> Validators { get; init; }

        public object DefaultValue { get; set; }

        [Obsolete("初始化时可以直接指定顺序，所以此字段没用了")]
        public int OrderIndex { get; set; }

        public IReadOnlyDictionary<string, object> AdditionalData { get; set; }
    }
    public class DynamicPropertyDto
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public object Value { get; set; }
    }

    public class DynamicPropertyEditDto
    {
        /// <summary>
        /// 属性名称
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 当前值
        /// </summary>
        public string CurrentValue { get; set; }
    }

    /// <summary>
    /// 后台管理合同属性变更显示模型
    /// </summary>
    public class PropertyChangeRecordDto
    {
        /// <summary>
        /// 属性名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 属性显示名
        /// </summary>
        public string DisplayName { get; set; } //=> PropertyMap.GetDisplayName(PropertyName, !OrderItemId.HasValue);
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
        /// <summary>
        /// 是否有变动
        /// </summary>
        public bool IsChanged => this.OriginalValue.Equals(this.CurrentValue);
    }
    //public class SemiDynamicPropertyDto {
    //    public string PropertyName { get; set; }
    //    public string PropertyDisplayName { get; set; }
    //    public IReadOnlyCollection<ValidationAttribute> Validators { get; set; }
    //}
    //public class ValidationDto { }
}
