using ZLJ.Core.TestTree;
using ZLJ.Application.Common.Share.TestTree;
using ZLJ.Application.Share.TestTree;
using Abp.Authorization;
using BXJG.Utils.Application.Share.Dtos;

namespace ZLJ.Application.TestTree
{
    /// <summary>
    /// 测试树 应用服务
    ///</summary>
    [AbpAuthorize(TestTreeApplicationShareConsts.PermissionNameGet)]
    public class TestTreeAppService : AdminTreeCrudBaseAppService<TestTreeEntity, TestTreeDto, TestTreeCreateDto, TestTreeEditDto, TestTreeCondition>
    {
        //提供权限名称
        protected override string GetPermissionName => TestTreeApplicationShareConsts.PermissionNameGet;
        protected override string CreatePermissionName => TestTreeApplicationShareConsts.PermissionNameCreate;
        protected override string UpdatePermissionName => TestTreeApplicationShareConsts.PermissionNameUpdate;
        protected override string DeletePermissionName => TestTreeApplicationShareConsts.PermissionNameDelete;
        
        //protected override async ValueTask MapToEntity(TestTreeEntity entity)
        //{
        //     await base.MapToEntity(input);
        //     新增和修改时，通过dto映射到entity之后都会调用，通常在这里对entity做进一步设置
        //}
       
        //protected override async Task DeleteCore(TestTreeEntity entity)
        //{
        //      await base.DeleteCore(entity);
        //      删除的核心逻辑，通常在这里
        //}

        //protected override async Task<TestTreeEntity> GetEntityByIdAsync(long id, bool track = true)
        //{
        //    return await base.GetEntityByIdAsync(id, track);
        //    crud都会调用，获取单个实体，通常在这里Include更多导航属性，另外参考BuildQuery方法
        //}

        //protected override IQueryable<TestTreeEntity> GetAllFilter(IQueryable<TestTreeEntity> q, TestTreeCondition input, string parentCode)
        //{
        //    return base.GetAllFilter(q, input, parentCode);
        //    获取分页数据时调用，通常在这里Include更多导航属性，另外参考BuildQuery方法
        //}

        //protected override IQueryable<TestTreeEntity> BuildQuery(bool track = true)
        //{
        //     return base.BuildQuery(track);
        //     通常在这里Include获取单个数据和列表时都要的导航属性
        //}
    }
}