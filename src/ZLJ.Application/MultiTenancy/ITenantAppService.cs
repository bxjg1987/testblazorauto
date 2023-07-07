using Abp.Application.Services;
using ZLJ.App.Admin.MultiTenancy.Dto;

namespace ZLJ.App.Admin.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

