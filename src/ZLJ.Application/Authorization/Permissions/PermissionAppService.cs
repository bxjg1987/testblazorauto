using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Navigation;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Extensions;
using Abp.Localization;
using Abp.UI;
using BXJG.Utils.Application.Share.GeneralTree;
using BXJG.Utils.GeneralTree;
using ZLJ.Application.Share.Authorization.Permissions;

namespace ZLJ.Application.Admin.Authorization.Permissions
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //要求登录
        IUserNavigationManager sdf;
        public PermissionAppService(IUserNavigationManager sdf)
        {
            this.sdf = sdf;
        }
        [AbpAuthorize]
        public IList<DataDictionaryForSelectDto> GetAllPermissions()
        {
            return null;
            ////若某个权限是用来被其它权限依赖的，那么在可选列表中不要显示
            //var permissions = PermissionManager.GetAllPermissions().Where(c => !c.GetDependentedPermissions().Any());
            //var list = permissions.Select(c => new GeneralTreeNodeDto
            //{
            //    Id = long.Parse( c.Name,
            //    Text = c.DisplayName?.Localize(base.LocalizationManager),
            //    //State = c.Children != null && c.Children.Count > 0 ? "closed" : "open",
            //    ParentId = c.Parent?.Name,
            //    ExtData = c.Properties// new { btn = c.Properties.ContainsKey("btn") && Convert.ToBoolean(c.Properties["btn"]) }
            //}).ToList();
            //list.ForEach(c => c.Children = list.Where(d => d.ParentId == c.Id).ToList());

            //return list.Where(c => c.ParentId.IsNullOrWhiteSpace()).ToList();

        }
        [AbpAuthorize]
        public Task<IReadOnlyList<UserMenu>> GetMenusAsync()
        {
            return sdf.GetMenusAsync(new Abp.UserIdentifier(base.AbpSession.TenantId, base.AbpSession.UserId.Value));
        }
    }
}