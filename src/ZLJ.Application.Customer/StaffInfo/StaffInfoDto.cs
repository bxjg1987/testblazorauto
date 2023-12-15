using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using BXJG.Utils.Localization;
using System.Linq;
using ZLJ.App.Common.OU;
using ZLJ.App.Common.Post;
using ZLJ.App.Common.Users;

namespace ZLJ.App.Customer.StaffInfo
{
    /// <summary>
    /// 客户管理员 管理员工时 查询列表返回的项模型
    /// </summary>
    public class StaffInfoDto : UserDto //FullAuditedEntityDto<long>
    {
        /// <summary>
        /// 密码
        /// </summary>
        public string EquipmentPwd { get; set; }
        /// <summary>
        /// 客户id
        /// </summary>
        public long CustomerId { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set;  }
        /// <summary>
        /// 性别
        /// </summary>
        public BXJG.Common.Gender Gender { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string GenderText => this.Gender.ToLocalizationString();
        /// <summary>
        /// 生日
        /// </summary>
        public DateTimeOffset? Birthday { get; set; }
        /// <summary>
        /// 生日yyyy-MM-dd
        /// </summary>
        public string BirthdayText => Birthday?.ToString("yyyy-MM-dd");
        /// <summary>
        /// 所属部门
        /// </summary>
        public long? OuId { get; set; }
        /// <summary>
        /// 所属部门名称
        /// </summary>
        public string OuName { get; set; }  
    }
}