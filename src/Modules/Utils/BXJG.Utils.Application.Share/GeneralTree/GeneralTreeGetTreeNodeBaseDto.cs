using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using BXJG.Common.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
    public class GeneralTreeNodeBaseDto<TChild> : AuditedEntityDto<long>, IExtendableObj, IGeneralTree<TChild>,IPassivable
        where TChild : GeneralTreeNodeBaseDto<TChild>
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        [Display(Name="是否启用")]
        public bool IsActive { get; set; } = true;
        long? parentId;
        public long? ParentId { get => parentId; set => parentId = value; }
        object IHaveParentId.Id { get => Id; set => Id = Convert.ToInt64(value); }
        object IHaveParentId.ParentId { get => ParentId; set => ParentId = value == null ? null : Convert.ToInt64(value); }

        /// <summary>
        /// 父节点
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        public TChild Parent { get; set; }
        ///// <summary>
        ///// 父级组织单位id
        ///// </summary>
        //public long? ParentId { get; set; }
        /// <summary>
        /// 有层次结构的代码
        /// </summary>
        [DisplayName("代码")]
        public string Code { get; set; }

        public string ParentDisplayName { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        [DisplayName("名称")]
        public string DisplayName { get; set; }
        /// <summary>
        /// 子集
        /// </summary>
        public List<TChild> Children { get; set; }
        /// <summary>
        /// 配合easyui，state：节点状态，'open' 或 'closed'，默认：'open'。如果为'closed'的时候，将不自动展开该节点。
        /// </summary>
       // [Ignore]
        public string State => this.Children != null && this.Children.Count > 0&& !string.IsNullOrWhiteSpace(this.Code) ? "closed" : "open";
        /// <summary>
        /// 子节点数量
        /// </summary>
        [DisplayName("子节点数量")]
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
        [DisplayName("名称")]
        public string Text { get { return DisplayName; } }
        ///// <summary>
        ///// 排序索引
        ///// </summary>
        //public int OrderIndex { get; set; }

        private string extensionData;
        /// <summary>
        /// 节点标识，不同租户下同类型的节点，此字段一样
        /// 如：品牌  表示品牌节点，不同租户下此字段值一样
        /// 使用场景：在数据字典功能中，前端下拉框绑定时可以通过此字段绑定指定节点类型
        /// 不能用DisplayName，因为它可能变
        /// 不能用id，因为相同数据库中的不同租户id不同
        /// 不能用code，因为节点移动后，code也会变
        /// 用不到此字段时，请忽略。此字段通常不允许修改
        /// </summary>
        [DisplayName("节点名称")]
        public string? Name { get; set; }
        //对ExtData的赋值本来可以直接在AutoMapper映射中来做
        //但是模块中使用了泛型，加上实现子类可能有更多泛型，AutoMapper好像支持不太好
        //因此AutoMapper映射原始的ExtensionData，在属性内部设置ExtData
        //ExtensionData本身就不需要序列化到前端了

        //public string ExtensionData
        //{
        //    get
        //    {
        //        return extensionData;
        //    }
        //    set
        //    {
        //        extensionData = value;
        //        if (string.IsNullOrWhiteSpace(value))
        //            ExtData = null;
        //        else
        //            ExtData = JsonConvert.DeserializeObject<dynamic>(value);
        //    }
        //}
        //// public dynamic ExtensionData { get; set; } 
        ///// <summary>
        ///// 扩展属性
        ///// </summary>
        //// [Ignore]
        //public dynamic ExtData { get; private set; }
        [DisplayName("扩展属性")]
        public dynamic ExtensionData { get; set; }
    }
}
