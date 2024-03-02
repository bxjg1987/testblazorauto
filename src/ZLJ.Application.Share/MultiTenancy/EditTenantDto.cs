using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.MultiTenancy;
using ZLJ.Core.Share;

namespace ZLJ.Application.Share.MultiTenancy
{
    //[AutoMapTo(typeof(Tenant))]
    public class EditTenantDto:EntityDto<int>
    {
        [Required]
        [StringLength(ZLJConsts.MaxTenancyNameLength)]
       [RegularExpression(ZLJConsts.TenancyNameRegex)]
        [DisplayName("租户唯一名")]
        public string TenancyName { get; set; }

        [Required]
        [StringLength(ZLJConsts.MaxNameLength)]
        [DisplayName("租户显示名")]
        public string Name { get; set; }

        [Required]
       [StringLength(ZLJConsts.MaxEmailAddressLength)]
        [DisplayName("管理员邮箱")] public string AdminEmailAddress { get; set; }

       [StringLength(ZLJConsts.MaxConnectionStringLength)]
        [DisplayName("连接字符串")] public string ConnectionString { get; set; }
        [DisplayName("启用")]
        public bool IsActive { get; set; }
    }
}
