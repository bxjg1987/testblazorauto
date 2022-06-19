using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Expressions;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrderCategory
{
    
    [Flags]
    public enum CategoryTypeQueryType
    {
        /// <summary>
        /// 只获取共享的
        /// </summary>
        OnlyShared=0,
        /// <summary>
        /// 包含在传入的类型列表中的
        /// </summary>
        Included=1,
        /// <summary>
        /// 不包含在传入的类型列表中的
        /// </summary>
        Excluded=2
    }
    /// <summary>
    /// 工单仓储扩展
    /// </summary>
    public static class WorkOrderCategoryExtensions1
    {
        ///// <summary>
        ///// 根据工单类型获取默认类别
        ///// 若指定工单类别包含指定工单类型，且是默认时返回，否则尝试获取无关联工单类型的类别，且已设为默认的作为返回
        ///// </summary>
        ///// <param name="repository"></param>
        ///// <param name="workOrderType"></param>
        ///// <returns></returns>
        //public static async Task<CategoryEntity> GetDefaultAsync(this IRepository<CategoryEntity, long> repository, string workOrderType)
        //{
        //    if (!workOrderType.IsNullOrWhiteSpace())
        //    {
        //        var entity = await repository.GetAll().Where(c => c.WorkOrderTypes.Any(d => d.WorkOrderType == workOrderType && d.IsDefault)).SingleOrDefaultAsync();
        //        if (entity != default)
        //            return entity;
        //    }
        //    var top = await repository.GetAll().SingleOrDefaultAsync(c => !c.WorkOrderTypes.Any() && c.IsDefault);
        //    if (top == default)
        //        throw new UserFriendlyException("请在工单类别中设置默认类别".BXJGWorkOrderL());
        //    return top;
        //}

        /// <summary>
        /// 获取查询条件，只要该类别所关联的工单类型包含workOrderTypes中的任何一个就认为符合条件
        /// </summary>
        /// <param name="query"></param>
        /// <param name="workOrderTypes"></param>
        /// <param name="categoryTypeQueryType"></param>
        /// <param name="containsNullWorkOrderType"></param>
        /// <returns></returns>
        public static IQueryable<CategoryEntity> WhereWorkOrderType(this IQueryable<CategoryEntity> query, CategoryTypeQueryType? categoryTypeQueryType = default, IEnumerable<string> workOrderTypes = default, bool? containsNullWorkOrderType = true)
        {
            Expression<Func<CategoryEntity, bool>> where = c => true;
            if (categoryTypeQueryType == CategoryTypeQueryType.OnlyShared)
            {
                where = c => !c.WorkOrderTypes.Any();
            }
            else if (workOrderTypes != null && workOrderTypes.Any())
            {
                //where = c => workOrderTypes.Any(d => c.WorkOrderTypes.Any(e => e.WorkOrderType == d));
                if (categoryTypeQueryType == CategoryTypeQueryType.Included)
                    where = c => c.WorkOrderTypes.Any(d => workOrderTypes.Any(e => e == d.WorkOrderType));
                else
                    where = c => !c.WorkOrderTypes.Any(d => workOrderTypes.Any(e => e == d.WorkOrderType));

                if (containsNullWorkOrderType == true)
                {
                    where = where.Or(c => !c.WorkOrderTypes.Any());
                }
            }
            return query.Where(where);
        }
    }
}
