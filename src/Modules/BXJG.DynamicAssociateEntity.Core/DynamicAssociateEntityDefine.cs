using Abp.Domain.Entities;
using Abp.Localization;
using System;
using System.Collections.Generic;

namespace BXJG.DynamicAssociateEntity
{
    /// <summary>
    /// 动态关联实体定义
    /// </summary>
    public class DynamicAssociateEntityDefine //: IMayHaveTenant
    {
        public string ParentName => Parent?.Name;
        //public int? TenantId { get; set; }
        /// <summary>
        /// 要关联到的目标实体名，推荐目标实体完整类名，但不是必须的
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 要关联到的目标实体的显示名，下拉框或列名会用到
        /// </summary>
        public ILocalizableString DisplayName { get; set; }
        /// <summary>
        /// 目标实体字段描述
        /// </summary>
        public virtual List<DynamicAssociateEntityDefineField> Fields { get; set; }
        /// <summary>
        /// 获取下拉框数据，应用层查询关联数据组合dto时会使用到
        /// </summary>
        public Type ServiceType { get; set; }
        /// <summary>
        /// 父节点<br />
        /// 考虑到级联关联，比如关联到订单明细时，是先选择订单，再选择明细
        /// </summary>
        public DynamicAssociateEntityDefine Parent { get; set; }
        /// <summary>
        /// 子节点集合<br />
        /// 考虑到级联关联，比如关联到订单明细时，是先选择订单，再选择明细
        /// </summary>
        public List<DynamicAssociateEntityDefine> Children { get; set; }
        //简单起见，先不考虑预留字段
        //public string ExtField1 { get; set; }
        //public string ExtField2 { get; set; }
        //public string ExtField3 { get; set; }
    }
    /// <summary>
    /// 动态关联实体定义中的字段
    /// </summary>
    public class DynamicAssociateEntityDefineField
    {
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
        /// 显示宽度
        /// </summary>
        public int DislayWidth { get; set; }
    }
}
