using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;
using Abp.Extensions;
using ZLJ.Core.BaseInfo.StaffInfo;

namespace ZLJ.Core.Authorization.Users
{
    public class User : AbpUser<User>
    {   
        /// <summary>
        /// 是否关联登录
        /// </summary>
        public bool IsEnableAccount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(ZLJ.Core.Share.ZLJConsts.RemarkMaxLength)]
        public string? Remark { get; set; }
        public const string DefaultPassword = "123qwe";
        /// <summary>
        /// 拼音首字母
        /// </summary>
        public string Pinyin { get; set; }
        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }
        public virtual ICollection<UserOrganizationUnit> OrganizationUnits { get; set; }
        public static User CreateTenantAdminUser(int tenantId, string emailAddress)
        {
            var user = new StaffInfoEntity
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress, 
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();

            return user;
        }
    }
}
