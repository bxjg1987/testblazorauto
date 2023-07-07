using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ZLJ.MultiTenancy;

namespace ZLJ.App.Admin.Sessions.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class TenantLoginInfoDto  : Common.Sessions.Dto.TenantLoginInfoDto
    {
    }
}
