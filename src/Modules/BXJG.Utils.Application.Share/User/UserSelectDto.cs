using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share.User
{
    /// <summary>
    /// 作为下拉框选择用户的数据模型
    /// 共用的、不包含敏感数据的
    /// </summary>
    public class UserSelectDto : EntityDto<long>//, IUserForSelectDto
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        public  string? Name { get; set; }
        /// <summary>
        /// 邮箱地址
        /// </summary>
        [Display(Name = "邮箱地址")]
        public  string? EmailAddress { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        [Display(Name = "手机号码")]
        public  string? PhoneNumber { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        [Display(Name = "角色")]
        public string[]? RoleNames { get; set; }
    }
}
