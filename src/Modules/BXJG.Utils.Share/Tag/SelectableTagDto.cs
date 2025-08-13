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
    public class SelectableTagDto
    {
        public string TagName { get; set; }
        public string TagDisplayName { get; set; }
        /// <summary>
        /// 数字越大，优先级越高
        /// </summary>
        [JsonIgnore]
        public int OrderIndex { get; set; }

        public SelectableTagDto(string TagName, string TagDisplayName, int OrderIndex = 0) //: this(TagName, TagName, OrderIndex)
        {
            this.TagName = TagName;
            this.TagDisplayName = TagDisplayName;
            this.OrderIndex = OrderIndex;
        }
        public SelectableTagDto(string TagName, int OrderIndex = 0) : this(TagName, TagName, OrderIndex)
        {

        }
    }
}
