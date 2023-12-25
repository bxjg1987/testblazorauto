using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.BaseInfoCommon.StaffInfo
{
    /// <summary>
    /// 获取员工信息时的条件模型
    /// </summary>
    public class GetTotalInput
    {
        //public int UserId { get; set; }
        /// <summary>
        /// 所属区域code
        /// </summary>
        public string AreaCode { get; set; }
        /// <summary>
        /// 关键字，模糊查询 姓名、工号、手机号等
        /// </summary>
        public string Keyword { get; set; }
    }
}
