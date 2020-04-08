using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Customer.Dto
{
    /// <summary>
    /// 后台管理页使用的DTO
    /// </summary>
    [AutoMapFrom(typeof(CustomerEntity<>))]
    public class CustomerDto : FullAuditedEntityDto<long>
    {
        #region abp用户信息
        /// <summary>
        /// 关联到abp用户的id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 关联到abp用户
        /// </summary>
        public string UserFullName { get; set; }
        /// <summary>
        /// 应该是登录名
        /// </summary>
        public string UserUserName { get; set; }

        public string UserSurname { get; set; }
        public string UserEmailAddress { get; set; }
        public bool UserIsActive { get; set; }
        public string UserPhoneNumber { get; set; }
        #endregion

        /// <summary>
        /// 顾客的积分
        /// </summary>
        public long Integral { get; set; }
        /// <summary>
        /// 总消费金额
        /// </summary>
        public decimal Amount { get; set; }
    }
}
