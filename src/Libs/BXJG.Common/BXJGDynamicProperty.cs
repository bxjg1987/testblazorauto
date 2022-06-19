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

        public int OrderIndex { get; set; }

        public IReadOnlyDictionary<string, object> AdditionalData{ get; set; }
    }
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
    //public class SemiDynamicPropertyDto {
    //    public string PropertyName { get; set; }
    //    public string PropertyDisplayName { get; set; }
    //    public IReadOnlyCollection<ValidationAttribute> Validators { get; set; }
    //}
    //public class ValidationDto { }
}
