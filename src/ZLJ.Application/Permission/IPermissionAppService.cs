using Abp.Application.Navigation;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ZLJ.Authorization.Permissions
{
    /// <summary>
    /// 权限管理类(应用层)
    /// </summary>
    public interface IPermissionAppService : IApplicationService
    {
        /// <summary>
        /// 获取系统所有权限定义(IPermissionManager没有提供异步的，暂时用个同步的吧，以后自己加一个异步的)
        /// </summary>
        /// <returns></returns>
        IList<ZLJ.Dto.TreeNodeDto> GetAllPermissions();

        Task<IReadOnlyList< UserMenu>> GetMenusAsync();
    }
}
