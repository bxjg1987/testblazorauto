using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Core.Share
{
    [Flags]
    public enum OUType
    {
        /// <summary>
        /// 总部
        /// </summary>
        [Display(Name = "总部")]
        HeadOffice = 1,
        /// <summary>
        /// 分公司
        /// </summary>
        [Display(Name = "分公司")]
        Branch = 2,
        /// <summary>
        /// 部门
        /// </summary>
        [Display(Name = "部门")]
        Department = 4
    }
}
