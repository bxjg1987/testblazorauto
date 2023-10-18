using Abp.Application.Services.Dto;

namespace ZLJ.App.Admin.Roles.Dto
{
    public class PagedRoleResultRequestDto:IHaveKeywords //: PagedAndSortedResultRequestDto
    {
        public string Keywords { get; set; }
    }
}

