using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator
{
    /// <summary>
    /// 字段定义
    /// </summary>
    public class FieldDefine
    {
        public ModelDefine Model {  get; set; }

        /// <summary>
        /// 是否是主键
        /// </summary>
        public bool IsPrimary { get; set; }
        /// <summary>
        /// 字段名，如：Id
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }
        /// <summary>
        /// 不设置的化，由ef自己根据数据类型去判断
        /// </summary>
        public bool IsRequired { get; set; }
        /// <summary>
        /// 字段显示名，如：编号
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 界面上显示的宽度
        /// 不同ui库要的单位不同
        /// </summary>
        public string Width { get; set; } = "150";
        /// <summary>
        /// 在列表中显示时是否固定 true固定在左侧；false固定在右侧；null不固定
        /// </summary>
        public bool? FixedLeft { get; set; }
        /// <summary>
        /// 在列表中显示的位置，true左 false右 null中
        /// </summary>
        public bool? Position {  get; set; }
        /// <summary>
        /// c#类型名称，对应Type.Name
        /// </summary>
        public string CSharpType { get; set; }
        /// <summary>
        /// 字段长度
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// 是否使用unicode编码
        /// </summary>
        public bool IsUnicode { get; set; }
        /// <summary>
        /// 字段MaxLength常量名，如：NameMaxLength
        /// </summary>
        public string MaxLength => $"{Name}MaxLength";
        /// <summary>
        /// 最大数值，类型为数字时有效，若为空则不管最大值
        /// </summary>
        public decimal? NumberMax { get; set; }
        /// <summary>
        /// 最小数值，类型为数字时有效，若为空则不管设置最小值
        /// </summary>
        public decimal? NumberMin { get; set; }
        /// <summary>
        /// 步进数值，类型为数字时有效，若为空则不管步进
        /// </summary>
        public decimal? NumberStep { get; set; }
        /// <summary>
        /// 若是小数，则指定它的精度，若为空，则保持字段类型默认的精度
        /// </summary>
        public int? NumberPrecision { get; set; }
        /// <summary>
        /// 字段MaxLength常量全名，如：TestConsts.NameMaxLength
        /// </summary>
        public string CoreShareConstsMaxLength => $"{Model.CoreShareConstName}.{MaxLength}";
        /// <summary>
        /// 是否参与排序
        /// </summary>
        public bool IsSort { get; set; }
        /// <summary>
        /// 是否作为搜索时的条件
        /// </summary>
        public bool IsCondition { get; set; }
        /// <summary>
        /// 是否作为范围条件，IsCondition为true时有效
        /// </summary>
        public bool IsConditionRange { get; set; }
        /// <summary>
        /// 作为范围条件时的字段名称，如：AgeMin
        /// </summary>
        public string ConditionRangeMin => $"{Name}Min";
        /// <summary>
        /// 作为范围条件时的字段名称，如：AgeMax
        /// </summary>
        public string ConditionRangeMax => $"{Name}Max";
    }
}