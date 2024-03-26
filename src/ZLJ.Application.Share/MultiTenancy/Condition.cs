using Abp.Application.Services.Dto;
using BXJG.Common.Contracts;

namespace ZLJ.Application.Share.MultiTenancy
{
    public class Condition:IHaveKeywords //: PagedResultRequestDto
    {
        public bool? IsActive { get; set; }
        public string? Keywords { get; set; }
    }
}

