using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BXJG.Common.Contracts
{
    /// <summary>
    /// 性别
    /// </summary>
    [Flags]
    public enum Gender
    {
        /// <summary>
        /// 女
        /// </summary>
       [Description("女")]
        [Display(Name ="女")]
        Woman = 2,
        /// <summary>
        /// 男
        /// </summary>
       [Description("男")]
        [Display(Name ="男")]
        Man = 1,
        /// <summary>
        /// 未知
        /// </summary>
        [Description("未知")]
       [Display(Name ="未知")]
        Unknown = 0
    }
    /// <summary>
    /// 申请审核状态
    /// </summary>
    [Flags]
    [Obsolete]
    public enum AppliedAuditedStatus
    {
        /// <summary>
        /// 已申请
        /// </summary>
        Applied = 1,
        /// <summary>
        /// 已审核
        /// </summary>
        Audited = 2
    }
}
