using ZLJ.Core.Authorization.Users;
using ZLJ.Core.Authorization.Roles;
using Microsoft.AspNetCore.Identity;
using Abp.Organizations;
using Abp.Authorization.Users;
using ZLJ.Application.Common.StaffInfo;
using ZLJ.Core.BaseInfo.Post;
using ZLJ.Core.BaseInfo;
using ZLJ.Application.Admin.Authorization.Permissions;
using ZLJ.Application.Share.Authorization.Permissions;
using ZLJ.Application.Common.Share.OU;

namespace ZLJ.Application.Admin.BaseInfo.StaffInfo
{
    /*
     * 目前是笛卡尔查询，性能低，考虑分次查询
     */

    /// <summary>
    /// 后台管理-员工档案
    /// </summary>
    public class BXJGBaseInfoStaffInfoAppService : AdminBaseAppService
    {
        private readonly UserManager userManager;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly RoleManager roleManager;
        private readonly IRepository<PostEntity> postRepository;
        private readonly Abp.Notifications.INotificationPublisher notificationPublisher;
        private readonly IRepository<OrganizationUnitEntity, long> organizationUnitRepository;
        private readonly IRepository<OrganizationUnitRole, long> organizationUnitRoleRepository;
        private readonly IRepository<UserOrganizationUnit, long> ouUserRepository;
        private readonly IRepository<UserRole, long> userRoleRepository;
        private readonly IRepository<StaffInfoEntity, long> repository;
        //private readonly IRepository<WorkloadRecordEntity, Guid> _workloadRecordRepository;
        public BXJGBaseInfoStaffInfoAppService(IRepository<StaffInfoEntity, long> repository,
                                               //IRepository<WorkloadRecordEntity, Guid> workloadRecordRepository,
                                               UserManager userManager,
                                               IPasswordHasher<User> passwordHasher,
                                               RoleManager roleManager,
                                               IRepository<PostEntity> roleRepository,
                                               Abp.Notifications.INotificationPublisher notificationPublisher,
                                               IRepository<OrganizationUnitEntity, long> organizationUnitRepository,
                                               IRepository<OrganizationUnitRole, long> organizationUnitRoleRepository,
                                               IRepository<UserOrganizationUnit, long> ouUserRepository,
                                               IRepository<UserRole, long> userRoleRepository)
        {
            LocalizationSourceName = ZLJ.Core.Share.ZLJConsts.LocalizationSourceName;
            this.repository = repository;

            //CreatePermissionName = PermissionNames.BXJGBaseInfoStaffInfoCreate;
            //UpdatePermissionName = PermissionNames.BXJGBaseInfoStaffInfoUpdate;
            //DeletePermissionName = PermissionNames.BXJGBaseInfoStaffInfoDelete;
            //GetPermissionName = PermissionNames.BXJGBaseInfoStaffInfo;
            // _workloadRecordRepository = workloadRecordRepository;
            this.userManager = userManager;
            this.passwordHasher = passwordHasher;
            this.roleManager = roleManager;
            this.postRepository = roleRepository;
            this.notificationPublisher = notificationPublisher;
            this.organizationUnitRepository = organizationUnitRepository;
            this.organizationUnitRoleRepository = organizationUnitRoleRepository;
            this.ouUserRepository = ouUserRepository;
            this.userRoleRepository = userRoleRepository;
        }

        [AbpAuthorize(PermissionNames.BXJGBaseInfoStaffInfoCreate)]
        public async Task<StaffInfoDto> CreateAsync(StaffInfoCreateDto input)
        {
            var entity = ObjectMapper.Map<StaffInfoEntity>(input);

            entity.Surname = entity.Name;
            //user.TenantId = AbpSession.TenantId;
            entity.Id = default;
            entity.IsEmailConfirmed = true;
            entity.IsPhoneNumberConfirmed = true;
            entity.IsLockoutEnabled = true;
            entity.Pinyin = TinyPinyin.PinyinHelper.GetPinyinInitials(entity.Name);
            //生日是计算来的
            //if (input.Birthday.HasValue)
            //{
            //    var r = input.Birthday.Value.DateTime.CalculateAge(Clock.Now);
            //    entity.AgeDays = r.days;
            //    entity.AgeMonths = r.months;
            //    entity.AgeYears = r.years;
            //    entity.AgeString = $"{r.years}";
            //}
            //if (user.EmailAddress.IsNullOrWhiteSpace())
            //    user.EmailAddress = input.PhoneNumber + "@a.com";
            await userManager.InitializeOptionsAsync(AbpSession.TenantId);
            CheckErrors(await userManager.CreateAsync(entity, input.Password));
            await Hnad(entity, input.RoleNames, input.ouIds);
            //在updating事件中，这个无论是同步还是异步，无论是加载直接导航还是间接导航，都会阻塞，不确定是死锁还是会超时
            //这里是否会出问题还需要测试
            await repository.EnsurePropertyLoadedAsync(entity, c => c.Area);
            var qt = (await GetRoleAndOusAsync(new[] { entity.Id })).First();
            return MapToEntityDto(entity, qt.Select(c=>c.Post).Where(c => c != default), qt.Select(c=>c.Ou).Where(c => c != default));
        }
        private IQueryable<QueryTemp> GetFullQuery()
        {
            var q = from staff in repository.GetAll().AsNoTrackingWithIdentityResolution().Include(c => c.Area)
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

            var q = from staff in repository.GetAll().AsNoTrackingWithIdentityResolution().Include(c => c.Area)
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
        [AbpAuthorize(PermissionNames.BXJGBaseInfoStaffInfoUpdate)]
        public async Task<StaffInfoDto> UpdateAsync(StaffInfoEditDto input)
        {
            //var entity = await repository.GetAsync(input.Id);
            var entity = await userManager.GetUserByIdAsync(input.Id) as StaffInfoEntity;
            entity.Surname = entity.Name;
            //ObjectMapper.Map(input, entity);
            // base.MapToEntity(input, entity);
            // await Repository.UpdateAsync(entity);
            await userManager.InitializeOptionsAsync(AbpSession.TenantId);
            #region 更新维修人员当前月的工作量积分
            //if (input.CurrentMonthRecordRulePoints != null && input.CurrentMonthRecordRulePoints != 0)
            //{
            //    DateTime statisticsTime = Clock.Now.AddDays(1 - DateTime.Now.Day).Date;
            //    var workload = await _workloadRecordRepository.FirstOrDefaultAsync(p => p.StaffInfoId == input.Id && p.StatisticsTime == statisticsTime);
            //    workload.RulePoints = input.CurrentMonthRecordRulePoints.Value;
            //    await _workloadRecordRepository.UpdateAsync(workload);
            //}
            #endregion

            #region 处理关联的用户

            // var user = await userManager.GetUserByIdAsync(input.UserId.Value);


            //if (input.EmailAddress.IsNullOrWhiteSpace())
            //    input.EmailAddress = input.PhoneNumber + "@a.com";

            //user.Surname = user.Name;

            var oldPwd = entity.Password;
           // var newPwd = input.Password;

            base.ObjectMapper.Map(input, entity);

            //生日是会变的，该动态计算
            //if (input.Birthday.HasValue)
            //{
            //    var r = input.Birthday.Value.DateTime.CalculateAge(Clock.Now);
            //    entity.AgeDays = r.days;
            //    entity.AgeMonths = r.months;
            //    entity.AgeYears = r.years;
            //    entity.AgeString = $"{r.years}";
            //}
            entity.Pinyin = TinyPinyin.PinyinHelper.GetPinyinInitials(entity.Name);

            CheckErrors(await userManager.UpdateAsync(entity));//按理说这里不会去动密码

            if (!input.Password.IsNullOrWhiteSpace())
            {
                CheckErrors(await userManager.ChangePasswordAsync(entity, input.Password)); //这里会应用密码策略 会操作数据库
                //input.Password = passwordHasher.HashPassword(entity, input.Password);
            }
            else
                entity.Password = oldPwd;


            await Hnad(entity, input.RoleNames, input.ouIds);
            #endregion

            await CurrentUnitOfWork.SaveChangesAsync();
            //在updating事件中，这个无论是同步还是异步，无论是加载直接导航还是间接导航，都会阻塞，不确定是死锁还是会超时
            //这里是否会出问题还需要测试
            await repository.EnsurePropertyLoadedAsync(entity, c => c.Area);
            var qt = (await GetRoleAndOusAsync(new[] { entity.Id })).First();
            return MapToEntityDto(entity, qt.Select(c=>c.Post).Where(c => c != default), qt.Select(c=>c.Ou).Where(c => c != default));
        }
        private async Task Hnad(StaffInfoEntity staff, IEnumerable<string> postNames, IEnumerable<long> ouIds)
        {
            if (postNames == default && ouIds == default)
                return;

            if (postNames != null)
                await userManager.SetRolesAsync(staff, postNames.ToArray());

            if (postNames == default && ouIds != default)
            {
                await userManager.SetOrganizationUnitsAsync(staff.Id, ouIds.ToArray());
            }
            else if (ouIds != default && postNames != default)
            {
                var sdf = from role in postRepository.GetAll().AsNoTrackingWithIdentityResolution().Where(c => postNames.Contains(c.Name))
                          join ouRole in organizationUnitRoleRepository.GetAll().AsNoTrackingWithIdentityResolution() on role.Id equals ouRole.RoleId into ouRoles
                          from ouRole in ouRoles.DefaultIfEmpty()
                          select ouRole.OrganizationUnitId;
                var glid = await sdf.ToListAsync();
                var dcld = ouIds.Except(glid);
                await userManager.SetOrganizationUnitsAsync(staff.Id, dcld.ToArray());
            }
        }
        //public async Task DeleteAsync(EntityDto<long> input)
        //{
        //    base.CheckDeletePermission();

        //    var userId = await Repository.GetAll().Where(c => c.Id == input.Id).Select(c => c.Id).SingleOrDefaultAsync();
        //    if (userId != default)
        //    {
        //        var user = await userManager.GetUserByIdAsync(userId);
        //        await userManager.DeleteAsync(user);
        //    }

        //    await base.DeleteAsync(input);
        //}
        /// <summary>
        /// 批量删除
        /// </summary>
        [AbpAuthorize(PermissionNames.BXJGBaseInfoStaffInfoDelete)]
        public async Task<BatchOperationOutputLong> DeleteBatchAsync(BatchOperationInputLong input)
        {
            var result = new BatchOperationOutputLong();
            foreach (var item in input.Ids)
            {
                try
                {
                    using var sw = base.UnitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.RequiresNew);
                    var user = await userManager.GetUserByIdAsync(item);
                    await userManager.DeleteAsync(user);
                    await sw.CompleteAsync();
                    result.Ids.Add(item);
                }
                catch (UserFriendlyException ex)
                {
                    //Logger.Warn($"删除员工档案失败，员工Id：{item}", ex);
                    result.ErrorMessage.Add(new BatchOperationErrorMessage(item, ex.Message));
                }
                catch (Exception ex)
                {
                    Logger.Warn($"删除员工档案失败，员工Id：{item}", ex);
                    result.ErrorMessage.Add(item.Message500());
                }
            }

            return result;
        }
        [AbpAuthorize(PermissionNames.BXJGBaseInfoStaffInfo)]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<StaffInfoDto>> GetAllAsync(GetStaffInfoListInput input)
        {
            var r = new PagedResultDto<StaffInfoDto>();
            var q = GetFullQuery().WhereIf(!input.Keywords.IsNullOrWhiteSpace(), c => c.Staff.Name.Contains(input.Keywords) ||
                                                                                      c.Staff.Pinyin.Contains(input.Keywords) ||
                                                                                      c.Staff.PhoneNumber.Contains(input.Keywords) ||
                                                                                      c.Staff.CurrentAddress.Contains(input.Keywords))
                                  .WhereIf(!input.OuCode.IsNullOrWhiteSpace(), c => c.Ou.Code.StartsWith(input.OuCode))
                                  .WhereIf(input.PostId.HasValue, c => c.Post.Id == input.PostId.Value)
                                  .WhereIf(!input.AreaCode.IsNullOrWhiteSpace(), c => c.Staff.Area.Code.StartsWith(input.OuCode));
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
               // var roleAndOu = roleAndOus[item.Key];
                dtos.Add(MapToEntityDto(item.Key, item.Select(c=>c.Post).Where(c=>c!=default), item.Select(c => c.Ou).Where(c => c != default)));
            }
            r.Items = dtos;
            return r;
        }
        [AbpAuthorize(PermissionNames.BXJGBaseInfoStaffInfo)]
        [UnitOfWork(false)]
        public async Task<StaffInfoDto> GetAsync(EntityDto<long> input)
        {
            var entity = await repository.GetAllIncluding(c => c.Area).SingleAsync(c => c.Id == input.Id);
            var roleAndOu = (await GetRoleAndOusAsync(new[] { input.Id })).Single();
            return MapToEntityDto(entity, roleAndOu.Select(c=>c.Post).Where(c => c != default), roleAndOu.Select(c=>c.Ou).Where(c => c != default));
        }
        private StaffInfoDto MapToEntityDto(StaffInfoEntity entity, IEnumerable<PostEntity> posts, IEnumerable<OrganizationUnitEntity> ous)
        {
            var dto = ObjectMapper.Map<StaffInfoDto>(entity);
            dto.Posts = ObjectMapper.Map<List<Application.Common.Post.PostDto>>(posts.Where(c => c != default).DistinctBy(c => c.Id));
            dto.RoleNames = dto.Posts.Where(c=>c!=default).DistinctBy(c=>c.Id).Select(c => c.Name).ToArray();
            dto.Ous = ObjectMapper.Map<List<OuDto>>(ous.Where(c => c != default).DistinctBy(c => c.Id));
            return dto;
        }
    }
}