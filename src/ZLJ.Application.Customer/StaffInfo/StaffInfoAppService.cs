
using ZLJ.Authorization.Users;
using ZLJ.Authorization.Roles;
using Abp.Events.Bus;
using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Identity;
using Abp.Organizations;
using Abp.Authorization.Users;
using Abp.UI;
using System.Linq.Dynamic.Core;
using ZLJ.App.Common.StaffInfo;
using ZLJ.BaseInfo.Post;
using ZLJ.BaseInfo;
using ZLJ.App.Customer;
using ZLJ.Customer;
using System.Linq;
using OpenXmlPowerTools;
using DocumentFormat.OpenXml.Spreadsheet;
//using DotNetCore.CAP;
using Abp.BackgroundJobs;
using Microsoft.Extensions.Logging;
using Abp.Threading.BackgroundWorkers;
using System.Threading;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Abp.Domain.Repositories;

namespace ZLJ.App.Customer.StaffInfo
{
    //public class TestJob : AsyncBackgroundJob<int>, ITransientDependency
    //{
    //    public override async Task ExecuteAsync(int number)
    //    {
    //        Logger.Debug("===============表示不在一个事务中============>"+number.ToString());
    //    }
    //}


    /*
     * 任何订阅都必须注册到ioc，因为cap回来调用
     * 任何订阅都必须实现 ICapSubscribe，cap才能识别这些是订阅者
     * 必须是虚方法，经过测试的，（而abp不晓得咋实现的，非虚方法好些也许）
     * 所有订阅者的 有[CapSubscribe]的方法默认是开启事务的
     */

    public class sdfdf : ITransientDependency//, ICapSubscribe
    {
        private readonly IRepository<CustomerOUEntity, long> ouRepository;

        public sdfdf(IRepository<CustomerOUEntity, long> ouRepository)
        {
            this.ouRepository = ouRepository;
        }

        ////[UnitOfWork]
        //[CapSubscribe("test.show.time")]
        //public virtual async Task TestCap(DateTime t)
        //{
        //    //var sdfdf = base.CurrentUnitOfWork;
        //    // var dd = this;
        //    //var sdddd = base.UnitOfWorkManager.Begin();
        //    await ouRepository.CountAsync();
        //    //return Task.CompletedTask;
        //    //  return Task.CompletedTask;
        //}
    }
    /// <summary>
    /// 客户管理自己的员工的接口
    /// </summary>
    public class StaffInfoAppService : CustomerAppServiceBase//, ICapSubscribe
    {
        private readonly UserManager userManager;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly IRepository<CustomerOUEntity, long> ouRepository;
        private readonly IRepository<UserOrganizationUnit, long> ouUserRepository;
        private readonly IRepository<CustomerStaffInfoEntity, long> repository;
        //private readonly ICapPublisher cap;
        private readonly IBackgroundJobManager backgroundJobManager;

        public StaffInfoAppService(IRepository<CustomerStaffInfoEntity, long> repository,
                                   UserManager userManager,
                                   IPasswordHasher<User> passwordHasher,
                                   IRepository<CustomerOUEntity, long> organizationUnitRepository,
                                   //ICapPublisher cap,
                                   IRepository<UserOrganizationUnit, long> ouUserRepository,
                                   IBackgroundJobManager backgroundJobManager)
        {
            this.repository = repository;
            this.userManager = userManager;
            this.passwordHasher = passwordHasher;
            this.ouRepository = organizationUnitRepository;
            this.ouUserRepository = ouUserRepository;
            //this.cap = cap;
            this.backgroundJobManager = backgroundJobManager;
        }

        /// <summary>
        /// 添加客户员工
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<StaffInfoDto> CreateAsync(StaffInfoCreateDto input)
        {
            var entity = ObjectMapper.Map<CustomerStaffInfoEntity>(input);
            entity.Surname = entity.Name;
            entity.EquipmentPwd = input.Password;
            entity.Pinyin = TinyPinyin.PinyinHelper.GetPinyinInitials(entity.Name);
            entity.Id = default;
            entity.IsEmailConfirmed = true;
            entity.IsPhoneNumberConfirmed = true;
            entity.IsLockoutEnabled = true;
            entity.IsActive = true;
            entity.CustomerId = base.CustomerSession.CustomerId.Value;
            entity.EmailAddress = Guid.NewGuid().ToString("n") + "@a.com";
            await userManager.InitializeOptionsAsync(AbpSession.TenantId);
            CheckErrors(await userManager.CreateAsync(entity, Guid.NewGuid().ToString("n")));
            await userManager.SetOrganizationUnitsAsync(entity.Id, new[] { input.OuId.Value });
            var ou = ouRepository.Get(input.OuId.Value);
            CurrentUnitOfWork.Items["ous"] = new Dictionary<long, CustomerOUEntity> { { entity.Id, ou } };
            return MapToEntityDto(entity);
        }




        public virtual async Task<StaffInfoDto> UpdateAsync(StaffInfoEditDto input)
        {
            // UnitOfWorkManager.MountCapAbpEFCore(cap); //已通过拦截器实现取代此方法
            //await cap.PublishAsync("test.show.time", DateTime.Now);

            // await backgroundJobManager.EnqueueAsync<TestJob, int>(3);

            //  var entity = await repository.GetAsync(input.Id);
            var entity = await userManager.GetUserByIdAsync(input.Id) as CustomerStaffInfoEntity;

            var oldPwd = entity.Password;
            base.ObjectMapper.Map(input, entity);
            entity.Surname = entity.Name;
            entity.Pinyin = TinyPinyin.PinyinHelper.GetPinyinInitials(entity.Name);

            await userManager.InitializeOptionsAsync(AbpSession.TenantId);
            CheckErrors(await userManager.UpdateAsync(entity));
            if (!input.Password.IsNullOrWhiteSpace())
            {
                entity.EquipmentPwd = input.Password;
                CheckErrors(await userManager.ChangePasswordAsync(entity, input.Password));//这个会应用密码规则，还会操作数据库
                                                                                           //input.Password = passwordHasher.HashPassword(entity, input.Password);
            }
            else
                entity.Password = oldPwd;

            await userManager.SetOrganizationUnitsAsync(entity.Id, new[] { input.OuId });
            var ou = ouRepository.Get(input.OuId);
            CurrentUnitOfWork.Items["ous"] = new Dictionary<long, CustomerOUEntity> { { entity.Id, ou } };

            await CurrentUnitOfWork.SaveChangesAsync();

            // throw new Exception("测试backgroundJobManager和abp的业务是否在一个事务中");
            return MapToEntityDto(entity);

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
        /// 批量禁用客户员工
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<BatchOperationOutputLong> DeleteBatchAsync(BatchOperationInputLong input)
        {
            var result = new BatchOperationOutputLong();

            foreach (var item in input.Ids)
            {
                try
                {
                    using var sw = base.UnitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.RequiresNew);
                    var user = await userManager.GetUserByIdAsync(item);
                    if (await userManager.IsInRoleAsync(user, CustomerRole.CustomerAdminRole))
                        throw new UserFriendlyException($"{user.Name}是管理员");
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
                    Logger.Warn($"删除客户员工档案失败，Id：{item}", ex);
                    result.ErrorMessage.Add(item.Message500());
                }
            }

            return result;
        }

        /// <summary>
        /// 开关
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<BatchOperationOutputLong> SwitchActive(BatchSwitchInputLong input)
        {
            var result = new BatchOperationOutputLong();
            foreach (var id in input.Ids)
            {
                try
                {
                    var sw = UnitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.RequiresNew);
                    var item = await repository.SingleAsync(c => c.Id == id);
                    if (await userManager.IsInRoleAsync(item, CustomerRole.CustomerAdminRole))
                        throw new UserFriendlyException($"{item.Name}是管理员");
                    item.IsActive = input.IsActive;
                    await sw.CompleteAsync();
                    result.Ids.Add(item.Id);
                }
                catch (UserFriendlyException ex)
                {
                    result.ErrorMessage.Add(new BatchOperationErrorMessage(id, ex.Message));
                }
                catch (Exception ex)
                {
                    result.ErrorMessage.Add(BatchOperationErrorMessageExt.Message500(id));
                    Logger.Warn($"部分或全部开关失败，员工Id：{id}", ex);
                }
            }
            return result;
        }

        /// <summary>
        /// 根据条件获取客户员工的分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(false)]
        public virtual async Task<PagedResultDto<StaffInfoDto>> GetAllAsync(GetStaffInfoListInput input)
        {
            var r = new PagedResultDto<StaffInfoDto>();

            var q = from user in repository.GetAll().AsNoTrackingWithIdentityResolution()
                    join b in ouUserRepository.GetAll().AsNoTrackingWithIdentityResolution() on user.Id equals b.UserId into ouUsers
                    from b in ouUsers.DefaultIfEmpty()
                    join ou in ouRepository.GetAll().AsNoTrackingWithIdentityResolution() on b.OrganizationUnitId equals ou.Id into ous
                    from ou in ous.DefaultIfEmpty()
                    select new { user, ou };

            q = q.WhereIf(!input.Keywords.IsNullOrWhiteSpace(), x => x.user.Name.Contains(input.Keywords) ||
                                                                     x.user.PhoneNumber.Contains(input.Keywords) ||
                                                                     x.user.Pinyin.Contains(input.Keywords))
                 .WhereIf(!input.OuCode.IsNullOrWhiteSpace(), x => x.ou.Code.StartsWith(input.OuCode))
                 .WhereIf(input.IsActive.HasValue, x => x.user.IsActive == input.IsActive.Value);

            q = q.Where(c => c.user.CustomerId == CustomerSession.CustomerId);

            r.TotalCount = await q.GroupBy(c => c.user.Id).CountAsync();

            if (input.Sorting.IsNullOrEmpty())
                input.Sorting = "user.CreationTime desc";

            var q2 = q.OrderBy(input.Sorting).PageBy(input);
            var list = await q2.ToListAsync();

            CurrentUnitOfWork.Items["ous"] = list.ToDictionary(c => c.user.Id, c => c.ou);

            var dtos = new List<StaffInfoDto>();
            foreach (var item in list)
            {
                var dto = MapToEntityDto(item.user);
                dtos.Add(dto);
            }
            r.Items = dtos;
            return r;
        }
        /// <summary>
        /// 获取指定id的客户员工
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(false)]
        public virtual async Task<StaffInfoDto> GetAsync(EntityDto<long> input)
        {
            var entity = await repository.GetAllIncluding(c => c.OrganizationUnits).AsNoTrackingWithIdentityResolution().SingleAsync(c => c.Id == input.Id && c.CustomerId == CustomerSession.CustomerId);
            if (entity.OrganizationUnits.Count > 0)
            {
                var ouId = entity.OrganizationUnits.Single().OrganizationUnitId;
                var ou = await ouRepository.GetAll().AsNoTrackingWithIdentityResolution().SingleAsync(c => c.Id == ouId);
                CurrentUnitOfWork.Items["ous"] = new Dictionary<long, CustomerOUEntity> { { entity.Id, ou } };
            }

            return MapToEntityDto(entity);
        }
        private StaffInfoDto MapToEntityDto(CustomerStaffInfoEntity entity)
        {
            var dto = ObjectMapper.Map<StaffInfoDto>(entity);
            if (CurrentUnitOfWork.Items.TryGetValue("ous", out var obj))
            {
                var ous = obj as Dictionary<long, CustomerOUEntity>;
                if (ous.TryGetValue(dto.Id, out var ou))
                {
                    dto.OuId = ou?.Id;
                    dto.OuName = ou?.DisplayName;
                }
            }
            return dto;
        }
        //private async Task Hnad(User staff, long ouId)
        //{
        //    await userManager.SetOrganizationUnitsAsync(staff.Id, ouIds.ToArray());
        //}


    }
}