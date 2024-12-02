using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BXJG.Common.Contracts
{
    //常用枚举
    [Flags]
    public enum Gender
    {
        /// <summary>
        /// 
        /// </summary>
        Woman = 2,
        /// <summary>
        /// 
        /// </summary>
        Man = 1
    }
    [Flags]
    public enum AppliedAuditedStatus
    {
        /// <summary>
        /// 
        /// </summary>
        Applied = 1,
        /// <summary>
        /// 
        /// </summary>
        Audited = 2
    }
}
