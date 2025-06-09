using Abp.Application.Navigation;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Extensions;
using Abp.Localization;
using Abp.UI;
using BXJG.Utils.Application.Share.GeneralTree;
using BXJG.Utils.GeneralTree;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZLJ.Application.Share.Authorization.Permissions;
using ZLJ.Application.Share.Roles;

namespace ZLJ.Application.Authorization.Permissions
{
    public class PermissionAppService : AdminBaseAppService, IPermissionAppService
    {
        //public ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions()
        //{
        //    var permissions = PermissionManager.GetAllPermissions();
        //    var rootPermissions = permissions.Where(p => p.Parent == null);

        //    var result = new List<FlatPermissionWithLevelDto>();

        //    foreach (var rootPermission in rootPermissions)
        //    {
        //        var level = 0;
        //        AddPermission(rootPermission, permissions, result, level);
        //    }

        //    return new ListResultDto<FlatPermissionWithLevelDto>
        //    {
        //        Items = result
        //    };
        //}

        //private void AddPermission(Permission permission, IReadOnlyList<Permission> allPermissions, List<FlatPermissionWithLevelDto> result, int level)
        //{
        //    var flatPermission = permission.MapTo<FlatPermissionWithLevelDto>();
        //    flatPermission.Level = level;
        //    result.Add(flatPermission);

        //    if (permission.Children == null)
        //    {
        //        return;
        //    }

        //    var children = allPermissions.Where(p => p.Parent != null && p.Parent.Name == permission.Name).ToList();

        //    foreach (var childPermission in children)
        //    {
        //        AddPermission(childPermission, allPermissions, result, level + 1);
        //    }
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        ////要求登录
        //IUserNavigationManager sdf;
        //public PermissionAppService(IUserNavigationManager sdf)
        //{
        //    this.sdf = sdf;
        //}
        [AbpAuthorize]
        public IList<FlatPermissionDto> GetAllPermissions()
        {

            //代码是抄过来的，其实我们这里木有必要递归

            var permissions = PermissionManager.GetAllPermissions();
           
            var r = ObjectMapper.Map<IList<FlatPermissionDto>>(permissions).OrderBy(p => p.DisplayName).ToList();

            r.ForEach(x=>x.Children=Enumerable.Empty<FlatPermissionDto>());

            return r;
        }


        //public IList<FlatPermissionDto> GetAllPermissions()
        //{

        //    //代码是抄过来的，其实我们这里木有必要递归

        //    var permissions = PermissionManager.GetAllPermissions();

        //    var rootPermissions = permissions.Where( p => p.Parent==null);

        //    var result = new List<FlatPermissionDto>();

        //    foreach (var rootPermission in rootPermissions)
        //    {
        //        var level = 0;
        //        AddPermission(rootPermission, permissions, result, level);
        //    }

        //    return result;
        //}

        //private void AddPermission(Permission permission, IReadOnlyList<Permission> allPermissions, List<FlatPermissionDto> result, int level)
        //{
        //    var flatPermission = ObjectMapper.Map<FlatPermissionDto>(permission);
        //    flatPermission.Level = level;
        //    result.Add(flatPermission);

        //    if (permission.Children == null)
        //    {
        //        return;
        //    }

        //    var children = allPermissions.Where(p => p.Parent != null && p.Parent.Name == permission.Name).ToList();

        //    foreach (var childPermission in children)
        //    {
        //        AddPermission(childPermission, allPermissions, result, level + 1);
        //    }
        //}

        //[AbpAuthorize]
        //public Task<IReadOnlyList<UserMenu>> GetMenusAsync()
        //{
        //    return sdf.GetMenusAsync(new Abp.UserIdentifier(base.AbpSession.TenantId, base.AbpSession.UserId.Value));
        //}
    }
}