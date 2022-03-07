using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using BXJG.Utils.Dto;
using BXJG.Utils.File;
using BXJG.WorkOrder.WorkOrderCategory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace BXJG.WorkOrder.WorkOrder
{
    /// <summary>
    /// 获取任意工单类型列表时的条件模型
    /// </summary>
    public class GetTotalInputBase1
    {
        /// <summary>
        /// 包含这些工单类型的
        /// </summary>
        public string[] WorkOrderType { get; set; }
        /// <summary>
        /// 处理人Id
        /// </summary>
        public virtual string EmployeeId { get; set; }
        /// <summary>
        /// 只包含在这几种状态内的工单
        /// </summary>
        public virtual Status[] Statuses { get; set; }
        /// <summary>
        /// 只包含在这几种紧急程度内的工单
        /// </summary>
        public virtual UrgencyDegree[] UrgencyDegrees { get; set; }
        /// <summary>
        /// 这包含这几种工单类别的
        /// </summary>
        public virtual string[] CategoryCodes { get; set; }
        /// <summary>
        /// 预计开始时间范围-开始
        /// </summary>
        public virtual DateTimeOffset? EstimatedExecutionTimeStart { get; set; }
        /// <summary>
        /// 预计结束时间范围-结束
        /// </summary>
        public virtual DateTimeOffset? EstimatedExecutionTimeEnd { get; set; }
        /// <summary>
        /// 预计完成时间范围-开始
        /// </summary>
        public virtual DateTimeOffset? EstimatedCompletionTimeStart { get; set; }
        /// <summary>
        /// 预计完成时间范围-结束
        /// </summary>
        public virtual DateTimeOffset? EstimatedCompletionTimeEnd { get; set; }
        /// <summary>
        /// 实际开始时间-开始
        /// </summary>
        public virtual DateTimeOffset? ExecutionTimeStart { get; set; }
        /// <summary>
        /// 实际开始时间-结束
        /// </summary>
        public virtual DateTimeOffset? ExecutionTimeEnd { get; set; }
        /// <summary>
        /// 实际完成时间-开始
        /// </summary>
        public virtual DateTimeOffset? CompletionTimeStart { get; set; }
        /// <summary>
        /// 实际完成实际-结束
        /// </summary>
        public virtual DateTimeOffset? CompletionTimeEnd { get; set; }
        /// <summary>
        /// 关键字，模糊匹配处理人名称、电话、工单标题等
        /// </summary>
        public virtual string Keyword { get; set; }
    }
    /// <summary>
    /// 后台管理工单时，查询所有类型的工单的应用服务
    /// </summary>
    public class WorkOrderBaseAppService<TDto, TFilter> : AppServiceBase
        where TFilter : GetTotalInputBase1
    {
        protected readonly IRepository<OrderBaseEntity, long> repository;
        protected readonly IRepository<CategoryEntity, long> categoryRepository;

        private readonly string getPermissionName;

        public WorkOrderBaseAppService(IRepository<OrderBaseEntity, long> orderBaseRepository,
                                       IRepository<CategoryEntity, long> categoryRepository,
                                       string getPermissionName = default)

        {
            this.repository = orderBaseRepository;
            this.getPermissionName = getPermissionName;
            this.categoryRepository = categoryRepository;
        }
        protected virtual Task CheckGetPermissionAsync()
        {
            return CheckPermissionAsync(getPermissionName);
        }
        public virtual async Task<PagedResultDto<TDto>> GetListAsync(PagedAndSortedResultRequest<TFilter> input)
        {
            await CheckGetPermissionAsync();
            var query = await GetAllFilterAsync(input.Filter);
            var count = await query.CountAsync();
            query = OrderBy(query, input);
            query = PageBy(query, input);
            var list = await query.ToListAsync();



            //var images = await CreateAttachmentManager("").GetAttachmentsAsync(entityIds: list.Select(c => c.Order.Id.ToString()).ToArray());

            var items = new List<TDto>();
            foreach (var item in list)
            {
                TDto ttt = EntityToDto(item);
                items.Add(ttt);
            }
            return new PagedResultDto<TDto>(count, items);
        }

        private TDto EntityToDto(TQueryTemp item)
        {
            throw new NotImplementedException();
        }

        protected class TQueryTemp
        {
            public OrderBaseEntity Order { get; set; }
            public CategoryEntity Category { get; set; }
        }

        /// <summary>
        /// 获取基础的查询对象
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        protected virtual IQueryable<TQueryTemp> GetBaseQueryFilter(TFilter filter)
        {
            var query = from c in repository.GetAll().AsNoTrackingWithIdentityResolution()
                        join lb in categoryRepository.GetAll().AsNoTrackingWithIdentityResolution() on c.CategoryId equals lb.Id into g
                        from kk in g.DefaultIfEmpty()
                        select new TQueryTemp { Order = c, Category = kk };
            return query;
        }
        /// <summary>
        /// 应用条件筛选
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        protected virtual async Task<IQueryable<TQueryTemp>> GetAllFilterAsync(TFilter input)
        {
            var query = GetQuery();
            //if (input.CategoryCodes != null)
            //{
            //    //Expression<Func<TQueryTemp, bool>> where = c => false;
            //    //foreach (var item in input.CategoryCodes)
            //    //{
            //    //    where = where.Or(c => c.Category.Code.StartsWith(item));
            //    //}
            //    query = query.Where(await ApplyCls(input.CategoryCodes));
            //}
            //var query = query1.Select(c => c.Order);


            //var empIdsQuery = await GetEmployeeIdsAsync(input);
            //if (empIdsQuery != null && empIdsQuery.Count() > 0)
            //    query = query.Where(c => empIdsQuery.Contains(c.Order.EmployeeId));

            //应用基本条件过滤
            query = await ApplyOther(query, input);
            //关键字条件过滤比较特殊，定义在单独的方法中，允许重写
            query = query.WhereIf(!input.Keyword.IsNullOrWhiteSpace(), await ApplyKeyword(input.Keyword));
            //query = await GetAndAllFilterAsync(query);
            return query;
        }
        /// <summary>
        /// 获取查询对象，默认repository.GetAll()
        /// 通常，当你的查询需要include时可以重写此方法
        /// </summary>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> GetOrderQuery()
        {
            return repository.GetAll();
        }
        /// <summary>
        /// 获取分类的查询对象，默认categoryRepository.Value.GetAll()
        /// </summary>
        /// <returns></returns>
        protected virtual IQueryable<CategoryEntity> GetClsQuery()
        {
            return categoryRepository.Value.GetAll();
        }
        protected virtual IQueryable<TQueryTemp> GetQuery()
        {
            var query = from c in GetOrderQuery().AsNoTrackingWithIdentityResolution()
                        join lb in GetClsQuery().AsNoTrackingWithIdentityResolution() on c.CategoryId equals lb.Id into g
                        from kk in g.DefaultIfEmpty()
                        select new TQueryTemp { Order = c, Category = kk };
            return query;
        }
        protected virtual IQueryable<TQueryTemp> PageBy(IQueryable<TQueryTemp> query, IPagedResultRequest input)
        {
            return query.PageBy(input);
        }
        protected virtual IQueryable<TQueryTemp> OrderBy(IQueryable<TQueryTemp> query, ISortedResultRequest input)
        {
            return query.OrderBy(input.Sorting);
        }
        protected virtual ValueTask<Expression<Func<TQueryTemp, bool>>> ApplyKeyword(string keyword)
        {
            Expression<Func<TQueryTemp, bool>> where = c => c.Order.Title.Contains(keyword) || c.Order.Description.Contains(keyword);
            return ValueTask.FromResult(where);
        }
        protected virtual ValueTask<IQueryable<TQueryTemp>> ApplyOther(IQueryable<TQueryTemp> query, TFilter input)
        {
            if (input.CategoryCodes != null)
            {
                Expression<Func<TQueryTemp, bool>> where = c => false;
                foreach (var item in input.CategoryCodes)
                {
                    where = where.Or(c => c.Category.Code.StartsWith(item));
                }
                query = query.Where(where);
            }
            query = query.WhereIf(input.UrgencyDegrees != null, c => input.UrgencyDegrees.Contains(c.Order.UrgencyDegree))
                         .WhereIf(input.Statuses != null, c => input.Statuses.Contains(c.Order.Status))
                         .WhereIf(!input.EmployeeId.IsNullOrWhiteSpace(), c => c.Order.EmployeeId == input.EmployeeId)
                         .WhereIf(input.EstimatedExecutionTimeStart.HasValue, c => c.Order.EstimatedExecutionTime >= input.EstimatedExecutionTimeStart)
                         .WhereIf(input.EstimatedExecutionTimeEnd.HasValue, c => c.Order.EstimatedExecutionTime < input.EstimatedExecutionTimeEnd)
                         .WhereIf(input.EstimatedCompletionTimeStart.HasValue, c => c.Order.EstimatedCompletionTime >= input.EstimatedCompletionTimeStart)
                         .WhereIf(input.EstimatedCompletionTimeEnd.HasValue, c => c.Order.EstimatedCompletionTime < input.EstimatedCompletionTimeEnd)
                         .WhereIf(input.ExecutionTimeStart.HasValue, c => c.Order.ExecutionTime >= input.ExecutionTimeStart)
                         .WhereIf(input.ExecutionTimeEnd.HasValue, c => c.Order.ExecutionTime < input.ExecutionTimeEnd)
                         .WhereIf(input.CompletionTimeStart.HasValue, c => c.Order.CompletionTime >= input.CompletionTimeStart)
                         .WhereIf(input.CompletionTimeEnd.HasValue, c => c.Order.CompletionTime < input.CompletionTimeEnd);
            return ValueTask.FromResult(query);
        }

    }
}
