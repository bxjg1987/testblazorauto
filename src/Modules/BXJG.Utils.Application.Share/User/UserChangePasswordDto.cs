using System.ComponentModel.DataAnnotations;

namespace BXJG.Utils.Application.Share.User
{
    /// <summary>
    /// 用户自己修改密码时提交的参数模型
    /// </summary>
    public class UserChangePasswordDto
    {
        /// <summary>
        /// 当前密码
        /// </summary>
        [Required]
        public string CurrentPassword { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        [Required]
        public string NewPassword { get; set; }
    }
}
