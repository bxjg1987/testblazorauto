using Abp.Application.Services.Dto;

namespace ZLJ.Application.Share.MultiTenancy
{
    public class Condition:IHaveKeywords //: PagedResultRequestDto
    {
        public bool? IsActive { get; set; }
        public string? Keywords { get; set; }
    }
}

