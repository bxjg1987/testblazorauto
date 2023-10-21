using Abp.Application.Services.Dto;
using ZLJ.App.Admin.Roles.Dto;

namespace ZLJ.App.Admin.Post.Dto
{
    public class PagedPostResultRequestDto : PagedRoleResultRequestDto, IDynamicCondition, IHaveKeywords
    {
        public string OuCode { get; set; }
        public string Permission { get; set; }
        public string Keywords { get; set; }
       public IEnumerable<ConditionFieldDefine> Conditions { get; set; }
    }
}

