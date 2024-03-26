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
        Woman = 2,
        Man = 1
    }
    [Flags]
    public enum AppliedAuditedStatus
    {
        Applied = 1,
        Audited = 2
    }
}
