using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Common
{
    /*
     * 有必要用ioc吗？
     * 原始的描述数据，没必要
     * 
     * 后期可以考虑根据反射实体上的Attribute生成元数据
     */

    public abstract class PropertyMetadataManager<TProperty> : IReadOnlyDictionary<string, TProperty>, IReadOnlyList<TProperty>
        where TProperty : BXJGDynamicPropertyWithoutInput
    {
        Dictionary<string, TProperty> dic = new Dictionary<string, TProperty>();
        List<TProperty> list;
        public PropertyMetadataManager(IList<TProperty> properties)
        {
            this.list = properties.ToList();
            this.list.ForEach(c => dic.Add(c.Name, c));
            //list.ToDictionary<string, TProperty>(c => c.Name, c=>c);
        }



        public TProperty this[string key] => ((IReadOnlyDictionary<string, TProperty>)dic)[key];

        public TProperty this[int index] => ((IReadOnlyList<TProperty>)list)[index];

        public IEnumerable<string> Keys => ((IReadOnlyDictionary<string, TProperty>)dic).Keys;

        public IEnumerable<TProperty> Values => ((IReadOnlyDictionary<string, TProperty>)dic).Values;

        public int Count => ((IReadOnlyCollection<KeyValuePair<string, TProperty>>)dic).Count;

        public bool ContainsKey(string key)
        {
            return ((IReadOnlyDictionary<string, TProperty>)dic).ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<string, TProperty>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, TProperty>>)dic).GetEnumerator();
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out TProperty value)
        {
            return ((IReadOnlyDictionary<string, TProperty>)dic).TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)dic).GetEnumerator();
        }

        IEnumerator<TProperty> IEnumerable<TProperty>.GetEnumerator()
        {
            return ((IEnumerable<TProperty>)list).GetEnumerator();
        }
    }




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
        /// <summary>
        /// 字段名称
        /// </summary>
        public string Name { get; init; }
        /// <summary>
        /// 字段显示名称
        /// </summary>
        public string DisplayName { get; init; }
        /// <summary>
        /// 字段类型
        /// </summary>
        public Type Type { get; init; }
        /// <summary>
        /// 格式化显示模式
        /// </summary>
        public string DisplayFormatter { get; init; }
        /// <summary>
        /// 小数位数
        /// </summary>
        public int Precision { get; init; }
        /// <summary>
        /// 额外状态数据
        /// </summary>
        public IReadOnlyDictionary<string, object> AdditionalData { get; init; }
    }
    /// <summary>
    /// 字段元数据，包含输入
    /// </summary>
    public class BXJGDynamicProperty : BXJGDynamicPropertyWithoutInput
    {
        public BXJGDynamicProperty(string name,
                                   string displayName,
                                   string inputType = "text",
                                   string displayFormatter = "yyyy-MM-dd HH:mm:ss",
                                   int decimalPlaces = 2,
                                   IDictionary<string, object> values = default,
                                   ICollection<ValidationAttribute> validators = default,
                                   IReadOnlyDictionary<string, object> additionalData = default,
                                   object defaultValue = null,
                                   Type type = default) : base(name,
                                                               displayName,
                                                               type,
                                                               additionalData,
                                                               displayFormatter,
                                                               decimalPlaces)
        {
            // Name = name;
            //DisplayName = displayName;
            InputType = inputType;
            //DisplayFormatter = displayFormatter;
            //Precision = decimalPlaces;
            Values = new ReadOnlyDictionary<string, object>(values ?? new Dictionary<string, object>());
            Validators = validators?.ToList() ?? new List<ValidationAttribute>();
            // AdditionalData = new ReadOnlyDictionary<string, object>(additionalData ?? new Dictionary<string, object>());
            DefaultValue = defaultValue;
        }

        //public string Name { get; init; }

        //public string DisplayName { get; init; }

        /// <summary>
        /// 前端输入框类型
        /// </summary>
        public string InputType { get; init; }
        //  public string DisplayFormatter { get; init; }

        //        public int Precision { get; init; }
        /// <summary>
        /// 可选值
        /// </summary>
        public IReadOnlyDictionary<string, object> Values { get; init; }
        /// <summary>
        /// 是否允许多选
        /// </summary>
        public bool MultipleSelect { get; init; }
        /// <summary>
        /// 验证规则
        /// </summary>
        public IReadOnlyCollection<ValidationAttribute> Validators { get; init; }
        /// <summary>
        /// 默认值
        /// </summary>
        public object DefaultValue { get; set; }

        //      [Obsolete("初始化时可以直接指定顺序，所以此字段没用了")]
        //    public int OrderIndex { get; set; }

        //   public IReadOnlyDictionary<string, object> AdditionalData { get; set; }
    }
    /// <summary>
    /// 关联显示时使用
    /// </summary>
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
        [Required]
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
