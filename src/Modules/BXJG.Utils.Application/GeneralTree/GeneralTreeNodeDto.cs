using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.GeneralTree
{
    /// <summary>
    /// 获取树形下拉框数据的模型
    /// </summary>
    public class GeneralTreeNodeDto<T> where T : GeneralTreeNodeDto<T>
    {
        public string Id { get; set; }//用id是为了适配easyui的tree  combotree共用此模型
        public string Text { get; set; }
        public string IconCls { get; set; }
        public bool Checked { get; set; }
        public string State => this.Children != null && this.Children.Count > 0 && !string.IsNullOrWhiteSpace(this.Code) ? "closed" : "open";
        /// <summary>
        /// 子节点数量
        /// </summary>
        public int ChildrenCount { get; set; }
        //public dynamic attributes { get; set; }
        public IList<T> Children { get; set; }

        public string ParentId { get; set; }

        //因为不确定前端是否支持这样自定义的字段，因此保险起见数据都保存到attributes，这里只提供读取
        public string Code { get; set; }

        private string extensionData;
        [JsonIgnore]//目前默认使用的并非.net 3.x的json序列化
        public string ExtensionData
        {
            get
            {
                return extensionData;
            }
            set
            {
                extensionData = value;
                if (string.IsNullOrWhiteSpace(value))
                    ExtData = null;
                else
                    ExtData = JsonConvert.DeserializeObject<dynamic>(value);
            }
        }
        /// <summary>
        /// 扩展属性
        /// </summary>
       // [Ignore]
        public dynamic ExtData { get; set; }
    }

    public class GeneralTreeNodeDto : GeneralTreeNodeDto<GeneralTreeNodeDto> { }
}
