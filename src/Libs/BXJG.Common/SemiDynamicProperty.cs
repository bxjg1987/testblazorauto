using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Common
{
    /// <summary>
    /// 半动态属性
    /// </summary>
    public class SemiDynamicProperty
    {
        public SemiDynamicProperty(string name,
                                   string displayName,
                                   string inputType = "SINGLEINPUT",
                                   string dateTimeFormatter = "yyyy-MM-dd HH:mm:ss",
                                   int decimalPlaces = 2,
                                   IDictionary<string, object> values = default,
                                   ICollection<ValidationAttribute> validators = default)
        {
            Name = name;
            DisplayName = displayName;
            InputType = inputType;
            DateTimeFormatter = dateTimeFormatter;
            DecimalPlaces = decimalPlaces;
            Values = new ReadOnlyDictionary<string, object>(values ?? new Dictionary<string, object>());
            Validators = validators?.ToList() ?? new List<ValidationAttribute>();
        }
        /// <summary>
        /// 字段名
        /// </summary>
        public string Name { get; init; }
        /// <summary>
        /// 显示名
        /// </summary>
        public string DisplayName { get; init; }
        /// <summary>
        /// 输入类型
        /// </summary>
        public string InputType { get; init; }
        /// <summary>
        /// 若是时间类型，则表示时间格式
        /// </summary>
        public string DateTimeFormatter { get; init; }
        /// <summary>
        /// 若是小数类型，则表示小数位数
        /// </summary>
        public int DecimalPlaces { get; init; }
        /// <summary>
        /// 若是下拉选择，可选值
        /// </summary>
        public IReadOnlyDictionary<string, object> Values { get; init; }
        /// <summary>
        /// 若是下拉选择，是否多选
        /// </summary>
        public bool MultipleSelect { get; init; }
        /// <summary>
        /// 多个验证器
        /// </summary>
        public IReadOnlyCollection<ValidationAttribute> Validators { get; init; }
    }

    //public class SemiDynamicPropertyDto {
    //    public string PropertyName { get; set; }
    //    public string PropertyDisplayName { get; set; }
    //    public IReadOnlyCollection<ValidationAttribute> Validators { get; set; }
    //}
    //public class ValidationDto { }

}
