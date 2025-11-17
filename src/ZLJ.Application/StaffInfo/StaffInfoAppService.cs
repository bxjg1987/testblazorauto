using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Notifications;
using Abp.Organizations;
using BXJG.Common.Contracts;
using BXJG.Utils.Application.Share.OU;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using System.Collections.ObjectModel;
using ZLJ.Application.Authorization.Permissions;
using ZLJ.Application.Common.Share.OU;
using ZLJ.Application.Common.Share.Post;
using ZLJ.Application.Common.Share.User;
using ZLJ.Application.Common.StaffInfo;
using ZLJ.Application.Share.Authorization.Permissions;
using ZLJ.Application.Share.StaffInfo;
using ZLJ.Core.Authorization.Roles;
using ZLJ.Core.Authorization.Users;
using ZLJ.Core.BaseInfo.Post;
using ZLJ.Core.OU;



namespace ZLJ.Application.StaffInfo
{
    /*
     * 由于abp顶层设计有约束 User:AbpUser<User>，
     * 我们的用户子类无法实现，所以继承父类应用服务时，使用抽象类，硬编码做转换
     */

    /// <summary>
    /// 后台管理-员工档案
    /// </summary>
    public class StaffInfoAppService : AdminCrudBaseAppService<User,
                                                               StaffInfoDto,
                                                               long,
                                                               PagedAndSortedResultRequest<GetStaffInfoListCondition>,
                                                               StaffInfoCreateDto,
                                                               StaffInfoEditDto>
    {
        private readonly IRepository<PostEntity> postRepository;
        private readonly IRepository<OrganizationUnitEntity, long> organizationUnitRepository;
        private readonly IRepository<OrganizationUnitRole, long> organizationUnitRoleRepository;
        private readonly IRepository<UserOrganizationUnit, long> ouUserRepository;
        private readonly IRepository<UserRole, long> userRoleRepository;

        public RoleManager RoleManager { get; set; }
        public IPasswordHasher<User> PasswordHasher { get; set; }
        public UserManager UserManager { get; set; }
        public IEnumerable<IPasswordValidator<User>> _passwordValidators { get; set; }
        public INotificationSubscriptionManager _notificationSubscriptionManager { get; set; }
        public IRepository<Role, int> RoleRepository { get; set; }

        //private readonly BXJG.Utils.Application.User.UserAppService<User,
        //                                                            Role,
        //                                                            UserManager,
        //                                                            RoleManager,
        //                                                            StaffInfoDto,
        //                                                            GetStaffInfoListCondition,
        //                                                            StaffInfoCreateDto,
        //                                                            StaffInfoEditDto> userAppService;
        public StaffInfoAppService(IRepository<User, long> repository,
                                   IRepository<PostEntity> roleRepository,
                                   IRepository<OrganizationUnitEntity, long> organizationUnitRepository,
                                   IRepository<OrganizationUnitRole, long> organizationUnitRoleRepository,
                                   IRepository<UserOrganizationUnit, long> ouUserRepository,
                                   IRepository<UserRole, long> userRoleRepository/*,
                                   BXJG.Utils.Application.User.UserAppService<User, Role, UserManager, RoleManager, StaffInfoDto, GetStaffInfoListCondition, StaffInfoCreateDto, StaffInfoEditDto> userAppService*/) : base(repository)
        {
            LocalizationSourceName = ZLJConsts.LocalizationSourceName;
            //this.repository = repository;

            CreatePermissionName = PermissionNames.BXJGBaseInfoStaffInfoCreate;
            UpdatePermissionName = PermissionNames.BXJGBaseInfoStaffInfoUpdate;
            DeletePermissionName = PermissionNames.BXJGBaseInfoStaffInfoDelete;
            GetPermissionName = PermissionNames.BXJGBaseInfoStaffInfoGet;
            // _workloadRecordRepository = workloadRecordRepository;
            //this.userManager = userManager;
            //this.passwordHasher = passwordHasher;
            //this.roleManager = roleManager;
            postRepository = roleRepository;
            this.organizationUnitRepository = organizationUnitRepository;
            this.organizationUnitRoleRepository = organizationUnitRoleRepository;
            this.ouUserRepository = ouUserRepository;
            this.userRoleRepository = userRoleRepository;
            //this.userAppService = userAppService;
        }


        protected override User MapToEntity(StaffInfoCreateDto createInput)
        {
            var user = ObjectMapper.Map<StaffInfoEntity>(createInput); //base.MapToEntity(createInput);
            ObjectMapper.Map(createInput.BaseDto, user);
            return user;
        }
        protected override async Task CreateSave(User staff, StaffInfoCreateDto input)
        {
            var user = staff as StaffInfoEntity;
            user.TenantId = AbpSession.TenantId;

            await UserManager.InitializeOptionsAsync(AbpSession.TenantId);
            foreach (var validator in _passwordValidators)
            {
                CheckErrors(await validator.ValidateAsync(UserManager, user, input.Password));
            }

            user.Password = PasswordHasher.HashPassword(user, input.Password);

            user.Roles = new Collection<UserRole>();
            foreach (var roleName in input.RoleNames)
            {
                var role = await RoleManager.GetRoleByNameAsync(roleName);
                user.Roles.Add(new UserRole(AbpSession.TenantId, user.Id, role.Id));
            }

            user.Surname = user.Name;

            CheckErrors(await UserManager.CreateAsync(user));
            await CurrentUnitOfWork.SaveChangesAsync(); //To get new user's Id.


            //Organization Units
            await UserManager.SetOrganizationUnitsAsync(user, input.OrganizationUnits.ToArray());


            //Notifications
            await _notificationSubscriptionManager.SubscribeToAllAvailableNotificationsAsync(user.ToUserIdentifier());


            //  await base.CreateSave(user, input);
        }
        //[AbpAuthorize(PermissionNames.BXJGBaseInfoStaffInfoCreate)]
        //public override async Task<StaffInfoDto> CreateAsync(StaffInfoCreateDto input)
        //{



        //    userAppService.CreatePermissionName = PermissionNames.BXJGBaseInfoStaffInfoCreate;
        //    userAppService.MapToEntityCreateFunc = (input) =>
        //    {
        //        var entity = ObjectMapper.Map<StaffInfoEntity>(input);
        //        entity.Id = default;
        //        entity.IsEmailConfirmed = true;
        //        entity.IsPhoneNumberConfirmed = true;
        //        entity.IsLockoutEnabled = true;
        //        return ValueTask.FromResult( entity as User);
        //    };
        //    userAppService.MapToEntityFunc = MapToEntity;
        //    //return base.CreateAsync(input);r
        //    return await userAppService.CreateAsync(input);
        //}


        protected override async Task DeleteCore(User entity)
        {
            if (entity.Id == AbpSession.UserId)
                UserFriendlyExceptionFactory.Throw("不能删除自己！", LocalizationSource);

            CheckErrors(await UserManager.DeleteAsync(entity));
            // return base.DeleteCore(entity);
        }

        //protected override User MapToEntity(StaffInfoCreateDto createInput)
        //{
        //    //var entity =  base.MapToEntity(createInput);
        //    var entity = ObjectMapper.Map<StaffInfoEntity>(createInput);
        //    entity.Id = default;
        //    entity.IsEmailConfirmed = true;
        //    entity.IsPhoneNumberConfirmed = true;
        //    entity.IsLockoutEnabled = true;
        //    return entity;
        //}

        //protected override ValueTask MapToEntity(User entity)
        //{
        //    entity.Pinyin = TinyPinyin.PinyinHelper.GetPinyinInitials(entity.Name);
        //    return base.MapToEntity(entity);
        //}

        protected override async Task<StaffInfoDto> GetDtoById(User entity, long id = default)
        {
            var qt = (await GetRoleAndOusAsync(new[] { entity == null ? id : entity.Id })).First();
            CurrentUnitOfWork.Items["tmpPost"] = qt.Select(c => c.Post).Where(c => c != default);
            CurrentUnitOfWork.Items["tmpOu"] = qt.Select(c => c.Ou).Where(c => c != default);
            return await base.GetDtoById(entity, id);
        }

        private IQueryable<QueryTemp> GetFullQuery()
        {
            var q = from staff in Repository.GetAll().OfType<StaffInfoEntity>().AsNoTrackingWithIdentityResolution().Include(c => c.Area)
                    join ouUser in ouUserRepository.GetAll().AsNoTrackingWithIdentityResolution() on staff.Id equals ouUser.UserId into ouUsers
                    from ouUser in ouUsers.DefaultIfEmpty()
                    join userRole in userRoleRepository.GetAll().AsNoTrackingWithIdentityResolution() on staff.Id equals userRole.UserId into userRoles
                    from userRole in userRoles.DefaultIfEmpty()
                    join ouRole in organizationUnitRoleRepository.GetAll().AsNoTrackingWithIdentityResolution() on userRole.RoleId equals ouRole.RoleId into ouRoles
                    from ouRole in ouRoles.DefaultIfEmpty()
                    join post in postRepository.GetAll().AsNoTrackingWithIdentityResolution() on userRole.RoleId equals post.Id into posts
                    from post in posts.DefaultIfEmpty()
                        //outer apply
                    from ou in organizationUnitRepository.GetAll()
                                                         .AsNoTrackingWithIdentityResolution()
                                                         .Where(c => c.Id == ouUser.OrganizationUnitId || c.Id == ouRole.OrganizationUnitId)
                                                         .DefaultIfEmpty()
                    select new QueryTemp
                    {
                        Staff = staff,
                        Post = post,
                        Ou = ou
                    };
            return q;
        }
        private async Task<List<IGrouping<StaffInfoEntity, QueryTemp>>> GetRoleAndOusAsync(IList<long> ids)
        {
            //var postQuery = from userRole in userRoleRepository.GetAll()
            //                                                   .AsNoTrackingWithIdentityResolution()
            //                                                   .Where(c => ids.Contains(c.UserId))
            //                join post in postRepository.GetAll().AsNoTrackingWithIdentityResolution() on userRole.RoleId equals post.Id into roles
            //                from post in roles.DefaultIfEmpty()
            //                select post;
            //var posts = await postQuery.ToListAsync();
            //var postIds = posts.Select(c => c.Id);
            //var ouRoleIds = organizationUnitRoleRepository.GetAll()
            //                                                           .AsNoTrackingWithIdentityResolution()
            //                                                           .Where(c => postIds.Contains(c.RoleId))
            //                                                           .Select(c => c.OrganizationUnitId).ToListAsync();
            //ef目前好像还不支持Union，所以分次查询会出现4次查询

            var q = from staff in Repository.GetAll().OfType<StaffInfoEntity>().AsNoTrackingWithIdentityResolution().Include(c => c.Area)
                    join ouUser in ouUserRepository.GetAll().AsNoTrackingWithIdentityResolution() on staff.Id equals ouUser.UserId into ouUsers
                    from ouUser in ouUsers.DefaultIfEmpty()
                    join userRole in userRoleRepository.GetAll().AsNoTrackingWithIdentityResolution() on staff.Id equals userRole.UserId into userRoles
                    from userRole in userRoles.DefaultIfEmpty()
                    join ouRole in organizationUnitRoleRepository.GetAll().AsNoTrackingWithIdentityResolution() on userRole.RoleId equals ouRole.RoleId into ouRoles
                    from ouRole in ouRoles.DefaultIfEmpty()
                    join post in postRepository.GetAll().AsNoTrackingWithIdentityResolution() on userRole.RoleId equals post.Id into posts
                    from post in posts.DefaultIfEmpty()
                        //outer apply
                    from ou in organizationUnitRepository.GetAll()
                                                         .AsNoTrackingWithIdentityResolution()
                                                         .Where(c => c.Id == ouUser.OrganizationUnitId || c.Id == ouRole.OrganizationUnitId)
                                                         .DefaultIfEmpty()
                    select new QueryTemp
                    {
                        Ou = ou,
                        Post = post,
                        Staff = staff
                    };

            //var sql = q.ToQueryString();
            var list = await q.ToListAsync();
            var sdf = list.GroupBy(c => c.Staff);
            var df = new List<IGrouping<StaffInfoEntity, QueryTemp>>();
            foreach (var item in ids)
            {
                df.Add(sdf.Single(c => c.Key.Id == item));
            }
            return df;
        }

        [AbpAuthorize(PermissionNames.BXJGBaseInfoStaffInfoGet)]
        [UnitOfWork(false)]
        public override async Task<PagedResultDto<StaffInfoDto>> GetAllAsync(PagedAndSortedResultRequest<GetStaffInfoListCondition> input)
        {
            var r = new PagedResultDto<StaffInfoDto>();
            var q = GetFullQuery().WhereIf(!input.Filter.Keywords.IsNullOrWhiteSpace(), c => c.Staff.Name.Contains(input.Filter.Keywords) ||
                                                                                      c.Staff.Pinyin.Contains(input.Filter.Keywords) ||
                                                                                      c.Staff.PhoneNumber.Contains(input.Filter.Keywords) ||
                                                                                      c.Staff.CurrentAddress.Contains(input.Filter.Keywords))
                                  .WhereIf(!input.Filter.OuCode.IsNullOrWhiteSpace(), c => c.Ou.Code.StartsWith(input.Filter.OuCode))
                                  .WhereIf(input.Filter.PostId.HasValue, c => c.Post.Id == input.Filter.PostId.Value)
                                  .WhereIf(!input.Filter.AreaCode.IsNullOrWhiteSpace(), c => c.Staff.Area.Code.StartsWith(input.Filter.OuCode));
            r.TotalCount = await q.GroupBy(c => c.Staff.Id).CountAsync();
            q = q.OrderBy("Staff." + input.Sorting);

            var q2 = q.GroupBy(c => c.Staff.Id).Select(c => c.Key);
            // q = q.OrderBy( input.Sorting);

            r.TotalCount = await q2.CountAsync();

            q2 = q2.PageBy(input);
            var list = await q2.ToListAsync();
            var roleAndOus = await GetRoleAndOusAsync(list);
            var dtos = new List<StaffInfoDto>();

            foreach (var item in roleAndOus)
            {
                var qt = roleAndOus.First();
                CurrentUnitOfWork.Items["tmpPost"] = qt.Select(c => c.Post).Where(c => c != default);
                CurrentUnitOfWork.Items["tmpOu"] = qt.Select(c => c.Ou).Where(c => c != default);
                // var roleAndOu = roleAndOus[item.Key];
                dtos.Add(MapToEntityDto(item.Key));
            }
            r.Items = dtos;
            return r;
        }
        protected override StaffInfoDto MapToEntityDto(User user)
        {
            StaffInfoEntity entity = user as StaffInfoEntity;
            IEnumerable<PostEntity> posts = CurrentUnitOfWork.Items["tmpPost"] as IEnumerable<PostEntity>;
            IEnumerable<OrganizationUnitEntity> ous = CurrentUnitOfWork.Items["tmpOu"] as IEnumerable<OrganizationUnitEntity>;


            var dto = ObjectMapper.Map<StaffInfoDto>(entity);
            dto.Posts = ObjectMapper.Map<List<Common.Share.Post.PostForSelectDto>>(posts.Where(c => c != default).DistinctBy(c => c.Id));
            dto.RoleNames = dto.Posts.Where(c => c != default).DistinctBy(c => c.Id).Select(c => c.Name).ToArray();
            dto.Ous = ObjectMapper.Map<List<OUSelectDto>>(ous.Where(c => c != default).DistinctBy(c => c.Id));
            return dto;
        }

        //public async Task<StaffInfoDto> GetEdit()
        //{
        //    var roleQ = await RoleRepository.GetAllReadonlyAsync();
        //    var roles = await roleQ./*Where(x => x.IsDefault).*/ToListAsync(CancellationTokenProvider.Token);
        //    var rs = roles.Select(x => new PostForSelectDto { });
        //    return new StaffInfoDto
        //    {
        //        RoleNames = roles.Select(c => c.Name).ToArray(),
        //        BaseDto = new UserDto
        //        {
        //            IsActive = true,
        //            Roles = rs,
        //            //    RoleNames
        //        }
        //    };
        //}

    }
}