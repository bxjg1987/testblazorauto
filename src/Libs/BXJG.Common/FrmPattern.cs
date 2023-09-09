using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Common
{
    /// <summary>
    /// 弹窗模式
    /// </summary>
    public enum FrmPattern
    {
        [Description("新增")]
        Add,
        [Description("修改")]
        Edit,
        [Description("查看")]
        Look
    }
}
