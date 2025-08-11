using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Share.Tag
{
    /// <summary>
    /// tag数据传输对象
    /// </summary>
    /// <param name="TagName">同一个标签类型的唯一名称</param>
    /// <param name="TagDisplayName">显示名称</param>
    /// <param name="OrderIndex">排序索引</param>
    public record class TagDto(string TagName, string TagDisplayName,int OrderIndex=0/*,string? ExtField1, string? ExtField2*/);
}
