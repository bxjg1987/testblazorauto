using Abp.Application.Services;
using ZLJ.Application.Admin.MultiTenancy.Dto;

namespace ZLJ.Application.Admin.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

