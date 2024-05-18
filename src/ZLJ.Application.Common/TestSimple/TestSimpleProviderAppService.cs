
using ZLJ.Core.TestSimple;
using ZLJ.Application.Common.Share.TestSimple;
using Abp.Authorization;
using BXJG.Utils.Application.Share.GeneralTree;
using BXJG.Utils.Application.Share.Dtos;
namespace ZLJ.Application.Common.TestSimple
{
    /// <summary>
    /// 获取 普通数据测试 以供选择
    ///</summary>
    [AbpAuthorize]
    public class TestSimpleProviderAppService : 
                                                    CommonProviderBaseAppService<TestSimpleEntity,PagedAndSortedResultRequest<TestSimpleProviderCondition>,TestSimpleProviderDto,long>
    {
        protected override IQueryable<TestSimpleEntity> CreateFilteredQuery(PagedAndSortedResultRequest<TestSimpleProviderCondition> input)
        {

            var query = base.CreateFilteredQuery(input);
            var condition = input.Filter;
    
            query = query.WhereIf(condition.Keywords.IsNotNullOrWhiteSpaceBXJG(), x => 
                x.Name.Contains(condition.Keywords)
                || x.StringField1.Contains(condition.Keywords)
            );

            query = query.WhereIf(condition.AgeMin.HasValue,x=> x.Age >= condition.AgeMin.Value );
            query = query.WhereIf(condition.AgeMax.HasValue,x=> x.Age < condition.AgeMax.Value );
            query = query.WhereIf(condition.BirthdayMin.HasValue,x=> x.Birthday >= condition.BirthdayMin.Value );
            query = query.WhereIf(condition.BirthdayMax.HasValue,x=> x.Birthday < condition.BirthdayMax.Value );
            query = query.WhereIf(condition.F2Min.HasValue,x=> x.F2 >= condition.F2Min.Value );
            query = query.WhereIf(condition.F2Max.HasValue,x=> x.F2 < condition.F2Max.Value );

            query = query.WhereIf(condition.Status.HasValue,x=> x.Status == condition.Status.Value );
            query = query.WhereIf(condition.F3.HasValue,x=> x.F3 == condition.F3.Value );

            return query;
        }
    }
}