using Abp.Application.Services.Dto;
using BXJG.Common.Dto;
using BXJG.Utils.Share.GeneralTree;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share.GeneralTree
{
    /// <summary>
    /// 树形数据管理的列表页使用的dto基类，它不是抽象的，可以直接使用
    /// </summary>
    /// <typeparam name="TChild"></typeparam>
    public class GeneralTreeGetTreeNodeBaseDto<TChild> : AuditedEntityDto<long>, IExtendableDto, IGeneralTree<TChild>
        where TChild : GeneralTreeGetTreeNodeBaseDto<TChild>
    {
        long? parentId;
        public long? ParentId { get => parentId; set => parentId = value; }
        object IHaveParentId.Id { get => Id; set => Id = Convert.ToInt64(value); }
        object IHaveParentId.ParentId { get => ParentId; set => ParentId = value == null ? null : Convert.ToInt64(value); }

        /// <summary>
        /// 父节点
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public TChild Parent { get; set; }
        ///// <summary>
        ///// 父级组织单位id
        ///// </summary>
        //public long? ParentId { get; set; }
        /// <summary>
        /// 有层次结构的代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 子集
        /// </summary>
        public IList<TChild> Children { get; set; }
        /// <summary>
        /// 配合easyui，state：节点状态，'open' 或 'closed'，默认：'open'。如果为'closed'的时候，将不自动展开该节点。
        /// </summary>
       // [Ignore]
        public string State => this.Children != null && this.Children.Count > 0&& !string.IsNullOrWhiteSpace(this.Code) ? "closed" : "open";
        /// <summary>
        /// 子节点数量
        /// </summary>
        public int ChildrenCount { get; set; }

        //{
        //    get
        //    {
        //        return Children!=null&& Children.Count > 0&& Id!=0 ? "closed" : "open";
        //    }
        //}
        /// <summary>
        /// 什么鬼
        /// </summary>
        public string Text { get { return DisplayName; } }
        ///// <summary>
        ///// 排序索引
        ///// </summary>
        //public int OrderIndex { get; set; }

        private string extensionData;

        //对ExtData的赋值本来可以直接在AutoMapper映射中来做
        //但是模块中使用了泛型，加上实现子类可能有更多泛型，AutoMapper好像支持不太好
        //因此AutoMapper映射原始的ExtensionData，在属性内部设置ExtData
        //ExtensionData本身就不需要序列化到前端了

        [Newtonsoft.Json.JsonIgnore]//目前默认使用的并非.net 3.x的json序列化
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
        // public dynamic ExtensionData { get; set; } 
        /// <summary>
        /// 扩展属性
        /// </summary>
        // [Ignore]
        public dynamic ExtData { get; private set; }
        dynamic IExtendableDto.ExtensionData { get; set; }
    }
}
