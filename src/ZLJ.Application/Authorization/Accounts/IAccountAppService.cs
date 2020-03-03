using System.Threading.Tasks;
using Abp.Application.Services;
using ZLJ.Authorization.Accounts.Dto;

namespace ZLJ.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
