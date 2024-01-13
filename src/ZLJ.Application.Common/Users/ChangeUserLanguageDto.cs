using System.ComponentModel.DataAnnotations;

namespace ZLJ.Application.Common.Users
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}