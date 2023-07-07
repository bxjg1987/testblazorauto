using Abp.Application.Services.Dto;

namespace ZLJ.App.Admin.Roles.Dto
{
    public class PagedRoleResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}

