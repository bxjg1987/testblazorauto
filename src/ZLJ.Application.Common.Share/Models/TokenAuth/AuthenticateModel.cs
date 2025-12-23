using Abp.Auditing;
using System.ComponentModel.DataAnnotations;

namespace ZLJ.Application.Common.Share.Models.TokenAuth
{
    public class AuthenticateModel
    {
        [DisableAuditing]
        [Required]
        [StringLength(256)]
        public string UserNameOrEmailAddress { get; set; }

        [DisableAuditing]
        [Required]
        [StringLength(32)]
        public string Password { get; set; }
        [DisableAuditing]
        public string? TenancyName { get; set; }

        public bool RememberClient { get; set; }
        //[Required]
        [StringLength(200)]
        public string? YzmKey { get; set; }
        //[Required]
        [StringLength(20)]
        public string? YzmValue { get; set; }
    }
}
