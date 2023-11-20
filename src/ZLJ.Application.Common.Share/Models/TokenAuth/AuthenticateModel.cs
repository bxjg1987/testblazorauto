using System.ComponentModel.DataAnnotations;

namespace ZLJ.Application.Common.Share.Models.TokenAuth
{
    public class AuthenticateModel
    {
        [Required]
        [StringLength(256)]
        public string UserNameOrEmailAddress { get; set; }

        [Required]
        [StringLength(32)]
        public string Password { get; set; }

        public string? TenancyName { get; set; }

        public bool RememberClient { get; set; }
    }
}
