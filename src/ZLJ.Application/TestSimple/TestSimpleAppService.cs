using ZLJ.Core.TestSimple;
using ZLJ.Application.Common.Share.TestSimple;
using ZLJ.Application.Share.TestSimple;
using Abp.Authorization;
using BXJG.Utils.Application.Share.Dtos;
using Abp.Notifications;
using Microsoft.Extensions.Configuration;

namespace ZLJ.Application.TestSimple
{
    /// <summary>
    /// 后台管理 普通数据测试 应用服务
    ///</summary>
    //[AbpAuthorize(TestSimpleApplicationShareConsts.PermissionNameGet)]
    public class TestSimpleAppService : AdminCrudBaseAppService<TestSimpleEntity, 
                                                                                             TestSimpleDto, 
                                                                                             long, 
                                                                                             PagedAndSortedResultRequest<TestSimpleCondition>, 
                                                                                             TestSimpleCreateDto, 
                                                                                             TestSimpleEditDto>
    {
        IConfiguration configuration;
        public TestSimpleAppService(IRepository<TestSimpleEntity, long> repository, IConfiguration configuration) : base(repository)
        {
            this.configuration = configuration;
        }

        //提供权限名称
        protected override string GetPermissionName => TestSimpleApplicationShareConsts.PermissionNameGet;
        protected override string GetAllPermissionName => TestSimpleApplicationShareConsts.PermissionNameGet;
        protected override string CreatePermissionName => TestSimpleApplicationShareConsts.PermissionNameCreate;
        protected override string UpdatePermissionName => TestSimpleApplicationShareConsts.PermissionNameUpdate;
        protected override string DeletePermissionName => TestSimpleApplicationShareConsts.PermissionNameDelete;
        public INotificationPublisher notificationPublisher { get; set; }

        public override async Task<PagedResultDto<TestSimpleDto>> GetAllAsync(PagedAndSortedResultRequest<TestSimpleCondition> input)
        {
           await notificationPublisher.PublishAsync("xvsdf", 
                                                    new MessageNotificationData(DateTime.Now.ToLongTimeString()),
                                                    userIds:[new UserIdentifier(AbpSession.TenantId, AbpSession.UserId.Value)]);
            return await base.GetAllAsync(input);
        }

        public override async Task<TestSimpleDto> UpdateAsync(TestSimpleEditDto input)
        {
          await  base.SettingManager.ChangeSettingForApplicationAsync("Abp.Net.Mail.DefaultFromDisplayName", DateTime.Now.ToLongTimeString());

            var sdfsdf = configuration["Abp.Net.Mail.DefaultFromDisplayName"];
            await Task.Delay(7000);
            base.Logger.Info("测试abp setting修改时，新的配置："+ sdfsdf);
            return await base.UpdateAsync(input);
        }

        //protected override async ValueTask MapToEntity(TestSimpleEntity entity)
        //{
        //     await base.MapToEntity(input);
        //     新增和修改时，通过dto映射到entity之后都会调用，通常在这里对entity做进一步设置
        //}

        //protected override async Task DeleteCore(TestSimpleEntity entity)
        //{
        //      await base.DeleteCore(entity);
        //      删除的核心逻辑，通常在这里
        //}

        //protected override async Task<TestSimpleEntity> GetEntityByIdAsync(long id, bool track = true)
        //{
        //    return await base.GetEntityByIdAsync(id,track);
        //    crud都会调用，获取单个实体，通常在这里Include更多导航属性，另外参考BuildQuery方法
        //}

        //protected override IQueryable<TestSimpleEntity> CreateFilteredQuery(PagedAndSortedResultRequest<TestSimpleCondition> input)
        //{
        //    return base.CreateFilteredQuery(input);
        //    获取分页数据时调用，通常在这里Include更多导航属性，另外参考BuildQuery方法
        //}

        //protected override IQueryable<TestSimpleEntity> BuildQuery(bool track = true)
        //{
        //     return base.BuildQuery(track);
        //     通常在这里Include获取单个数据和列表时都要的导航属性
        //}
    }
}