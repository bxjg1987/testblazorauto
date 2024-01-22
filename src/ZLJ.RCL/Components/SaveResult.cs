using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.RCL.Components
{
    /// <summary>
    /// 新增返回对象
    /// </summary>
    public class SaveResult<TEntityDto>
    {
        /// <summary>
        /// 新增后返回的dto对象
        /// </summary>
        public TEntityDto Dto { get; set; }
        /// <summary>
        /// 新增是否结束了，
        /// 若没有勾选“保存并继续”，则新增后表示新增结束
        /// 验证不过也会返回false
        /// </summary>
        public bool End { get; set; }
    }
}
