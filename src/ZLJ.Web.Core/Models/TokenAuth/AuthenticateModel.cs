using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;

namespace ZLJ.Models.TokenAuth
{
    public class AuthenticateModel
    {
        [Required]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string UserNameOrEmailAddress { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        public string Password { get; set; }

        public string? TenancyName { get; set; }

        public bool RememberClient { get; set; }
    }
}
