using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BXJG.Common.Dynamics;

namespace BXJG.Common.Contracts
{
    /// <summary>
    /// 动态条件字段定义
    /// </summary>
    public class ConditionFieldDefine
    {
        //public int MyProperty { get; set; }
        /// <summary>
        /// 属性名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 比较符
        /// </summary>
        public CompareType CompareType { get; set; }
        /// <summary>
        /// 比较值
        /// </summary>
        public string Value { get; set; }
    }
}
