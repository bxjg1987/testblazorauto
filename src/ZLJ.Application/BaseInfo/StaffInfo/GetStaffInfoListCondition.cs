using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Application.BaseInfo.StaffInfo
{
    /// <summary>
    /// 管理员工时，获取员工的列表的条件
    /// </summary>
    public class GetStaffInfoListCondition
    {
        /// <summary>
        /// 公司和部门code
        /// </summary>
        public string OuCode { get; set; }
        /// <summary>
        /// 区域编码
        /// </summary>
        public string AreaCode { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keywords { get; set; }
        /// <summary>
        /// 岗位id
        /// </summary>
        public int? PostId { get; set; }
    }
}