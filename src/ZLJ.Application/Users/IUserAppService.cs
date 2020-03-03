using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ZLJ.Roles.Dto;
using ZLJ.Users.Dto;

namespace ZLJ.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>
    {
        Task<ListResultDto<RoleDto>> GetRoles();
        Task<IEnumerable<long>> DeleteBatchAsync(IList<long> input);
        Task ChangeLanguage(ChangeUserLanguageDto input);
    }
}
