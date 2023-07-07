using Abp.Domain.Repositories;
using BXJG.Utils.GeneralTree;
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
using Abp.UI;
using Abp.Localization;
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
            base.LocalizationSourceName = CoreConsts.LocalizationSourceName;
        }

        //移动到WorkOrderCategoryRepositoryExtensions中了
        ///// <summary>
        ///// 获取查询条件，只要该类别所关联的工单类型包含workOrderTypes中的任何一个就认为符合条件
        ///// </summary>
        ///// <param name="workOrderTypes"></param>
        ///// <param name="containsNullWorkOrderType"></param>
        ///// <returns></returns>
        //public virtual Expression<Func<CategoryEntity, bool>> GetWhereExpression(IEnumerable<string> workOrderTypes, bool containsNullWorkOrderType)
        //{
        //    Expression<Func<CategoryEntity, bool>> where = c => true;
        //    if (workOrderTypes != null && workOrderTypes.Any())
        //    {
        //        where = c => workOrderTypes.Any(d => c.WorkOrderTypes.Any(e => e.WorkOrderType == d));
        //        if (containsNullWorkOrderType)
        //        {
        //            where = where.Or(c => !c.WorkOrderTypes.Any());
        //        }
        //    }
        //    return where;
        //}

        /// <summary>
        /// 根据工单类型处理默认分类，
        /// 确保未关联工单类型的类别有且只有一个默认类别，
        /// 确保每个工单类型无（共用无关联工单类型的默认分类）或只有一个默认工单类别
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async ValueTask HandSaveDefaultAsync(CategoryEntity entity)
        {
            //using var scop = UnitOfWorkManager.Begin();

            if (entity.WorkOrderTypes.Any())
            {
                entity.IsDefault = false;

                #region 检查是否存在不支持的工单类型
                var str = string.Empty;
                foreach (var item in entity.WorkOrderTypes)
                {
                    if (!workOrderTypeManager.ContainsKey(item.WorkOrderType))
                        str += workOrderTypeManager[item.WorkOrderType].DisplayName.Localize(LocalizationManager) + ",";
                }
                if (!str.IsNullOrWhiteSpace())
                    throw new ApplicationException($"不支持的工单类型！{str}");
                #endregion

                #region 确保一个工单类型只能有一个默认类别
                var ts = entity.WorkOrderTypes.Where(c => c.IsDefault).Select(c => c.WorkOrderType);
                if (ts.Any())
                {
                    var query = repository.GetAllIncluding(c => c.WorkOrderTypes)
                                          .Where(c => c.WorkOrderTypes.Any(d => ts.Any(e => e == d.WorkOrderType) && d.IsDefault))
                                          .Where(c => c.Id != entity.Id);
                    var cls = await AsyncQueryableExecuter.ToListAsync(query);
                    cls.ForEach(item =>
                    {
                        //item.IsDefault = false;
                        foreach (var workOrderType in item.WorkOrderTypes)
                        {
                            workOrderType.IsDefault = false;
                        }
                    });
                }
                #endregion
            }
            else
            {
                #region 确保未关联工单类型的分类有且只有一个默认类别
                if (entity.IsDefault)
                {
                    var query1 = repository.GetAll().Where(c => !c.WorkOrderTypes.Any() && c.Id != entity.Id && c.IsDefault);
                    var cls1 = await AsyncQueryableExecuter.ToListAsync(query1);
                    foreach (var item in cls1)
                    {
                        item.IsDefault = false;
                    }
                }
                #endregion
            }
            await CurrentUnitOfWork.SaveChangesAsync();
            var qq = repository.GetAll().Where(c => !c.WorkOrderTypes.Any() && c.IsDefault);
            if ((await AsyncQueryableExecuter.CountAsync(qq)) > 1)
                throw new UserFriendlyException("共享的类别有有且只能有一个默认类别！".BXJGWorkOrderL());
        }

        //移动到WorkOrderCategoryRepositoryExtensions中了
        ///// <summary>
        ///// 根据工单类型获取默认类别
        ///// 若指定工单类别包含指定工单类型，且是默认时返回，否则尝试获取无关联工单类型的类别，且已设为默认的作为返回
        ///// </summary>
        ///// <param name="workOrderType"></param>
        ///// <returns></returns>
        //public virtual async Task<CategoryEntity> GetDefaultAsync(string workOrderType)
        //{
        //    if (!workOrderType.IsNullOrWhiteSpace())
        //    {
        //        var entity = await repository.GetAll().Where(c => c.WorkOrderTypes.Any(d => d.WorkOrderType == workOrderType && d.IsDefault)).SingleOrDefaultAsync();
        //        if (entity != null)
        //            return entity;
        //    }
        //    var top = await repository.GetAll().SingleOrDefaultAsync(c => !c.WorkOrderTypes.Any() && c.IsDefault);
        //    if (top == default)
        //        throw new UserFriendlyException(L("请在工单类别中设置默认类别"));
        //    return top;
        //}

        //新增或修改以及并发情况下确保一个类型或无关联工单类型保持一个默认类别，但是目前考虑关系不大

        public override async Task<CategoryEntity> CreateAsync(CategoryEntity entity)
        {
            await HandSaveDefaultAsync(entity);
            return await base.CreateAsync(entity);
        }

        public override async Task<CategoryEntity> UpdateAsync(CategoryEntity entity)
        {
            var ts = entity.WorkOrderTypes.ToList();
            entity = await AsyncQueryableExecuter.FirstOrDefaultAsync(repository.GetAllIncluding(c => c.WorkOrderTypes).Where(c => c.Id == entity.Id));
            entity.WorkOrderTypes.Clear();
            entity.WorkOrderTypes = ts;
            await HandSaveDefaultAsync(entity);
            return await base.UpdateAsync(entity);
        }
    }
}
