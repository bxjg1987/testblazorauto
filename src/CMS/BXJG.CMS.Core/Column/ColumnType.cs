using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.CMS.Column
{
    /*
     * 单页栏目有两种方式，以关于我们栏目为例，下面有公司简介，发展历程，
     * 方式1、将关于我们设置为SinglePage，然后在此栏目下放公司简介和发展历程文章
     * 方式2、建立公司简介和发展历程两个栏目，类型设置为SinglePage，然后建立对应的文章进行关联
     * 目前简单起见选择方式1
     * 
     * 新闻
     *      公司新闻
     *              活动公告
     *              公司动态
     *      行业动态
     *      
     * 这种栏目结构下 新闻、公司新闻 因为有下级栏目，所以它们的类型设置为SinglePage或Info 都不太合适，所以单独定义Structure
     */

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
