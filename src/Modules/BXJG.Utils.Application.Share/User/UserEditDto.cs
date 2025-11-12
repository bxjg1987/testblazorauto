using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Runtime.Validation;

namespace BXJG.Utils.Application.Share.User
{
    //[AutoMapTo(typeof(User))]
    public class UserEditDto : EntityDto<long>, IUserEditDto, IShouldNormalize
    {
        //[Required]
        //[StringLength(AbpUserBase.MaxUserNameLength)]
        //public string UserName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [StringLength(256)]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        //[Required]
        //[StringLength(AbpUserBase.MaxSurnameLength)]
        //public string Surname { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Required]
        //[Phone]
        [StringLength(64)]
        [Display(Name = "手机号")]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 邮箱地址
        /// </summary>
        [Display(Name = "邮箱地址")]
        //[Required(AllowEmptyStrings = true)]
        //[EmailAddress]
        [StringLength(32)]
        public string? EmailAddress { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        [Display(Name = "是否启用")]
        public bool IsActive { get; set; }
        /// <summary>
        /// 是否修改密码
        /// </summary>
        [Display(Name = "修改密码")]
        public bool ChangePassword { get; set; }
        /// <summary>
        /// 密码
        /// 新增时必填
        /// </summary>
        [StringLength(32)]
        [DisableAuditing]
        [Display(Name = "密码")]
        public string? Password { get; set; } = "123qwe";
        ///// <summary>
        ///// 密码确认
        ///// </summary>
        //[StringLength(32)]
        //[DisableAuditing]
        //[Display(Name = "密码确认")]
        //[Compare(nameof(Password))]
        //public string? ConfirmPassword { get; set; }
        /// <summary>
        /// 多次登录失败锁定
        /// </summary>
        [Display(Name = "登录锁定")]
        public  bool IsLockoutEnabled { get; set; } = true;
        /// <summary>
        /// 分配到的角色
        /// </summary>
        public string[] RoleNames { get; set; }
        /// <summary>
        /// 所属组织机构
        /// </summary>
        public List<long> OrganizationUnits { get; set; }

        public void Normalize()
        {
            if (RoleNames == null)
            {
                RoleNames = Array.Empty<string>();
            }
        }
    }
}
