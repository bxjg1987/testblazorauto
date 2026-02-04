using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BXJG.Utils.Share.Menu
{
    /// <summary>
    /// 菜单显示模式
    /// </summary>
    [Flags]
    public enum MenuShowModel
    {
        /// <summary>
        /// 不显示
        /// </summary>
        [Display(Name ="不显示")]
        None = 1<< 0,
        /// <summary>
        /// 主菜单
        /// </summary>
        [Display(Name = "主菜单")]
        Main = 1 << 1,
        /// <summary>
        /// 普通
        /// </summary>
        [Display(Name = "普通")]
        Normal = 1 << 2,
    }
}
