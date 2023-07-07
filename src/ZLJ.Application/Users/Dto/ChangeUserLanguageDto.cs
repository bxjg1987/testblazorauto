using System.ComponentModel.DataAnnotations;

namespace ZLJ.App.Admin.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}