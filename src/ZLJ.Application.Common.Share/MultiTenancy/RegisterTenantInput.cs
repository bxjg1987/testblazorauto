using Abp.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Core.Share;

namespace ZLJ.Application.Common.Share.MultiTenancy
{
    /// <summary>
    /// 注册租户的输入参数模型
    /// </summary>
    public class RegisterTenantInput
    {
        [Required]
        [StringLength(ZLJConsts.MaxNameLength)]
        [DisplayName("租户显示名")]
        public string Name { get; set; }
        [Required]
        [StringLength(32)]
        [DisableAuditing]
        public string AdminUserName { get; set; } = "admin";
        [Required]
        [StringLength(32)]
        [DisableAuditing]
        public string AdminPassword { get; set; }
        [Required]
        [StringLength(32)]
        [DisableAuditing]
        [Compare("AdminPassword", ErrorMessage = "密码和确认密码不一致")]
        public string AdminPassword2 { get; set; }
        [Required]
        [StringLength(200)]
        [Display(Name="验证码")]
        public string YzmKey { get; set; }
        [Required]
        [StringLength(20)]
        [Display(Name = "验证码")]
        public string YzmValue { get; set; }
    }
}
