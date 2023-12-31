using Abp.Application.Services.Dto;

namespace ZLJ.Application.Share.Roles
{
    public class PagedRoleResultRequestDto:IHaveKeywords, IDynamicCondition//: PagedAndSortedResultRequestDto
    {
        public string? Keywords { get; set; }
        public IEnumerable<ConditionFieldDefine>? Conditions { get; set; }
    }
}

