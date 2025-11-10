using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Runtime.Validation;

namespace BXJG.Utils.Application.Share.User
{
    //[AutoMapTo(typeof(User))]
    public class UserEditDto : EntityDto<long>,IUserEditDto, IShouldNormalize
    {
        //[Required]
        //[StringLength(AbpUserBase.MaxUserNameLength)]
        //public string UserName { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        //[Required]
        //[StringLength(AbpUserBase.MaxSurnameLength)]
        //public string Surname { get; set; }

        [Required]
        //[Phone]
        [StringLength(64)]
        public string PhoneNumber { get; set; }

        [Required(AllowEmptyStrings = true)]
        //[EmailAddress]
        [StringLength(32)]
        public string EmailAddress { get; set; }

        public bool IsActive { get; set; }

        public string[] RoleNames { get; set; }

        //[Required]
        [StringLength(32)]
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
