using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.BaseInfoCommon.StaffInfo
{
    /// <summary>
    /// 员工信息的显示模型
    /// </summary>
    public class Dto:EntityDto<long>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 员工编号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 关联用户Id
        /// </summary>
        public long? UserId { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string UserPhoneNumber { get; set; }
        /// <summary>
        /// 所属区域Id
        /// </summary>
        public long? AreaId { get; set; }

        /// <summary>
        /// 所属区域名
        /// </summary>
        public string AreaDisplayName { get; set; }
    }
}
