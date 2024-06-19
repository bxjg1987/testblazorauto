using System.ComponentModel.DataAnnotations;

namespace ZLJ.Application.Common.Share.UserCenter
{
    public class ChangePasswordDto
    {
        [Display(Name = "旧密码")]
        [Required]
        public string CurrentPassword { get; set; }
        [Display(Name ="新密码")]
        [Required]
        public string NewPassword { get; set; }
        [Compare(nameof( NewPassword) ,ErrorMessage = "新密码两次输入不一致！")]
        [Display(Name = "新密码确认")]
        [Required]
        public string NewPassword1 { get; set; }

     
    }
}
