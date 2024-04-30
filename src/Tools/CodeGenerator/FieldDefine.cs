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
        /// <summary>
        /// 是否是主键
        /// </summary>
        public bool IsPrimary { get; set; }
        /// <summary>
        /// 字段名，如：Id
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 字段显示名，如：编号
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// c#类型名称，对应Type.Name
        /// </summary>
        public string CSharpType { get; set; }
        /// <summary>
        /// 字段长度
        /// </summary>
        public int Length { get; set; }
    }
}