using Abp.Application.Services.Dto;
using ZLJ.App.Admin.Roles.Dto;

namespace ZLJ.App.Admin.Post.Dto
{
    public class PagedPostResultRequestDto : PagedRoleResultRequestDto, IDynamicCondition
    {
       // public string Keyword { get; set; }
       public IEnumerable<ConditionFieldDefine> Conditions { get; set; }
    }
}

