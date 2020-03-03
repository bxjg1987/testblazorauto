using System.Threading.Tasks;
using Abp.Application.Services;
using ZLJ.Sessions.Dto;

namespace ZLJ.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
