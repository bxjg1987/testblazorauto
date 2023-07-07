using System.ComponentModel.DataAnnotations;

namespace ZLJ.App.Common.Users
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}