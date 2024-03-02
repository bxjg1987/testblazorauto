using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.MultiTenancy;
using ZLJ.Core.Share;

namespace ZLJ.Application.Share.MultiTenancy
{
   // [AutoMapFrom(typeof(Tenant))]
    public class TenantDto : EntityDto,IExtendableDto
    {
        public dynamic ExtensionData { get; set; }
        // [Required]
        // [StringLength(ZLJConsts.MaxTenancyNameLength)]
        //[RegularExpression(ZLJConsts.TenancyNameRegex)]
        [DisplayName("唯一名")] public string TenancyName { get; set; }

        //   [Required]
        // [StringLength(ZLJConsts.MaxNameLength)]
        [DisplayName("显示名")] public string Name { get; set; }

        [DisplayName("启用")] public bool IsActive { get; set; }
    }
}
