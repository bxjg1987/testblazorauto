using Abp.AspNetCore.Mvc.Authorization;
using Abp.AspNetCore.Mvc.Controllers;
using Abp.Authorization;
using Abp.Web.Configuration;
using Abp.Web.Models.AbpUserConfiguration;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Web.Controllers
{
    /*
     * abp默认提供的AbpUserConfigurationController会一次性加载所有配置
     * 因为它仅考虑传统的前后端分离场景
     * 
     * 而我们使用的是blazor auto模式
     * 无论是静态、server、wasm、还是auto，刷新页面是总是走的http请求
     * 最初是以静态方式请求的，此时页面权限会走传统的方式
     * 所以需要在http请求时，获取当前用户拥有的权限字符串，以便请求时执行的权限能通过。
     * 这里仅仅需要获取已授权的字符串。
     * 
     * 而后续在路由中获取全局配置，存储在appcontainer中
     *
     */

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AbpUserConfigurationController : AbpController
    {
        //private readonly AbpUserConfigurationBuilder _abpUserConfigurationBuilder;

        //public AbpUserConfigurationController(AbpUserConfigurationBuilder abpUserConfigurationBuilder)
        //{
        //    _abpUserConfigurationBuilder = abpUserConfigurationBuilder;
        //}
        //[AbpMvcAuthorize]
        [HttpGet]
        public async Task<AbpUserAuthConfigDto> GetAllGrantedPermissions()
        {
            //AbpUserConfigurationBuilder提供了对应的方法，不过它居然是protected的

            var config = new AbpUserAuthConfigDto();

            var allPermissionNames = PermissionManager.GetAllPermissions(false).Select(p => p.Name).ToList();
            var grantedPermissionNames = new List<string>();

            if (AbpSession.UserId.HasValue)
            {
                foreach (var permissionName in allPermissionNames)
                {
                    if (await PermissionChecker.IsGrantedAsync(permissionName))
                    {
                        grantedPermissionNames.Add(permissionName);
                    }
                }
            }

            config.AllPermissions = allPermissionNames.ToDictionary(permissionName => permissionName, permissionName => "true");
            config.GrantedPermissions = grantedPermissionNames.ToDictionary(permissionName => permissionName, permissionName => "true");

            return config;

        }
    }
}
