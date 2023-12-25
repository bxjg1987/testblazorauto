using Abp.Application.Services.Dto;

namespace ZLJ.App.Admin.Roles.Dto
{
    public class PagedRoleResultRequestDto:IHaveKeywords, IDynamicCondition//: PagedAndSortedResultRequestDto
    {
        public string Keywords { get; set; }
        public IEnumerable<ConditionFieldDefine> Conditions { get; set; }
    }
}

