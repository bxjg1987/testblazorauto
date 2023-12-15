using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.App.Customer.StaffInfo
{
    /// <summary>
    /// 管理员工时，获取员工的列表的条件
    /// </summary>
    public class GetStaffInfoListCondition
    {
        public bool? IsActive { get; set; }
        /// <summary>
        /// 公司和部门code
        /// </summary>
        public string OuCode { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keywords { get; set; }
    }
}