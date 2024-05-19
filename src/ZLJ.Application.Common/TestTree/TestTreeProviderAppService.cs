
using ZLJ.Core.TestTree;
using ZLJ.Application.Common.Share.TestTree;
using Abp.Authorization;
using BXJG.Utils.Application.Share.GeneralTree;
using BXJG.Utils.Application.Share.Dtos;
namespace ZLJ.Application.Common.TestTree
{
    /// <summary>
    /// 获取 测试树 以供选择
    ///</summary>
    [AbpAuthorize]
    public class TestTreeProviderAppService : 

                                                     CommonTreeProviderBaseAppService<TestTreeEntity, 
                                                                                      TestTreeProviderCondition,
                                                                                      TestTreeProviderDto,
                                                                                      GeneralTreeGetForSelectInput,
                                                                                      GeneralTreeComboboxDto>
    {
        protected override IQueryable<TestTreeEntity> ComboTreeFilter(TestTreeProviderCondition condition, string parentCode)
        {

            var query = base.ComboTreeFilter(condition, parentCode);
        
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