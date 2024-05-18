


using Abp.IdentityFramework;
using Abp.Organizations;
using Abp.Zero.Configuration;
using BXJG.Utils.Application.Share.Dtos;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using ZLJ.Application.Authorization.Permissions;
using ZLJ.Application.Roles;
using ZLJ.Application.Roles.Dto;
using ZLJ.Application.Common.OU;
using ZLJ.Application.Common.Share.OU;
using ZLJ.Application.Share.Authorization.Permissions;
using ZLJ.Application.Share.Post;
using ZLJ.Application.Share.Roles;
using ZLJ.Core.Authorization.Roles;
using ZLJ.Core.Authorization.Users;
using ZLJ.Core.BaseInfo.Post;

namespace ZLJ.Application.Post
{
    [AbpAuthorize(PermissionNames.AdministratorBaseInfoPost)]
    public class PostAppService : AdminCrudBaseAppService<PostEntity, PostDto, int, PagedAndSortedResultRequest<PagedPostResultRequestDto>, CreatePostDto, PostEditDto>//, IPostAppService
    {
        private readonly RoleManager _roleManager;
        private readonly UserManager _userManager;
        readonly IRoleManagementConfig roleManagementConfig;
        IRepository<OrganizationUnitRole, long> ouRoleRepository;
        IRepository<OrganizationUnit, long> ouRepository;
        OrganizationUnitManager unitManager;

        public PostAppService(IRepository<PostEntity> repository,
                              RoleManager roleManager,
                              UserManager userManager,
                              IRoleManagementConfig roleManagementConfig,
                              IRepository<OrganizationUnitRole, long> ouRoleRepository,
                              IRepository<OrganizationUnit, long> ouRepository,
                              OrganizationUnitManager unitManager)
            : base(repository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            this.roleManagementConfig = roleManagementConfig;

            LocalizationSourceName = ZLJ.Core.Share.ZLJConsts.LocalizationSourceName;

            base.CreatePermissionName = PermissionNames.AdministratorBaseInfoPostCreate;
            base.UpdatePermissionName = PermissionNames.AdministratorBaseInfoPostUpdate;
            base.DeletePermissionName = PermissionNames.AdministratorBaseInfoPostDelete;
            base.GetPermissionName = PermissionNames.AdministratorBaseInfoPost;
            base.GetAllPermissionName = PermissionNames.AdministratorBaseInfoPost;
            this.ouRoleRepository = ouRoleRepository;
            this.ouRepository = ouRepository;
            this.unitManager = unitManager;
        }
        [UnitOfWork(false)]
        public override async Task<PostDto> GetAsync(EntityDto<int> input)
        {
            var role = await GetEntityByIdAsync(input.Id) as PostEntity;
            await sdfsdf(role);
            var dto = MapToEntityDto(role);
            dto.GrantedPermissions = (await _roleManager.GetGrantedPermissionsAsync(role)).Where(c => c.Children.Count == 0).Select(c => c.Name).ToList();
            return dto;
        }

        protected override PostDto MapToEntityDto(PostEntity role)
        {
            var dto = base.MapToEntityDto(role);
            if (CurrentUnitOfWork.Items.TryGetValue("ous", out var ousTemp))
            {
                var ous = ousTemp as IDictionary<int, IEnumerable<OrganizationUnit>>;
                dto.Ous = ObjectMapper.Map<List<OuDto>>(ous[role.Id].Where(c => c != default));
            }

            //    var ous = CurrentUnitOfWork.Items["ous"] as IDictionary<int, IEnumerable<OrganizationUnit>>;
            //   if (ous != default)


            return dto;
        }

        protected virtual async Task sdfsdf(PostEntity entity)
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

        //[UnitOfWork]
        public override async Task<PostDto> CreateAsync(CreatePostDto input)
        {
            CheckCreatePermission();

            //采购部经理 销售部经理，都是经理，但它们是不同的岗位
            //而角色是可以多对多的

            if (input.OuIds != default && input.OuIds.Length > 1)
                throw new ApplicationException("岗位只能与一个公司或部门关联");

            //input.nam
            var role = ObjectMapper.Map<PostEntity>(input);
            role.Name = TinyPinyin.PinyinHelper.GetPinyin(input.DisplayName, "")+ BXJG.Common.RandomHelper.GetRandomString(6);//多音字咋搞？
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

            if (input.GrantedPermissions != default)
            {
                var grantedPermissions = PermissionManager
                .GetAllPermissions()
                .Where(p => input.GrantedPermissions.Contains(p.Name))
                .ToList();

                await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);
            }

            await CurrentUnitOfWork.SaveChangesAsync();
            await sdfsdf(role);
            var dto = MapToEntityDto(role);
            // var dto = ObjectMapper.Map<RoleDto>(role);
            dto.GrantedPermissions = (await _roleManager.GetGrantedPermissionsAsync(role)).Select(c => c.Name).ToList();
            return dto;
        }


        [UnitOfWork(false)]
        public override async Task<PagedResultDto<PostDto>> GetAllAsync(PagedAndSortedResultRequest<PagedPostResultRequestDto> input)
        {
            var q2 = from role in Repository.GetAll().AsNoTrackingWithIdentityResolution().OfType<PostEntity>()
                     join ouRole in ouRoleRepository.GetAll().AsNoTrackingWithIdentityResolution() on role.Id equals ouRole.RoleId into tem1
                     from ouRole in tem1.DefaultIfEmpty()
                     join ou in ouRepository.GetAll().AsNoTrackingWithIdentityResolution() on ouRole.OrganizationUnitId equals ou.Id into tem2
                     from ou in tem2.DefaultIfEmpty()
                     select new { role, ou };

            q2 = q2.WhereIf(input.Filter.OuCode.IsNotNullOrWhiteSpaceBXJG(), c => c.ou.Code.StartsWith(input.Filter.OuCode))
                   .WhereIf(input.Filter.IsStatic.HasValue,c=>c.role.IsStatic==input.Filter.IsStatic.Value)
                   .WhereIf(input.Filter.Keywords.IsNotNullOrWhiteSpaceBXJG(), c => c.role.Name.Contains(input.Filter.Keywords)|| c.role.DisplayName.Contains(input.Filter.Keywords));

            q2 = from role in q2.Select(c => c.role).Distinct()
                 join ouRole in ouRoleRepository.GetAll().AsNoTrackingWithIdentityResolution() on role.Id equals ouRole.RoleId into tem1
                 from ouRole in tem1.DefaultIfEmpty()
                 join ou in ouRepository.GetAll().AsNoTrackingWithIdentityResolution() on ouRole.OrganizationUnitId equals ou.Id into tem2
                 from ou in tem2.DefaultIfEmpty()
                 select new { role, ou };
            //input.Sorting.Replace("DisplayName");
            var ct = await q2.CountAsync(CancellationTokenProvider.Token);

            

            q2 = q2.OrderBy(input.Sorting).PageBy(input);

          //q2 = q2.OrderBy(c => c.role.DisplayName).PageBy(input);

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
            var dtos = new List<PostDto>();
            foreach (var item in roles)
            {
                dtos.Add(MapToEntityDto(item));
            }
            return new PagedResultDto<PostDto>(ct, dtos);
        }

        //[UnitOfWork(false)]
        //public async Task<IList<PostDto>> GetPostsAsync(GetPostsInput input)
        //{
        //    var q2 = from role in Repository.GetAll().AsNoTrackingWithIdentityResolution().OfType<PostEntity>()
        //             join ouRole in ouRoleRepository.GetAll().AsNoTrackingWithIdentityResolution() on role.Id equals ouRole.RoleId into tem1
        //             from ouRole in tem1.DefaultIfEmpty()
        //             join ou in ouRepository.GetAll().AsNoTrackingWithIdentityResolution() on ouRole.OrganizationUnitId equals ou.Id into tem2
        //             from ou in tem2.DefaultIfEmpty()
        //             select new { role, ou };

        //    q2 = q2.WhereIf(!input.OuCode.IsNullOrWhiteSpace(), c => c.ou.Code.StartsWith(input.OuCode));

        //    q2 = q2.OrderBy(c => c.role.DisplayName);

        //    q2 = from role in q2.Select(c => c.role).Distinct()
        //         join ouRole in ouRoleRepository.GetAll().AsNoTrackingWithIdentityResolution() on role.Id equals ouRole.RoleId into tem1
        //         from ouRole in tem1.DefaultIfEmpty()
        //         join ou in ouRepository.GetAll().AsNoTrackingWithIdentityResolution() on ouRole.OrganizationUnitId equals ou.Id into tem2
        //         from ou in tem2.DefaultIfEmpty()
        //         select new { role, ou };



        //    var list = await q2.ToListAsync();

        //    var groups = list.GroupBy(c => c.role, c => c.ou);
        //    CurrentUnitOfWork.Items["ous"] = groups.ToDictionary(c => c.Key.Id, c => c.AsEnumerable());
        //    var roles = groups.Select(c => c.Key);
        //    //var roles = await _roleManager
        //    //    .Roles
        //    //    .WhereIf(
        //    //        !input.Permission.IsNullOrWhiteSpace(),
        //    //        r => r.Permissions.Any(rp => rp.Name == input.Permission && rp.IsGranted)
        //    //    )
        //    //    .ToListAsync();
        //    var dtos = new List<PostDto>();
        //    foreach (var item in roles)
        //    {
        //        dtos.Add(MapToEntityDto(item));
        //    }
        //    return dtos;
        //    //  return ObjectMapper.Map<List<RoleDto>>(roles);
        //}




        //public override Task<RoleDto> UpdateAsync(RoleDto input)
        //{
        //    return base.UpdateAsync(input);
        //}
        public override async Task<PostDto> UpdateAsync(PostEditDto input)
        {
            CheckUpdatePermission();
            var role = await _roleManager.GetRoleByIdAsync(input.Id) as PostEntity;

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

                //暂时禁用，因为目前功能没做完，实际上这里是应该要放开的
                //await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);
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
            #endregion
            await CurrentUnitOfWork.SaveChangesAsync();

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
            var permissions = PermissionManager.GetAllPermissions();

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


        //[AbpAuthorize(PermissionNames.AdministratorBaseInfoPostDelete)]
        //public async Task<IEnumerable<int>> DeleteBatchAsync(params int[] input)
        //{
        //    var list = new List<int>();
        //    foreach (var item in input)
        //    {
        //        var role = await _roleManager.GetRoleByIdAsync(item);
        //        if (role.IsStatic)
        //        {
        //            continue;
        //            //throw new UserFriendlyException("CannotDeleteAStaticRole");
        //        }
        //        var rt = await _roleManager.DeleteAsync(role);
        //        if (rt.Succeeded)
        //            list.Add(role.Id);
        //    }
        //    return list;
        //}
        [UnitOfWork(false)]
        [AbpAuthorize(PermissionNames.AdministratorBaseInfoPostUpdate)]
        public async Task<GetPostForEditOutput> GetPostForEdit(EntityDto input)
        {
            var permissions = PermissionManager.GetAllPermissions();
            var role = await _roleManager.GetRoleByIdAsync(input.Id);
            var grantedPermissions = (await _roleManager.GetGrantedPermissionsAsync(role)).ToArray();
            var roleEditDto = ObjectMapper.Map<PostEditDto>(role);

            return new GetPostForEditOutput
            {
                Role = roleEditDto,
                Permissions = ObjectMapper.Map<List<FlatPermissionDto>>(permissions).OrderBy(p => p.DisplayName).ToList(),
                GrantedPermissionNames = grantedPermissions.Where(c => c.Children.Count == 0).Select(p => p.Name).ToList()
            };
        }
    }
}

