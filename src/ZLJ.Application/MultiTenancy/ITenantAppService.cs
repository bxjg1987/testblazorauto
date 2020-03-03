using Abp.Application.Services;
using ZLJ.MultiTenancy.Dto;

namespace ZLJ.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

