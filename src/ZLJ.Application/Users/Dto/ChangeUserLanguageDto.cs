using System.ComponentModel.DataAnnotations;

namespace ZLJ.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}