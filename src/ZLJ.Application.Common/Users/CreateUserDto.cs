using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Runtime.Validation;
using ZLJ.Authorization.Users;

namespace ZLJ.App.Common.Users
{
    [AutoMapTo(typeof(User))]
    public class CreateUserDto : EntityDto<long>, IShouldNormalize
    {

        [Required(ErrorMessage = "请输入登录名")]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }


        [Required(ErrorMessage ="请输入姓名")]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        //[Required]
        //[StringLength(AbpUserBase.MaxSurnameLength)]
        //public string Surname { get; set; }

        [Required]
        //[Phone]
        [StringLength(AbpUserBase.MaxPhoneNumberLength)]
        public string PhoneNumber { get; set; }

        [Required(AllowEmptyStrings = true)]
        //[EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        /*
         * 目前不考虑禁用，因为txdl未实现禁用，若这里禁用了会导致txdl删除用户
         * 而启用后，txdl会新增一个用户，业务系统用户还是原来的，但txdl那边的用户是新增的
         * txdl那边用户新采集的数量可能很少，因为是新用户，由于用户同步使用业务系统的用户id，上传的数据会跟业务系统对应上
         * 最终导致业务系统中用户采集量出现负数
         * 
         * 2023-1-1调整
         * txdl需要实现客户员工的禁用功能
         */
        public bool IsActive { get; set; }

        public string[] RoleNames { get; set; }

        [Required(ErrorMessage = "请设置密码")]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }

        public void Normalize()
        {
            if (RoleNames == null)
            {
                RoleNames = new string[0];
            }
        }
    }

    [AutoMapTo(typeof(User))]
    public class EditUserDto : EntityDto<long>, IShouldNormalize
    {
        //[Required]
        //[StringLength(AbpUserBase.MaxUserNameLength)]
        //public string UserName { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        //[Required]
        //[StringLength(AbpUserBase.MaxSurnameLength)]
        //public string Surname { get; set; }

        [Required]
        //[Phone]
        [StringLength(AbpUserBase.MaxPhoneNumberLength)]
        public string PhoneNumber { get; set; }

        [Required(AllowEmptyStrings = true)]
        //[EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        public bool IsActive { get; set; }

        public string[] RoleNames { get; set; }

        //[Required]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }

        public void Normalize()
        {
            if (RoleNames == null)
            {
                RoleNames = new string[0];
            }
        }
    }
}
