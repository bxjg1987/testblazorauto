using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Navigation;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Localization;
using Abp.UI;
using ZLJ.Dto;

namespace ZLJ.Authorization.Permissions
{
    public class PermissionAppService : ABPAppServiceBase, IPermissionAppService
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
        public PermissionAppService
        (IUserNavigationManager sdf)
        {
            this.sdf = sdf;
        }
        [AbpAuthorize]
        public IList<TreeNodeDto> GetAllPermissions()
        {
            //try
            //{
          
                var permissions = PermissionManager.GetAllPermissions();
                var list = permissions.Select(c => new ZLJ.Dto.TreeNodeDto
                {
                    id = c.Name,
                    text = c.DisplayName.Localize(new LocalizationContext(base.LocalizationManager)),
                    state = c.Children != null && c.Children.Count > 0 ? "closed" : "open",
                    parentId = c.Parent != null ? c.Parent.Name : null
                }).ToList();
                list.ForEach(c => c.children = list.Where(d => d.parentId == c.id).ToList());
                return list.Where(c => c.parentId == PermissionNames.Administrator).ToList();
            //}
            //catch (System.Exception ex)
            //{

            //    throw new UserFriendlyException(ex.Message);
            //}

        }
        [AbpAuthorize]
        public Task<IReadOnlyList<UserMenu>> GetMenusAsync()
        {
          return sdf.GetMenusAsync  ( new Abp.UserIdentifier(base.AbpSession.TenantId, base.AbpSession.UserId.Value));
        }
    }
}