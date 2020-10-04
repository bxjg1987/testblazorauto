using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BXJG.Common;
using BXJG.Utils.Enums;
using BXJG.Utils.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Customer
{
    /// <summary>
    /// 后台管理页使用的DTO
    /// </summary>
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
        public string Name { get; set; }
        /// <summary>
        /// 应该是登录名
        /// </summary>
        public string UserName { get; set; }
        //public DateTime UserLastLoginTime { get; set; }
        //public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public bool IsActive { get; set; }
        public string PhoneNumber { get; set; }
        #endregion
        /// <summary>
        /// 所属地区id
        /// </summary>
        public long? AreaId { get; set; }
        /// <summary>
        /// 所属地区
        /// </summary>
        public string AreaDisplayName{ get; set; }
        /// <summary>
        /// 顾客的积分
        /// </summary>
        public long Integral { get; set; }
        /// <summary>
        /// 总消费金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public Gender Gender { get; set; }
        /// <summary>
        /// 获取性别的本地化文本
        /// </summary>
        public string GenderText
        {
            get
            {
                return Gender.ToLocalizationString();
            }
        }
        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTimeOffset? Birthday { get; set; }
    }
}
