using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BXJG.Common;
using BXJG.Utils.Enums;
using BXJG.Utils.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public string Password { get; set; }
        #endregion
        /// <summary>
        /// 所属地区id
        /// </summary>
        public long? AreaId { get; set; }
        /// <summary>
        /// 所属地区
        /// </summary>
        public string AreaDisplayName { get; set; }
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
        /// <summary>
        /// 收货地址列表
        /// </summary>
        public List<ShippingAddressDto> Addresses { get; set; }
    }
    /// <summary>
    /// 后台管理顾客地址
    /// </summary>
    public class ShippingAddressDto : EntityDto<long>
    {
        /// <summary>
        /// 所属顾客id
        /// </summary>
        public long CustomerId { get; set; }
        /// <summary>
        /// 联系人名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 所属区域Id
        /// </summary>
        public long AreaId { get; set; }
        /// <summary>
        /// 所属区域
        /// </summary>
        public string AreaDislayName { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public string ZipCode { get; set; }

        //另一种方式是在CustomerEntity上定义默认收货地址的id，这样收货地址这个表可以少个字段，但是相比之下目前的方式实现起来更简单直观
        /// <summary>
        /// 是否为默认收货信息
        /// </summary>
        public bool IsDefault { get; set; }
        /// <summary>
        /// 扩展字段
        /// 目前没有想到它的使用场景，只是考虑其它系统引入商城模块时可以用这种简单的扩展方式扩展
        /// </summary>
        public object ExtensionData { get; set; }
    }
}
