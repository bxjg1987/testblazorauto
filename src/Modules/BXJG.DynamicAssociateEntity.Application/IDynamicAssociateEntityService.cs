using Abp.Application.Services.Dto;
using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.DynamicAssociateEntity
{
    public interface IDynamicAssociateEntityService : ITransientDependency
    {
        //IEnumerable<DefineMapItem> defines 此参数就不定义了，模块内部可以定义静态属性注册时存储下来，放心静态变量引用的也是同一个动态数据定义实例
        //因为如果要传递此参数，这每个方法都应该传

        Task<PagedResultDto<object>> GetAllAsync(string parentId, string keyword, string sorting, int skip, int maxcount);
        Task<IList<object>> GetAllNoPageAsync(string parentId, string keyword, string sorting);


        //下面这2个接口，动态关联实体模块并不使用
        //但工单也不能直接私下强关联设备或栏目、文章，所以虽然模块内部不使用，但它是一个规范

        ///// <summary>
        ///// 查询返回id、排序值
        ///// </summary>
        ///// <param name="sort"></param>
        ///// <param name="keyword"></param>
        ///// <param name="ids"></param>
        ///// <returns></returns>
        //Task<IEnumerable<IdSortDto>> GetIdsAndSortValuesAsync(string sort = default, string keyword = default, params IEnumerable<object>[] ids);
        Task<IEnumerable<object>> GetAllByIdsAsync(IEnumerable<object> ids);
    }
    //public class IdSortDto
    //{
    //    public object Id { get; set; }
    //    public object SortValue { get; set; }
    //}
}
