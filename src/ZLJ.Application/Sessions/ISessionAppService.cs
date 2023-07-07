using System.Threading.Tasks;
using Abp.Application.Services;
using ZLJ.App.Admin.Sessions.Dto;

namespace ZLJ.App.Admin.Sessions
{
    public interface ISessionAppService : Common.Sessions.Dto.UserLoginInfoDto
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
