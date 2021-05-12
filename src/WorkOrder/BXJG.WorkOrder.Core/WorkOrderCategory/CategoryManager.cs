using Abp.Domain.Repositories;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;
using Abp.Linq;
using Abp.Linq.Extensions;
using Abp.Linq.Expressions;
using System.Threading.Tasks;
using BXJG.WorkOrder.WorkOrderType;
using System.Linq;
using BXJG.Utils.Localization;
using Abp.Extensions;
using System.Linq.Expressions;

namespace BXJG.WorkOrder.WorkOrderCategory
{
    /// <summary>
    /// 工单分类领域服务
    /// </summary>
    public class CategoryManager : GeneralTreeManager<CategoryEntity>
    {
        /// <summary>
        /// 工单类型管理器
        /// </summary>
        protected readonly WorkOrderTypeManager workOrderTypeManager;

        public CategoryManager(IRepository<CategoryEntity, long> repository, WorkOrderTypeManager workOrderTypeManager) : base(repository)
        {
            this.workOrderTypeManager = workOrderTypeManager;
        }
     
        //public virtual IQueryable<CategoryEntity> GetAllQueryable(IQueryable<CategoryEntity> query, IEnumerable<string> workOrderTypes, bool containsNullWorkOrderType)
        //{
        //    if (workOrderTypes != null && workOrderTypes.Any())
        //    {
        //        Expression<Func<CategoryEntity, bool>> where1 = c => workOrderTypes.Any(d => c.WorkOrderTypes.Any(e => e.WorkOrderType == d));

        //        if (containsNullWorkOrderType)
        //        {
        //            where1 = where1.Or(c => c.WorkOrderTypes == null);
        //        }

        //        return query.Where(where1);

        //    }

        //    return query;
        //}
       
        public virtual Expression<Func<CategoryEntity, bool>> GetWhereExpression(IEnumerable<string> workOrderTypes, bool containsNullWorkOrderType)
        {
            Expression<Func<CategoryEntity, bool>> where = c => true;
            if (workOrderTypes != null && workOrderTypes.Any())
            {
                where = c => workOrderTypes.Any(d => c.WorkOrderTypes.Any(e => e.WorkOrderType == d));
                if (containsNullWorkOrderType)
                {
                    where = where.Or(c => !c.WorkOrderTypes.Any());
                }
            }
            return where;
        }

        /// <summary>
        /// 根据工单类型处理默认分类，
        /// 确保未关联工单类型的类别有且只有一个默认类别，
        /// 确保每个工单类型无（共用无关联工单类型的默认分类）或只有一个默认工单类别
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async ValueTask HandDefaultAsync(CategoryEntity entity)
        {
            //using var scop = UnitOfWorkManager.Begin();

            if (entity.WorkOrderTypes.Count > 0)
            {
                #region 检查是否存在不支持的工单类型
                var str = string.Empty;
                foreach (var item in entity.WorkOrderTypes)
                {
                    if (!workOrderTypeManager.ContainsKey(item.WorkOrderType))
                        str += item.WorkOrderType.GeneralTreeL() + ",";
                }
                if (!str.IsNullOrWhiteSpace())
                    throw new ApplicationException($"不支持的工单类型！{str}");
                #endregion

                #region 确保一个工单类型只能有一个默认类别
                var query = repository.GetAllIncluding(c => c.WorkOrderTypes)
                                      .Where(c => c.WorkOrderTypes.Any(d => entity.WorkOrderTypes.Any(e => e.WorkOrderType == d.WorkOrderType)))
                                      .Where(c => c.Id != entity.Id);
                var cls = await AsyncQueryableExecuter.ToListAsync(query);
                cls.ForEach(item =>
                {
                    foreach (var workOrderType in item.WorkOrderTypes)
                    {
                        workOrderType.IsDefault = false;
                    }
                });
                #endregion
                return;
            }

            #region 确保未关联工单类型的分类有且只有一个默认类别
            if (!entity.IsDefault)
                return;
            var query1 = repository.GetAll().Where(c => !c.WorkOrderTypes.Any() && c.Id != entity.Id);
            var cls1 = await AsyncQueryableExecuter.ToListAsync(query1);
            foreach (var item in cls1)
            {
                item.IsDefault = false;
            }
            #endregion

            //await scop.CompleteAsync();
        }
    }
}
