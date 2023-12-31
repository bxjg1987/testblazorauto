using Abp;
using Abp.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Web.Blazor.Abp
{
    //注意：服务端刚好是反的，是IAuthorizationService内部调用IPermissionChecker
    //目的是为了组件中统一使用IPermissionChekcer，并确保blazor授权标签、路由、组件等能正常工作。

    /// <summary>
    /// 客户端权限检查器
    /// </summary>
    public class ClientPermissionChecker : IPermissionChecker
    {
        IAuthorizationService _authorizationService;
        AuthenticationStateProvider authenticationState;

        public ClientPermissionChecker(IAuthorizationService authorizationService, AuthenticationStateProvider authenticationState)
        {
            _authorizationService = authorizationService;
            this.authenticationState = authenticationState;
        }

        public bool IsGranted(string permissionName)
        {
            return _authorizationService.AuthorizeAsync(authenticationState.GetAuthenticationStateAsync().ConfigureAwait(false).GetAwaiter().GetResult().User, permissionName)
                                         .ConfigureAwait(false).GetAwaiter().GetResult().Succeeded;
        }

        public bool IsGranted(UserIdentifier user, string permissionName)
        {
            //客户端部分 权限判断时 只会检查当前用户，所以这里传递参数直接忽略
            return IsGranted(permissionName);
        }

        public async Task<bool> IsGrantedAsync(string permissionName)
        {
            return (await _authorizationService.AuthorizeAsync((await authenticationState.GetAuthenticationStateAsync()).User, permissionName)).Succeeded;
        }

        public async Task<bool> IsGrantedAsync(UserIdentifier user, string permissionName)
        {
            //客户端部分 权限判断时 只会检查当前用户，所以这里传递参数直接忽略
            return await IsGrantedAsync(permissionName);
        }
    }
}
