using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using ZLJ.Authorization;
using ZLJ.Authorization.Roles;
using ZLJ.Authorization.Users;
using ZLJ.Roles.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Abp.Zero.Configuration;
using System;
using BXJG.GeneralTree;
using BXJG.Common.Dto;

namespace ZLJ.Roles
{
    //[AbpAuthorize(PermissionNames.Pages_Roles)]
    public class RoleAppService : AsyncCrudAppService<Role, RoleDto, int, PagedRoleResultRequestDto, CreateRoleDto, RoleDto>, IRoleAppService
    {
        private readonly RoleManager _roleManager;
        private readonly UserManager _userManager;
        readonly IRoleManagementConfig roleManagementConfig;

        public RoleAppService(IRepository<Role> repository, RoleManager roleManager, UserManager userManager,
            IRoleManagementConfig roleManagementConfig)
            : base(repository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            this.roleManagementConfig=roleManagementConfig;

            LocalizationSourceName = ZLJConsts.LocalizationSourceName;

            base.CreatePermissionName = Authorization.PermissionNames.AdministratorSystemRoleAdd;
            base.UpdatePermissionName = Authorization.PermissionNames.AdministratorSystemRoleUpdate;
            base.DeletePermissionName = Authorization.PermissionNames.AdministratorSystemRoleDelete;
            base.GetPermissionName = Authorization.PermissionNames.AdministratorSystemRole;
            base.GetAllPermissionName = Authorization.PermissionNames.AdministratorSystemRole;
        }

        public override async Task<RoleDto> CreateAsync(CreateRoleDto input)
        {
            CheckCreatePermission();

            var role = ObjectMapper.Map<Role>(input);
            role.SetNormalizedName();

            CheckErrors(await _roleManager.CreateAsync(role));

            var grantedPermissions = PermissionManager
                .GetAllPermissions()
                .Where(p => input.GrantedPermissions.Contains(p.Name))
                .ToList();

            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);

            return MapToEntityDto(role);
        }

        public async Task<ListResultDto<RoleListDto>> GetRolesAsync(GetRolesInput input)
        {
            var roles = await _roleManager
                .Roles
                .WhereIf(
                    !input.Permission.IsNullOrWhiteSpace(),
                    r => r.Permissions.Any(rp => rp.Name == input.Permission && rp.IsGranted)
                )
                .ToListAsync();

            return new ListResultDto<RoleListDto>(ObjectMapper.Map<List<RoleListDto>>(roles));
        }

        public override async Task<RoleDto> UpdateAsync(RoleDto input)
        {
            #region 默认代码
            //CheckUpdatePermission();

            //var role = await _roleManager.GetRoleByIdAsync(input.Id);

            //ObjectMapper.Map(input, role);

            //CheckErrors(await _roleManager.UpdateAsync(role));

            //var grantedPermissions = PermissionManager
            //    .GetAllPermissions()
            //    .Where(p => input.GrantedPermissions.Contains(p.Name))
            //    .ToList();

            //await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);

            //return MapToEntityDto(role);
            #endregion
            CheckUpdatePermission();
            var role = await _roleManager.GetRoleByIdAsync(input.Id);

            if (roleManagementConfig.StaticRoles.Any(c => c.RoleName.Equals(input.Name, StringComparison.Ordinal)))
                role.IsStatic = true;

            //var oldRoleNames = role.Permissions.Select(c => c.Name).ToList();

            //暂时去掉不允许修改超级管理员权限问题
            if (false)
            {
                //不判断，前端直接禁用控件
                //  if (input.Permissions.SequenceEqual(oldRoleNames))
                //if (input.Permissions.Count != oldRoleNames.Count||!input.Permissions.All(oldRoleNames.Contains))
                //       throw new UserFriendlyException("不允许修改静态角色的权限");

                //if (input.Name != role.Name)
                //    throw new UserFriendlyException("静态角色的标识名不允许修改");
                //role.Id = input.Id;
                role.Description = input.Description;
                role.DisplayName = input.DisplayName;
                //CheckErrors(await roleManager.UpdateAsync(role));
            }
            else
            {
                ObjectMapper.Map(input, role);
                //CheckErrors(await roleManager.UpdateAsync(role));
                if (input.GrantedPermissions == null)
                    input.GrantedPermissions = new List<string>();
                var grantedPermissions = PermissionManager
                    .GetAllPermissions()
                    .Where(p => input.GrantedPermissions.Contains(p.Name))
                    .ToList();

                await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);
            }
            role.SetNormalizedName();//如果不加 NormalizedName属性不会自动设置 会引起莫名其妙的异常
            await UnitOfWorkManager.Current.SaveChangesAsync();
            var dto = ObjectMapper.Map<RoleDto>(role);
            dto.GrantedPermissions = (await _roleManager.GetGrantedPermissionsAsync(role)).Select(c => c.Name).ToList();
            return dto;
        }

        public override async Task DeleteAsync(EntityDto<int> input)
        {
            CheckDeletePermission();

            var role = await _roleManager.FindByIdAsync(input.Id.ToString());
            var users = await _userManager.GetUsersInRoleAsync(role.NormalizedName);

            foreach (var user in users)
            {
                CheckErrors(await _userManager.RemoveFromRoleAsync(user, role.NormalizedName));
            }

            CheckErrors(await _roleManager.DeleteAsync(role));
        }

        public Task<ListResultDto<PermissionDto>> GetAllPermissions()
        {
            var permissions = PermissionManager.GetAllPermissions();

            return Task.FromResult(new ListResultDto<PermissionDto>(
                ObjectMapper.Map<List<PermissionDto>>(permissions).OrderBy(p => p.DisplayName).ToList()
            ));
        }

        protected override IQueryable<Role> CreateFilteredQuery(PagedRoleResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Permissions)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Keyword)
                || x.DisplayName.Contains(input.Keyword)
                || x.Description.Contains(input.Keyword));
        }

        protected override async Task<Role> GetEntityByIdAsync(int id)
        {
            return await Repository.GetAllIncluding(x => x.Permissions).FirstOrDefaultAsync(x => x.Id == id);
        }

        protected override IQueryable<Role> ApplySorting(IQueryable<Role> query, PagedRoleResultRequestDto input)
        {
            return query.OrderBy(r => r.DisplayName);
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        public async Task<GetRoleForEditOutput> GetRoleForEdit(EntityDto input)
        {
            var permissions = PermissionManager.GetAllPermissions();
            var role = await _roleManager.GetRoleByIdAsync(input.Id);
            var grantedPermissions = (await _roleManager.GetGrantedPermissionsAsync(role)).ToArray();
            var roleEditDto = ObjectMapper.Map<RoleEditDto>(role);

            return new GetRoleForEditOutput
            {
                Role = roleEditDto,
                Permissions = ObjectMapper.Map<List<FlatPermissionDto>>(permissions).OrderBy(p => p.DisplayName).ToList(),
                GrantedPermissionNames = grantedPermissions.Select(p => p.Name).ToList()
            };
        }

        [AbpAuthorize(Authorization.PermissionNames.AdministratorSystemRoleDelete)]
        public async Task<IEnumerable<int>> DeleteBatchAsync(params int[] input)
        {
            var list = new List<int>();
            foreach (var item in input)
            {
                var role = await _roleManager.GetRoleByIdAsync(item);
                if (role.IsStatic)
                {
                    continue;
                    //throw new UserFriendlyException("CannotDeleteAStaticRole");
                }
                var rt = await _roleManager.DeleteAsync(role);
                if (rt.Succeeded)
                    list.Add(role.Id);
            }
            return list;
        }

        public async Task<IReadOnlyList<RoleSelectDto>> GetForSelectAsync(GetForSelectInput a)
        {
                var list = await _roleManager.Roles.OrderBy(c => c.DisplayName).Select(c => new RoleSelectDto
                {
                    Value = c.Id.ToString(),
                    DisplayText = c.DisplayName,
                    Name = c.Name
                }).ToListAsync();
                if (a.ForType == 0)
                    list.Insert(0, new RoleSelectDto(null, string.IsNullOrWhiteSpace(a.ParentText) ? L("Please select role") : a.ParentText));
                else if (a.ForType == 1)
                    list.Insert(0, new RoleSelectDto(null, string.IsNullOrWhiteSpace(a.ParentText) ? L("Please select") : a.ParentText));
                return list;
          
           
        }

    }
}

