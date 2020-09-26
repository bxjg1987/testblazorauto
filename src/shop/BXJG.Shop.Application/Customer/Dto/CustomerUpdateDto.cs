using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using BXJG.Common;
using BXJG.Utils.Enums;

namespace BXJG.Shop.Customer
{
    /// <summary>
    /// 更新上架模型时前端提供的数据模型
    /// </summary>
    public class CustomerUpdateDto : EntityDto<long>
    {
        #region abp用户信息

        //public long UserId { get; set; }
        
        /// <summary>
        /// 登录名
        /// </summary>
        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        //identity中姓、名是分开的。我们这里不区分，之所以用FullName是为了与查询模型CustomerDto匹配

        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        //[Required]
        //[StringLength(AbpUserBase.MaxSurnameLength)]
        //public string Surname { get; set; }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }
        /// <summary>
        /// 是否已激活，激活才允许登陆
        /// </summary>
        public bool IsActive { get; set; }

        //顾客的角色目前考虑固定，使用静态角色Customer
        //public string[] RoleNames { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [Required]
        [StringLength(AbpUserBase.MaxPhoneNumberLength)]
        public string PhoneNumber { get; set; }
        #endregion
        /// <summary>
        /// 所属地区Id
        /// </summary>
        public long? AreaId { get; set; }
        ///// <summary>
        ///// 顾客的积分
        ///// </summary>
        //public long Integral { get; set; }
        ///// <summary>
        ///// 总消费金额
        ///// </summary>
        //public decimal Amount { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public Gender Gender { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTimeOffset? Birthday { get; set; }
    }
}
