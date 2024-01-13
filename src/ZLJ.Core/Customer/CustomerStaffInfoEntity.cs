using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BXJG.Utils.GeneralTree;
using BXJG.Utils.BusinessUser;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using ZLJ.Core.Authorization.Users;
using ZLJ.Core.BaseInfo.Administrative;
using ZLJ.Core.BaseInfo.Post;
using BXJG.Common.Extensions;
using Abp.Timing;
using ZLJ.Core.BaseInfo.AssociatedCompany;
using BXJG.Utils.Localization;

namespace ZLJ.Core.Customer
{
    /// <summary>
    /// 员工档案实体类
    /// </summary>
    public class CustomerStaffInfoEntity : User, IMustHaveCustomer
    {
        /// <summary>
        /// 客户id
        /// </summary>
        public long CustomerId { get; set; }
        /// <summary>
        /// 客户
        /// </summary>
        public virtual AssociatedCompanyEntity Customer { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public BXJG.Common.Gender Gender { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTimeOffset? Birthday { get; set; }

        /*
         * 可用用ef的侦听器、事件、过滤器来实现指定的字段加密，通常用Aes对称加密
         * 但这样就强依赖ef了
         * 
         * 或手动做加解密
         * 
         * 目前直接明文，反正也不允许客户用户登陆系统
         */
        //[Protected]
        public string EquipmentPwd { get; set; } = "123";
    }
}