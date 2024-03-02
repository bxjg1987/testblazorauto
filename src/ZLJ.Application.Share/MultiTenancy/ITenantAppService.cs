using Abp.Application.Services;
using BXJG.Utils.Application.Share;

namespace ZLJ.Application.Share.MultiTenancy
{
    public interface ITenantAppService : Common.Share.ICrudBaseAppService<TenantDto, int, PagedAndSortedResultRequest<Condition>, EditTenantDto, EditTenantDto>
    {
    }
}

