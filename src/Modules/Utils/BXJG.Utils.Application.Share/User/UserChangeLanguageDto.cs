using System.ComponentModel.DataAnnotations;

namespace BXJG.Utils.Application.Share.User
{
    /// <summary>
    /// 用户修改自己当前的语言的输入参数模型
    /// </summary>
    public class UserChangeLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}