using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using ZLJ.Application.Admin.Roles.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Abp.Zero.Configuration;
using System;
using BXJG.Utils.GeneralTree;

using BXJG.Common.Extensions;
using Abp.Organizations;
using ZLJ.Application.Common.OU;
using Abp.Domain.Uow;
using ZLJ.Core.Authorization.Roles;
using ZLJ.Core.Authorization.Users;
using ZLJ.Application.Admin.Authorization.Permissions;
using BXJG.Utils.Application.Share;
using ZLJ.Application.Share.Authorization.Permissions;
using ZLJ.Application.Share.Roles;
using ZLJ.Application.Common.Share.OU;

namespace ZLJ.Application.Admin.Roles
{
    [AbpAuthorize(PermissionNames.AdministratorSystemRole)]
    public class RoleAppService : AdminCrudBaseAppService<Role, RoleDto, int, PagedAndSortedResultRequest<PagedRoleResultRequestDto>, CreateRoleDto, RoleEditDto>, IRoleAppService
    {
        private readonly RoleManager _roleManager;
        private readonly UserManager _userManager;
        readonly IRoleManagementConfig roleManagementConfig;
        IRepository<OrganizationUnitRole, long> ouRoleRepository;
        IRepository<OrganizationUnit, long> ouRepository;
        OrganizationUnitManager unitManager;

        public RoleAppService(IRepository<Role> repository, RoleManager roleManager, UserManager userManager,
            IRoleManagementConfig roleManagementConfig, IRepository<OrganizationUnitRole, long> ouRoleRepository, IRepository<OrganizationUnit, long> ouRepository, OrganizationUnitManager unitManager)
            : base(repository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            this.roleManagementConfig = roleManagementConfig;

            LocalizationSourceName = ZLJ.Core.Share.ZLJConsts.LocalizationSourceName;

            base.CreatePermissionName = PermissionNames.AdministratorSystemRoleAdd;
            base.UpdatePermissionName = PermissionNames.AdministratorSystemRoleUpdate;
            base.DeletePermissionName = PermissionNames.AdministratorSystemRoleDelete;
            base.GetPermissionName = PermissionNames.AdministratorSystemRole;
            base.GetAllPermissionName = PermissionNames.AdministratorSystemRole;
            this.ouRoleRepository = ouRoleRepository;
            this.ouRepository = ouRepository;
            this.unitManager = unitManager;
        }
        
        [UnitOfWork(false)]
        public override async Task<RoleDto> GetAsync(EntityDto<int> input)
        {
            var role = await GetEntityByIdAsync(input.Id);
            await sdfsdf(role);
            var dto = MapToEntityDto(role);
            dto.GrantedPermissions = (await _roleManager.GetGrantedPermissionsAsync(role)).Where(c => c.Children.Count == 0).Select(c => c.Name).ToList();
            return dto;
        }

        protected override RoleDto MapToEntityDto(Role role)
        {
            var dto = base.MapToEntityDto(role);
            var ous = CurrentUnitOfWork.Items["ous"] as IDictionary<int, IEnumerable<OrganizationUnit>>;
            if (ous != default)
                dto.Ous = ObjectMapper.Map<List<OuDto>>(ous[role.Id].Where(c => c != default));
            return dto;
        }

        async Task sdfsdf(Role entity)
        {
            var q2 = from role in Repository.GetAll().AsNoTrackingWithIdentityResolution()
                     join ouRole in ouRoleRepository.GetAll().AsNoTrackingWithIdentityResolution() on role.Id equals ouRole.RoleId into tem1
                     from ouRole in tem1.DefaultIfEmpty()
                     join ou in ouRepository.GetAll().AsNoTrackingWithIdentityResolution() on ouRole.OrganizationUnitId equals ou.Id into tem2
                     from ou in tem2.DefaultIfEmpty()
                     where role.Id == entity.Id
                     select new { role, ou };
            var list = await q2.ToListAsync(CancellationTokenProvider.Token);
            var groups = list.GroupBy(c => c.role, c => c.ou);
            CurrentUnitOfWork.Items["ous"] = groups.ToDictionary(c => c.Key.Id, c => c.AsEnumerable());
        }

        public override async Task<RoleDto> CreateAsync(CreateRoleDto input)
        {
            CheckCreatePermission();
            //input.nam
            var role = ObjectMapper.Map<Role>(input);
            role.Name = TinyPinyin.PinyinHelper.GetPinyin(input.DisplayName, "");//.GetPinYinFirstLetter();
            role.SetNormalizedName();

            CheckErrors(await _roleManager.CreateAsync(role));

            await base.CurrentUnitOfWork.SaveChangesAsync();

            if (input.OuIds != null)
            {
                foreach (var item in input.OuIds)
                {
                    await _roleManager.AddToOrganizationUnitAsync(role.Id, item, base.AbpSession.TenantId);
                }
            }
            // 
            //unitManager.role

            var grantedPermissions = PermissionManager
                .GetAllPermissions()
                .Where(p => input.GrantedPermissions.Contains(p.Name))
                .ToList();

            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);
            await CurrentUnitOfWork.SaveChangesAsync();
            await sdfsdf(role);
            var dto = MapToEntityDto(role);
            // var dto = ObjectMapper.Map<RoleDto>(role);
            dto.GrantedPermissions = (await _roleManager.GetGrantedPermissionsAsync(role)).Select(c => c.Name).ToList();
            return dto;
        }

        [UnitOfWork(false)]
        public async Task<IList<RoleDto>> GetRolesAsync(GetRolesInput input)
        {

            var q2 = from role in Repository.GetAll().AsNoTrackingWithIdentityResolution()
                                                     .Where(c => EF.Property<string>(c, "Discriminator") == "Role")//ef也支持将某个实体属性作为鉴别列
                     join ouRole in ouRoleRepository.GetAll().AsNoTrackingWithIdentityResolution() on role.Id equals ouRole.RoleId into tem1
                     from ouRole in tem1.DefaultIfEmpty()
                     join ou in ouRepository.GetAll().AsNoTrackingWithIdentityResolution() on ouRole.OrganizationUnitId equals ou.Id into tem2
                     from ou in tem2.DefaultIfEmpty()
                     select new { role, ou };

            q2 = q2.WhereIf(!input.OuCode.IsNullOrWhiteSpace(), c => c.ou.Code.StartsWith(input.OuCode));

            q2 = q2.OrderBy(c => c.role.DisplayName);

            q2 = from role in q2.Select(c => c.role).Distinct()
                 join ouRole in ouRoleRepository.GetAll().AsNoTrackingWithIdentityResolution() on role.Id equals ouRole.RoleId into tem1
                 from ouRole in tem1.DefaultIfEmpty()
                 join ou in ouRepository.GetAll().AsNoTrackingWithIdentityResolution() on ouRole.OrganizationUnitId equals ou.Id into tem2
                 from ou in tem2.DefaultIfEmpty()
                 select new { role, ou };



            var list = await q2.ToListAsync(CancellationTokenProvider.Token);

            var groups = list.GroupBy(c => c.role, c => c.ou);
            CurrentUnitOfWork.Items["ous"] = groups.ToDictionary(c => c.Key.Id, c => c.AsEnumerable());
            var roles = groups.Select(c => c.Key);
            //var roles = await _roleManager
            //    .Roles
            //    .WhereIf(
            //        !input.Permission.IsNullOrWhiteSpace(),
            //        r => r.Permissions.Any(rp => rp.Name == input.Permission && rp.IsGranted)
            //    )
            //    .ToListAsync();
            var dtos = new List<RoleDto>();
            foreach (var item in roles)
            {
                dtos.Add(MapToEntityDto(item));
            }
            return dtos;
            //  return ObjectMapper.Map<List<RoleDto>>(roles);
        }

        //public override Task<RoleDto> UpdateAsync(RoleDto input)
        //{
        //    return base.UpdateAsync(input);
        //}
        public override async Task<RoleDto> UpdateAsync(RoleEditDto input)
        {
            CheckUpdatePermission();
            var role = await _roleManager.GetRoleByIdAsync(input.Id);

            //if (roleManagementConfig.StaticRoles.Any(c => c.RoleName.Equals(role.Name, StringComparison.Ordinal)))
            //    role.IsStatic = true;

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
            //await CurrentUnitOfWork.SaveChangesAsync();

            #region 处理组织单位
            if (input.OuIds != null)
            {
                var ous = (await ouRoleRepository.GetAllListAsync(c => c.RoleId == role.Id)).Select(c => c.OrganizationUnitId);
                foreach (var ousId in ous)
                {
                    await _roleManager.RemoveFromOrganizationUnitAsync(role.Id, ousId);
                }
                await CurrentUnitOfWork.SaveChangesAsync();
                foreach (var item in input.OuIds)
                {
                    await _roleManager.AddToOrganizationUnitAsync(role.Id, item, base.AbpSession.TenantId);
                }
            }
            await CurrentUnitOfWork.SaveChangesAsync();
            #endregion

            await sdfsdf(role);
            var dto = MapToEntityDto(role);

            // var dto = ObjectMapper.Map<RoleDto>(role);
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
        [UnitOfWork(false)]
        public Task<ListResultDto<PermissionDto>> GetAllPermissions()
        {
            //若某个权限是用来被其它权限依赖的，那么在可选列表中不要显示
            var permissions = PermissionManager.GetAllPermissions().Where(c => !c.GetDependentedPermissions().Any());

            return Task.FromResult(new ListResultDto<PermissionDto>(
                ObjectMapper.Map<List<PermissionDto>>(permissions).OrderBy(p => p.DisplayName).ToList()
            ));
        }

        //protected override IQueryable<Role> CreateFilteredQuery(PagedRoleResultRequestDto input)
        //{
        //    return Repository.GetAll()//.GetAllIncluding(x => x.Permissions)
        //        .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Keyword)
        //        || x.DisplayName.Contains(input.Keyword)
        //        || x.Description.Contains(input.Keyword));
        //}

        //protected override async Task<Role> GetEntityByIdAsync(int id)
        //{
        //    var entity = await Repository.FirstOrDefaultAsync(x => x.Id == id);
        //    await sdfsdf(entity);
        //    return entity;
        //}

        //protected override IQueryable<Role> ApplySorting(IQueryable<Role> query, PagedRoleResultRequestDto input)
        //{
        //    return query.OrderBy(r => r.DisplayName);
        //}

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
                GrantedPermissionNames = grantedPermissions.Where(c => c.Children.Count == 0).Select(p => p.Name).ToList()
            };
        }

        [AbpAuthorize(PermissionNames.AdministratorSystemRoleDelete)]
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


        //[Obsolete("应该单独定义应用服务，而不是在后台管理角色的服务中提供此方法")]
        //public async Task<IReadOnlyList<RoleSelectDto>> GetForSelectAsync(GetForSelectInput a)
        //{
        //    var list = await _roleManager.Roles.OrderBy(c => c.DisplayName).Select(c => new RoleSelectDto
        //    {
        //        Value = c.Id.ToString(),
        //        DisplayText = c.DisplayName,
        //        Name = c.Name
        //    }).ToListAsync();
        //    if (a.ForType == 0)
        //        list.Insert(0, new RoleSelectDto(null, string.IsNullOrWhiteSpace(a.ParentText) ? L("Please select role") : a.ParentText));
        //    else if (a.ForType == 1)
        //        list.Insert(0, new RoleSelectDto(null, string.IsNullOrWhiteSpace(a.ParentText) ? L("Please select") : a.ParentText));
        //    return list;


        //}

    }
}

