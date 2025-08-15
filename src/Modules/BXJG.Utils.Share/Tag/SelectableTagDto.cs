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
        //[JsonIgnore]
        public int OrderIndex { get; set; }
        /// <summary>
        /// 是否已选择
        /// </summary>
        public bool IsSelected { get; set; }
        public SelectableTagDto(string tagName, string tagDisplayName, int orderIndex = 0,bool isSelected=false) //: this(TagName, TagName, OrderIndex)
        {
            this.TagName = tagName;
            this.TagDisplayName = tagDisplayName;
            this.OrderIndex = orderIndex;
            this.IsSelected = isSelected;
        }
        public SelectableTagDto(string tagName, int orderIndex = 0, bool isSelected = false) : this(tagName, tagName, orderIndex, isSelected)
        {

        }
    }
}
