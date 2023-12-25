using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Enums
{
    [Flags]
    public enum Gender
    {
        //[Description("Man")]//未給参数时使用枚举名称作为本地化键
        //Unknown,
        Man=1,
        Woman=0
    }
}
