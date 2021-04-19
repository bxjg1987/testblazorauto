using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.DynamicAssociateEntity
{
    /// <summary>
    /// 被关联的数据选择时使用的控件
    /// </summary>
    public class Controls
    {
        /// <summary>
        /// 普通的下拉框
        /// </summary>
        public const string Combobox = "combobox";
        /// <summary>
        /// 下拉表格框
        /// </summary>
        public const string ComboGrid = "comboGrid";
        /// <summary>
        /// 下拉树形结构，如果感觉麻烦就不用这个，因为我们本身就提供级联选择，多个combobox或comboGrid 一样可以达到 下拉树形数据选择的效果
        /// </summary>
        public const string ComboTree = "comboTree";
        
        // 可以配置任意字符串，前端自己去适配
    }
}
