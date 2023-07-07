using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ZLJ.App.Admin.Roles.Dto;
using ZLJ.App.Admin.Users.Dto;

namespace ZLJ.App.Admin.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedUserResultRequestDto, CreateUserDto, EditUserDto>
    {
        Task<ListResultDto<RoleDto>> GetRoles();
        Task<IEnumerable<long>> DeleteBatchAsync(IList<long> input);
        Task ChangeLanguage(ChangeUserLanguageDto input);
    }
}
