using System.Threading.Tasks;
using Abp.Application.Services;
using ZLJ.Application.Admin.Authorization.Accounts.Dto;

namespace ZLJ.Application.Admin.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
