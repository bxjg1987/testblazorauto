using Abp.Domain.Entities;
using Abp.Localization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BXJG.DynamicAssociateEntity
{
    /// <summary>
    /// 动态关联实体定义
    /// </summary>
    public class DynamicAssociateEntityDefine
    {
        //public int? TenantId { get; set; }
        /// <summary>
        /// 要关联到的目标实体名,全局唯一，推荐目标实体完整类名，但不是必须的
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 要关联到的目标实体的显示名，下拉框或列名会用到
        /// </summary>
        public ILocalizableString DisplayName { get; set; }
        /// <summary>
        /// 目标实体字段定义
        /// </summary>
        public virtual DynamicAssociateEntityDefineField[] Fields { get; set; }
        /// <summary>
        /// 获取下拉框数据，应用层查询关联数据组合dto时会使用到
        /// </summary>
        public Type ServiceType { get; set; }
        ///// <summary>
        ///// 获取下拉框数据，应用层查询关联数据组合dto时会使用到
        ///// </summary>
        //public Type ServiceType2 { get; set; }
        /// <summary>
        /// 父节点<br />
        /// 考虑到级联关联，比如关联到订单明细时，是先选择订单，再选择明细
        /// </summary>
        public DynamicAssociateEntityDefine Parent { get; set; }
        /// <summary>
        /// 级联父级名称
        /// </summary>
        public string ParentName => Parent?.Name;
        /// <summary>
        /// 子节点集合<br />
        /// 考虑到级联关联，比如关联到订单明细时，是先选择订单，再选择明细
        /// </summary>
        public DynamicAssociateEntityDefine Child { get; set; }
        /// <summary>
        /// 级联子节点名称
        /// </summary>
        public string ChildName => Child?.Name;
        /// <summary>
        /// 数据主键字段
        /// </summary>
        public DynamicAssociateEntityDefineField KeyField => Fields.Single(c => c.IsKey);
        /// <summary>
        /// 用作显示的列集合
        /// </summary>
        public IReadOnlyList<DynamicAssociateEntityDefineField> DisplayFields => Fields.Where(c => c.IsDisplayField).ToList().AsReadOnly();
        //简单起见，先不考虑预留字段
        //public string ExtField1 { get; set; }
        //public string ExtField2 { get; set; }
        //public string ExtField3 { get; set; }
        //public int OrderIndex { get; set; }
        /// <summary>
        /// 控件，可以配置任意字符串，前端自己去适配。Controls也提供了几个常量<br />
        /// 目前只考虑被选择的数据只提供固定的UI选择形式，将来若需要可以把此属性当做数据的默认UI
        /// </summary>
        public string Control { get; set; } = Controls.ComboGrid;
    }
    /// <summary>
    /// 动态关联实体定义中的字段
    /// </summary>
    public class DynamicAssociateEntityDefineField
    {
        public bool IsDisplayField { get; set; }
        public bool IsKey { get; set; }
        /// <summary>
        /// 字段名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 字段显示名
        /// </summary>
        public ILocalizableString DislayName { get; set; }
        /// <summary>
        /// 显示格式
        /// </summary>
        public string DislayFormatter { get; set; }
        /// <summary>
        /// 显示宽度，不同的前端需要的宽度不同，这里只是个默认值，或者作为百分比
        /// </summary>
        public int DislayWidth { get; set; }

        //public int OrderIndex { get; set; }
    }
}
