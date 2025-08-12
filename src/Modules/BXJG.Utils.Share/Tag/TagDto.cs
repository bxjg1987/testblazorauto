using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BXJG.Utils.Share.Tag
{
    /// <summary>
    /// tag数据传输对象
    /// </summary>
    /// <param name="TagName">同一个标签类型的唯一名称</param>
    /// <param name="TagDisplayName">显示名称</param>
    /// <param name="OrderIndex">排序索引</param>
    public class TagDto
    {
        public string TagName { get; set; }
        public string TagDisplayName { get; set; }
        [JsonIgnore]
        public int OrderIndex { get; set; }

        public TagDto(string TagName, string TagDisplayName, int OrderIndex = 0) //: this(TagName, TagName, OrderIndex)
        {
            this.TagName = TagName;
            this.TagDisplayName = TagDisplayName;
            this.OrderIndex = OrderIndex;
        }
        public TagDto(string TagName, int OrderIndex = 0) : this(TagName, TagName, OrderIndex)
        {

        }
    }
}
