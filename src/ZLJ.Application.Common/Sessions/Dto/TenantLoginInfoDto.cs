using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ZLJ.MultiTenancy;

namespace ZLJ.App.Common.Sessions.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class TenantLoginInfoDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }
    }
}
