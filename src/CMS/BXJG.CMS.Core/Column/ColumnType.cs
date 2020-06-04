using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.CMS.Column
{
    /// <summary>
    /// 栏目类型
    /// </summary>
    public enum ColumnType
    {
        /// <summary>
        /// 单页栏目，比如：关于我们栏目下的每一篇文章都应该对应一个页面，不存在列表页和内容页
        /// </summary>
        SinglePage,
        /// <summary>
        /// 普通信息栏目，比如：社会新闻，有列表页和内容页
        /// </summary>
        Info,
        /// <summary>
        /// 结构型栏目，它不是一个具体的栏目，它是来组织栏目树的，比如 新闻，下有社会新闻，国际新闻，那么新闻栏目本身就是结构型栏目
        /// </summary>
        Structure
    }
}
