using Abp.Application.Services.Dto;
using Abp.Extensions;
using BXJG.Utils.Share.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share.GeneralTree
{
    /// <summary>
    /// 获取树形下拉框数据的模型
    /// </summary>
    public class GeneralTreeNodeDto<T> : EntityDto<long>, IGeneralTree<T>
        where T : GeneralTreeNodeDto<T>
    {
        //public string Id { get; set; }//用id是为了适配easyui的tree  combotree共用此模型
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

        /// <summary>
        /// 父节点
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public T Parent { get; set; }


        long? parentId;
        public long? ParentId { get => parentId; set => parentId = value; }
        object IHaveParentId.Id { get => Id; set => Id = Convert.ToInt64(value); }
        object IHaveParentId.ParentId { get => ParentId; set => ParentId = value == null ? null : Convert.ToInt64(value); }

        //因为不确定前端是否支持这样自定义的字段，因此保险起见数据都保存到attributes，这里只提供读取
        public string Code { get; set; }

        private string extensionData;
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
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
                    ExtData = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(value);
            }
        }
        /// <summary>
        /// 扩展属性
        /// </summary>
       // [Ignore]
        public dynamic ExtData { get; set; }
       // long IGeneralTree<T>.Id { get => long.Parse(Id); set => Id = value.ToString(); }
        //long? IGeneralTree<T>.ParentId { get =>  ParentId.IsNullOrWhiteSpaceBXJG()?default:long.Parse(ParentId)  ; set => ParentId= value.HasValue?value.ToString():default; }
      public  string DisplayName { get => Text; set => Text=value; }

    }

    public class GeneralTreeNodeDto : GeneralTreeNodeDto<GeneralTreeNodeDto> { }
}
