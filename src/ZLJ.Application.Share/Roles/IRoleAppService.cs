using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BXJG.Common.Dto;
using BXJG.Utils;

using BXJG.Utils.Application.Share;

namespace ZLJ.Application.Share.Roles
{
    public interface IRoleAppService : ICrudBaseAppService <RoleDto, int, PagedAndSortedResultRequest< PagedRoleResultRequestDto>,  CreateRoleDto, RoleEditDto>
    {
        //Task<ListResultDto<PermissionDto>> GetAllPermissions();

        //Task<GetRoleForEditOutput> GetRoleForEdit(EntityDto input);

        //Task<IList<RoleDto>> GetRolesAsync(GetRolesInput input);
        //Task<IReadOnlyList<RoleSelectDto>> GetForSelectAsync(GetForSelectInput a );
        //Task<IEnumerable<int>> DeleteBatchAsync(params int[] input);
    }
}
