using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Enums
{
    [Flags]
    public enum OUType
    {
        /// <summary>
        /// 总部
        /// </summary>
        HeadOffice=1,
        /// <summary>
        /// 分公司
        /// </summary>
        Branch=2,
        /// <summary>
        /// 部门
        /// </summary>
        Department=4
    }
}
