using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BXJG.Utils.Share.DataPermission
{
    /// <summary>
    /// 数据授权类型
    /// </summary>
    [Flags]
    public enum DataPermissionGrantType
    {
        /// <summary>
        /// 允许查看全部数据
        /// </summary>
        [Display(Name = "全部允许")]
        All =1<<0,
        /// <summary>
        /// 拒绝
        /// </summary>
        [Display(Name = "拒绝")]
        Rejected = 1 << 1,
        /// <summary>
        /// 仅自己
        /// </summary>
        [Display(Name = "仅自己")]
        OnlyMe = 1 << 2,
        /// <summary>
        /// 组织单位
        /// </summary>
        [Display(Name = "组织单位")]
        OrganizationUnit = 1 << 3,
        /// <summary>
        /// 组织单位（递归）
        /// </summary>
        [Display(Name = "组织单位（递归）")]
        OrganizationUnitRecursive = 1 << 4,
    }
}