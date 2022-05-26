using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Timing;
using BXJG.Common.Dto;
using BXJG.Utils.GeneralTree;
using BXJG.WorkOrder.WorkOrderCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Abp.Domain.Entities;
using Abp.UI;
using Abp.Dependency;
using Abp.Domain.Uow;
//using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using Abp.Application.Services;
using BXJG.Utils.File;
using BXJG.WorkOrder.WorkOrder;
using System.Linq.Dynamic;
using Abp.Linq.Expressions;
using System.Linq.Expressions;
using BXJG.WorkOrder.WorkOrderType;
using Microsoft.EntityFrameworkCore;
using BXJG.WorkOrder.Session;
using Abp.EntityHistory;

namespace BXJG.WorkOrder.EmployeeApplication.WorkOrder
{
    /// <summary>
    /// 工单处理人针对工单的相关操作接口，
    /// 不同类型的工单应提供不同的子类实现
    /// </summary>
    /// <typeparam name="TGetInput">获取单个工单的输入模型</typeparam>
    /// <typeparam name="TGetAllInput">获取工单分页列表时的输入模型</typeparam>
    /// <typeparam name="TGetTotalInput">获取工单数量时的输入模型</typeparam>
    /// <typeparam name="TDto">工单显示模型</typeparam>
    /// <typeparam name="TBatchAllocateInput">批量领取时的输入模型</typeparam>
    /// <typeparam name="TBatchAllocateOutput">批量领取时的输出模型</typeparam>
    /// <typeparam name="TBatchExcuteInput">批量执行时的输入模型</typeparam>
    /// <typeparam name="TBatchExcuteOutput">批量执行时的输出模型</typeparam>
    /// <typeparam name="TBatchCompletionInput">批量完成时的输入模型</typeparam>
    /// <typeparam name="TBatchCompletionOutput">批量完成时的输出模型</typeparam>
    /// <typeparam name="TBatchRejectInput">批量拒绝时的输入模型</typeparam>
    /// <typeparam name="TBatchRejectOutput">批量拒绝时的输出模型</typeparam>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TRepository">实体仓储类型</typeparam>
    /// <typeparam name="TManager">领域服务类型</typeparam>
    /// <typeparam name="TQueryTemp"></typeparam>
    public abstract class WorkOrderAppServiceBase<TGetInput,
                                                  TGetAllInput,
                                                  TGetTotalInput,
                                                  TDto,
                                                  TBatchAllocateInput,
                                                  TBatchAllocateOutput,
                                                  TBatchExcuteInput,
                                                  TBatchExcuteOutput,
                                                  TBatchCompletionInput,
                                                  TBatchCompletionOutput,
                                                  TBatchRejectInput,
                                                  TBatchRejectOutput,
                                                  TEntity,
                                                  TRepository,
                                                  TManager,
                                                  TQueryTemp> : AppServiceBase
        #region MyRegion
        where TGetInput : EntityDto<long>
        where TGetTotalInput : GetTotalInputBase, new()
        where TGetAllInput : GetAllInputBase<TGetTotalInput>
        where TDto : WorkOrderDtoBase, new()
        where TBatchAllocateInput : BatchAllocateInputBase
        where TBatchAllocateOutput : BatchOperationOutputLong, new()
        where TBatchExcuteInput : BatchChangeStatusInputBase
        where TBatchExcuteOutput : BatchOperationOutputLong, new()
        where TBatchCompletionInput : BatchChangeStatusInputBase
        where TBatchCompletionOutput : BatchOperationOutputLong, new()
        where TBatchRejectInput : BatchChangeStatusInputBase
        where TBatchRejectOutput : BatchOperationOutputLong, new()
        where TEntity : OrderBaseEntity
        where TRepository : IRepository<TEntity, long>
        where TManager : OrderBaseManager<TEntity>
        where TQueryTemp : QueryTemp<TEntity>, new()
        #endregion
    {
        #region MyRegion
        protected readonly TRepository repository;
        protected readonly Lazy<IRepository<CategoryEntity, long>> categoryRepository;
        protected readonly Lazy<TManager> manager;
        protected readonly Lazy<CategoryManager> clsManager;
        protected readonly Lazy<AttachmentManager<TEntity>> attachmentManager;
        protected readonly WorkOrderTypeDefine workOrderTypeDefine;
        protected readonly string getPermissionName,allocatePermissionName,executePermissionName,completionPermissionName,rejectPermissionName;
        #endregion

        #region 构造函数
        /// <summary>
        /// 工单处理人员端应用服务基类构造函数
        /// </summary>
        /// <param name="repository">工单仓储</param>
        /// <param name="empSession">处理人员session</param>
        /// <param name="manager">工单领域服务</param>
        /// <param name="categoryRepository">工单类别仓储</param>
        /// <param name="clsManager">工单类领域服务</param>
        /// <param name="workOrderType">工单类型</param>
        /// <param name="attachmentManager">附件管理器</param>
        /// <param name="workOrderTypeManager">工单类型定义</param>
        /// <param name="getPermissionName">获取工单的权限名</param>
        /// <param name="allocatePermissionName">领取工单的权限名</param>
        /// <param name="executePermissionName">执行工单的权限名</param>
        /// <param name="completionPermissionName">完成工单的权限名</param>
        /// <param name="rejectPermissionName">拒绝工单的权限名</param>
        public WorkOrderAppServiceBase(TRepository repository,
                                       IEmployeeSession empSession,
                                       Lazy<TManager> manager,
                                       Lazy<IRepository<CategoryEntity, long>> categoryRepository,
                                       Lazy<AttachmentManager<TEntity>> attachmentManager,
                                       Lazy<CategoryManager> clsManager,
                                       WorkOrderTypeManager workOrderTypeManager,
                                       string workOrderType,
                                       string getPermissionName = default,
                                       string allocatePermissionName = default,
                                       string executePermissionName = default,
                                       string completionPermissionName = default,
                                       string rejectPermissionName = default) : base(empSession)
        {
            this.repository = repository;
            this.manager = manager;
            this.categoryRepository = categoryRepository;
            this.clsManager = clsManager;
            this.attachmentManager = attachmentManager;
            this.workOrderTypeDefine = workOrderTypeManager[workOrderType];

            this.getPermissionName = getPermissionName;
            this.allocatePermissionName = allocatePermissionName;
            this.executePermissionName = executePermissionName;
            this.completionPermissionName = completionPermissionName;
            this.rejectPermissionName = rejectPermissionName;
        }
        #endregion

        /// <summary>
        /// 获取数量
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(false)]
        public virtual async Task<long> GetTotalAsync(TGetTotalInput input)
        {
            await CheckGetPermissionAsync();
            var query = await GetAllFilterAsync(input);
            //var sql = query.ToQueryString();
            return await AsyncQueryableExecuter.CountAsync(query);
        }
        /// <summary>
        /// 获取指定工单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(false)]
        public virtual async Task<TDto> GetAsync(TGetInput input)
        {
            await CheckGetPermissionAsync();
            var query = await GetFilterAsync(input);
            //var str = query.ToQueryString();
            var entity = await AsyncQueryableExecuter.FirstOrDefaultAsync(query);
            return await EntityToDto(entity);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(false)]
        public virtual async Task<PagedResultDto<TDto>> GetAllAsync(TGetAllInput input)
        {
            //分类、员工先查询 再用in，
            //假定员工和分类数量不会太多（太多的话考虑分配in查询），且可以使用缓存
            //in查询有索引时性能有所提升
            //按分类名称排序 倒是可用用join映射 select new { 工单实体，join的分类 } 后面再排序
            //按处理人和手机号比较麻烦，可以尝试join已经查询出来的员工列表试试
            //不过至少可用按分类id和处理人id排序
            //如果都无法满足时，可以考虑使用原始sql，毕竟这里只是查询需求，不做业务处理，可以引入dapper或ef的原始sql执行方式

            //var ss = dynamicAssociateEntityDefineManager.GroupedDefines;
            //var define = ss.First().Value.First();
            //var service = iocResolver.Resolve(define.ServiceType) as IDynamicAssociateEntityService;
            //var ss2 = await service.GetAllAsync(define, "a", "d");

            await CheckGetPermissionAsync();
            var query = await GetAllFilterAsync(input.GetTotalInput);
            var count = await AsyncQueryableExecuter.CountAsync(query);
            query = OrderBy(query, input);
            query = PageBy(query, input);
            //var str = query.ToQueryString();
            var list = await AsyncQueryableExecuter.ToListAsync(query);

            //var cIds = list.Select(c => c.CategoryId);
            //var cQuery = categoryRepository.GetAll().Where(c => cIds.Contains(c.Id));
            //var cls = await AsyncQueryableExecuter.ToListAsync(cQuery);

            //var empIds = list.Where(c => !c.Order.EmployeeId.IsNullOrWhiteSpace()).Select(c => c.Order.EmployeeId);

            //IEnumerable<EmployeeDto> emps = null;
            //if (empIds != null && empIds.Count() > 0)
            //{
            //    emps = await employeeAppService.GetByIdsAsync(empIds.ToArray());
            //}

            var images = await attachmentManager.Value.GetAttachmentsAsync( entityIds: list.Select(c => c.Order.Id.ToString()).ToArray());
            // var images2 = images.ToDictionary(c => c.Key, c =>c.Value);

            var state = await GetStateAsync(list);
            var items = new List<TDto>();
            foreach (var item in list)
            {
                var ttt = EntityToDto(item, images, state);
                items.Add(ttt);
            }
            return new PagedResultDto<TDto>(count, items);
        }
        /// <summary>
        /// 批量领取工单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UseCase(Description = "领取")]
        public virtual async Task<TBatchAllocateOutput> AllocateAsync(TBatchAllocateInput input)
        {
            await CheckAllocatePermissionAsync();
            var query = repository.GetAll().Where(c => input.Ids.Contains(c.Id));
            var list = await AsyncQueryableExecuter.ToListAsync(query);
            var r = new TBatchAllocateOutput();
            foreach (var item in list)
            {
                try
                {
                    var sw = UnitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.RequiresNew);
                    item.Allocate(Clock.Now, CurrentEmployeeId, input.EstimatedExecutionTime, input.EstimatedCompletionTime, input.StatusChangedDescription);
                    //await CurrentUnitOfWork.SaveChangesAsync();
                    await sw.CompleteAsync();
                    r.Ids.Add(item.Id);
                }
                catch (UserFriendlyException ex)
                {
                    r.ErrorMessage.Add(new BatchOperationErrorMessage(item.Id, ex.Message));
                }
                catch (Exception ex)
                {
                    r.ErrorMessage.Add(item.Id.Message500());
                    Logger.Warn(BXJGWorkOrderL("领取工单失败！"), ex);
                }
            }
            return r;
        }
        /// <summary>
        /// 批量执行
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UseCase(Description = "执行")]
        public virtual async Task<TBatchExcuteOutput> ExcuteAsync(TBatchExcuteInput input)
        {
            await CheckExecutePermissionAsync();
            var query = repository.GetAll().Where(c => input.Ids.Contains(c.Id));
            var list = await AsyncQueryableExecuter.ToListAsync(query);
            var r = new TBatchExcuteOutput();
            foreach (var item in list)
            {
                try
                {
                    if (item.EmployeeId != employeeSession.BusinessUserId)
                        throw new UserFriendlyException($"无权执行此操作！工单{item.Id + item.Title}不属于当前用户。");
                    var sw = UnitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.RequiresNew);
                    item.Execute(Clock.Now, input.StatusChangedDescription);
                   // await CurrentUnitOfWork.SaveChangesAsync();
                    await sw.CompleteAsync();
                    r.Ids.Add(item.Id);
                }
                catch (UserFriendlyException ex)
                {
                    r.ErrorMessage.Add(new BatchOperationErrorMessage(item.Id, ex.Message));
                }
                catch (Exception ex)
                {
                    r.ErrorMessage.Add(item.Id.Message500());
                    Logger.Warn(BXJGWorkOrderL("执行失败！"), ex);
                }
            }
            return r;
        }
        /// <summary>
        /// 批量完成
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UseCase(Description = "完成")]
        public virtual async Task<TBatchCompletionOutput> CompletionAsync(TBatchCompletionInput input)
        {
            await CheckCompletionPermissionAsync();
            var query = repository.GetAll().Where(c => input.Ids.Contains(c.Id));
            var list = await AsyncQueryableExecuter.ToListAsync(query);
            var r = new TBatchCompletionOutput();
            foreach (var item in list)
            {
                try
                {
                    if (item.EmployeeId != employeeSession.BusinessUserId)
                        throw new UserFriendlyException($"无权执行此操作！工单{item.Id + item.Title}不属于当前用户。");
                    var sw = UnitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.RequiresNew);
                    item.Completion(Clock.Now, input.StatusChangedDescription);
                    //await CurrentUnitOfWork.SaveChangesAsync();
                    await sw.CompleteAsync();
                    r.Ids.Add(item.Id);
                }
                catch (UserFriendlyException ex)
                {
                    r.ErrorMessage.Add(new BatchOperationErrorMessage(item.Id, ex.Message));
                }
                catch (Exception ex)
                {
                    r.ErrorMessage.Add(item.Id.Message500());
                    Logger.Warn(BXJGWorkOrderL("完成失败！"), ex);
                }
            }
            return r;
        }
        /// <summary>
        /// 批量拒绝
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UseCase(Description = "拒绝")]
        public virtual async Task<TBatchRejectOutput> RejectAsync(TBatchRejectInput input)
        {
            await CheckRejectPermissionAsync();
            var query = repository.GetAll().Where(c => input.Ids.Contains(c.Id));
            var list = await AsyncQueryableExecuter.ToListAsync(query);
            var r = new TBatchRejectOutput();
            foreach (var item in list)
            {
                try
                {
                    if (item.EmployeeId != employeeSession.BusinessUserId)
                        throw new UserFriendlyException($"无权执行此操作！工单{item.Id + item.Title}不属于当前用户。");
                    var sw = UnitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.RequiresNew);
                    item.Reject(Clock.Now, input.StatusChangedDescription);
                    //await CurrentUnitOfWork.SaveChangesAsync();
                    await sw.CompleteAsync();
                    r.Ids.Add(item.Id);
                }
                catch (UserFriendlyException ex)
                {
                    r.ErrorMessage.Add(new BatchOperationErrorMessage(item.Id, ex.Message));
                }
                catch (Exception ex)
                {
                    r.ErrorMessage.Add(item.Id.Message500());
                    Logger.Warn(BXJGWorkOrderL("拒绝失败！"), ex);
                }
            }
            return r;
        }
        /// <summary>
        /// 获取单个信息的查询，根据id查询，内部会join分类
        /// 通常不需要重写此方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual ValueTask<IQueryable<TQueryTemp>> GetFilterAsync(TGetInput input)
        {
            var q = GetQuery();
            q = q.Where(c => c.Order.Id == input.Id);
            //q = await GetAndAllFilterAsync(q);
            //var str = q.ToQueryString();
            return ValueTask.FromResult( q);
        }
        //protected virtual ValueTask<IQueryable<TQueryTemp>> GetAndAllFilterAsync(IQueryable<TQueryTemp> q, params Status[] statuses)
        //{
        //    Expression<Func<TQueryTemp, bool>> where = m => false;
        //    if (statuses != null && statuses.Length > 0)
        //    {
        //        if (statuses.Contains(Status.ToBeAllocated))
        //        {
        //            where = m => m.Order.Status == Status.ToBeAllocated && (m.Order.EmployeeId ==""|| m.Order.EmployeeId ==null);
        //        }
        //        var temp = statuses.Where(c => c != Status.ToBeAllocated);
        //        where = where.Or(c => c.Order.EmployeeId == employeeSession.BusinessUserId && statuses.Contains(c.Order.Status));
        //    }
        //    else
        //    {
        //        where = m => m.Order.Status == Status.ToBeAllocated && (m.Order.EmployeeId == "" || m.Order.EmployeeId == null);
        //        where = where.Or(c => c.Order.EmployeeId == employeeSession.BusinessUserId);
        //    }
        //    q = q.Where(where);
        //    return ValueTask.FromResult(q);
        //}

        /// <summary>
        /// 应用关键字模糊搜索，默认模糊搜索标题、描述
        /// 若你的工单类型某些字段需要参与关键字模糊搜索，则可以重写此方法，通常需要调用base.ApplyKeyword后，使用Or扩展方法应用更多模糊查询字段
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        protected virtual ValueTask<Expression<Func<TQueryTemp, bool>>> ApplyKeyword(string keyword)
        {
            Expression<Func<TQueryTemp, bool>> where = c => c.Order.Title.Contains(keyword) || c.Order.Description.Contains(keyword);
            return ValueTask.FromResult(where);
        }
        /// <summary>
        /// 获取数量或列表时都会调用，它主要用来应用过滤条件
        /// </summary>
        /// <param name="query"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual ValueTask<IQueryable<TQueryTemp>> ApplyOther(IQueryable<TQueryTemp> query, TGetTotalInput input)
        {
            #region 分类条件
            if (input.CategoryCodes != null)
            {
                Expression<Func<TQueryTemp, bool>> where = c => false;
                foreach (var item in input.CategoryCodes)
                {
                    where = where.Or(c => c.Category.Code.StartsWith(item));
                }
                query = query.Where(where);
            }
            #endregion

            #region 状态条件
            Expression<Func<TQueryTemp, bool>> where1 = m => false;
            if (input.Statuses != null && input.Statuses.Length > 0)
            {
                if (input.Statuses.Contains(Status.ToBeAllocated))
                {
                    where1 = m => m.Order.Status == Status.ToBeAllocated && (m.Order.EmployeeId == "" || m.Order.EmployeeId == null);
                }
                var temp = input.Statuses.Where(c => c != Status.ToBeAllocated);
                where1 = where1.Or(c => c.Order.EmployeeId == employeeSession.BusinessUserId && input.Statuses.Contains(c.Order.Status));
            }
            else
            {
                where1 = m => m.Order.Status == Status.ToBeAllocated && (m.Order.EmployeeId == "" || m.Order.EmployeeId == null);
                where1 = where1.Or(c => c.Order.EmployeeId == employeeSession.BusinessUserId);
            }
            query = query.Where(where1);
            #endregion

            #region 其它条件
            query = query.WhereIf(input.UrgencyDegrees != null, c => input.UrgencyDegrees.Contains(c.Order.UrgencyDegree))
                        //.WhereIf(input.Statuses != null, c => input.Statuses.Contains(c.Order.Status))
                        //.WhereIf(input.EmployeeIds != null && input.EmployeeType == EmpType.Contains, c => input.EmployeeIds.Contains(c.Order.EmployeeId))
                        //.WhereIf(input.EmployeeIds != null && input.EmployeeType == EmpType.Exclude, c => !input.EmployeeIds.Contains(c.Order.EmployeeId))
                        //.WhereIf(input.EmployeeType == EmpType.OnlyMe, c => c.Order.EmployeeId == CurrentEmployeeId)
                        .WhereIf(input.EstimatedExecutionTimeStart.HasValue, c => c.Order.EstimatedExecutionTime >= input.EstimatedExecutionTimeStart)
                        .WhereIf(input.EstimatedExecutionTimeEnd.HasValue, c => c.Order.EstimatedExecutionTime < input.EstimatedExecutionTimeEnd)
                        .WhereIf(input.EstimatedCompletionTimeStart.HasValue, c => c.Order.EstimatedCompletionTime >= input.EstimatedCompletionTimeStart)
                        .WhereIf(input.EstimatedCompletionTimeEnd.HasValue, c => c.Order.EstimatedCompletionTime < input.EstimatedCompletionTimeEnd)
                        .WhereIf(input.ExecutionTimeStart.HasValue, c => c.Order.ExecutionTime >= input.ExecutionTimeStart)
                        .WhereIf(input.ExecutionTimeEnd.HasValue, c => c.Order.ExecutionTime < input.ExecutionTimeEnd)
                        .WhereIf(input.CompletionTimeStart.HasValue, c => c.Order.CompletionTime >= input.CompletionTimeStart)
                        .WhereIf(input.CompletionTimeEnd.HasValue, c => c.Order.CompletionTime < input.CompletionTimeEnd);

            #endregion

            return ValueTask.FromResult(query);
        }
        //protected virtual Task<IEnumerable<string>> GetEmployeeIdsAsync(TGetTotalInput input)
        //{
        //    //if (!input.Keyword.IsNullOrWhiteSpace())
        //    //{
        //    //    return employeeAppService.GetIdsByKeywordAsync(input.Keyword);
        //    //}
        //    //return employeeAppService.GetIdsByKeywordAsync(input.Keyword);
        //    return null;
        //}

        protected virtual IQueryable<TEntity> GetOrderQuery()
        {
            return repository.GetAll();
        }
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

        protected virtual async Task<IQueryable<TQueryTemp>> GetAllFilterAsync(TGetTotalInput input)
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


            query = await ApplyOther(query, input);

            query = query.WhereIf(!input.Keyword.IsNullOrWhiteSpace(), await ApplyKeyword(input.Keyword));
            //query = await GetAndAllFilterAsync(query, input.Statuses);
            return query;
        }
        protected virtual IQueryable<TQueryTemp> PageBy(IQueryable<TQueryTemp> query, TGetAllInput input)
        {
            return query.PageBy(input);
        }
        protected virtual IQueryable<TQueryTemp> OrderBy(IQueryable<TQueryTemp> query, TGetAllInput input)
        {
            return query.OrderBy(input.Sorting);
        }
        protected virtual TDto EntityToDto(TQueryTemp temp, IDictionary<string, List<AttachmentEntity>> images, object state = null)
        {
            var dto = base.ObjectMapper.Map<TDto>(temp.Order);
            //dto.CategoryId = entity.CategoryId;
            //if (categories != null)
            //{
            //    dto.CategoryDisplayName = categories.SingleOrDefault(c => c.Id == entity.CategoryId)?.DisplayName;
            //}
            dto.CategoryDisplayName = temp.Category?.DisplayName;
            //dto.CompletionTime = entity.CompletionTime;
            //dto.CreationTime = entity.CreationTime;
            //dto.CreatorUserId = entity.CreatorUserId;
            //dto.DeleterUserId = entity.DeleterUserId;
            //dto.DeletionTime = entity.DeletionTime;
            //dto.Description = entity.Description;
            //dto.EmployeeId = entity.EmployeeId;
            //if (employees != null)
            //{
            //    var emp = employees.SingleOrDefault(c => c.Id == temp.Order.EmployeeId);
            //    dto.EmployeeName = emp?.Name;
            //    dto.EmployeePhone = emp?.Phone;
            //}
            //dto.EstimatedCompletionTime = entity.EstimatedCompletionTime;
            //dto.EstimatedExecutionTime = entity.EstimatedExecutionTime;
            //dto.ExecutionTime = entity.ExecutionTime;

            //dto.Id = entity.Id;
            //dto.IsDeleted = entity.IsDeleted;
            //dto.LastModificationTime = entity.LastModificationTime;
            //dto.LastModifierUserId = entity.LastModifierUserId;
            //dto.Status = entity.Status;
            //dto.StatusChangedDescription = entity.StatusChangedDescription;
            //dto.StatusChangedTime = entity.StatusChangedTime;
            //dto.Title = entity.Title;
            //dto.UrgencyDegree = entity.UrgencyDegree;
            if (images.ContainsKey(temp.Order.Id.ToString()))
                dto.Images = ObjectMapper.Map<List<AttachmentDto>>(images[temp.Order.Id.ToString()]);
            return dto;
        }
        protected virtual async Task<TDto> EntityToDto(TQueryTemp temp)
        {
            //var category = await categoryRepository.GetAsync(entity.CategoryId);
            //IEnumerable<EmployeeDto> emps = null;
            //if (!temp.Order.EmployeeId.IsNullOrWhiteSpace())
            //{
            //    emps = await employeeAppService.GetByIdsAsync(temp.Order.EmployeeId);
            //}
            var state = await GetStateAsync(new TQueryTemp[] { temp });
            var images = await attachmentManager.Value.GetAttachmentsAsync( entityIds: temp.Order.Id.ToString());

            return EntityToDto(temp, images, state);
        }
        protected virtual ValueTask<object> GetStateAsync(IEnumerable<TQueryTemp> entities)
        {
            return ValueTask.FromResult<object>(null);
        }
        #region 权限判断
        protected virtual Task CheckRejectPermissionAsync()
        {
            return CheckPermissionAsync(rejectPermissionName);
        }
        protected virtual Task CheckCompletionPermissionAsync()
        {
            return CheckPermissionAsync(completionPermissionName);
        }
        protected virtual Task CheckExecutePermissionAsync()
        {
            return CheckPermissionAsync(executePermissionName);
        }
        protected virtual Task CheckAllocatePermissionAsync()
        {
            return CheckPermissionAsync(allocatePermissionName);
        }
        protected virtual Task CheckGetPermissionAsync()
        {
            return CheckPermissionAsync(getPermissionName);
        }
        #endregion
    }

    /// <summary>
    /// 后台管理默认工单应用服务接口
    /// </summary>
    public class WorkOrderAppService : WorkOrderAppServiceBase<EntityDto<long>,
                                                               GetAllInputBase<GetTotalInputBase>,
                                                               GetTotalInputBase,
                                                               WorkOrderDto,
                                                               BatchAllocateInputBase,
                                                               BatchOperationOutputLong,
                                                               BatchChangeStatusInputBase,
                                                               BatchOperationOutputLong,
                                                               BatchChangeStatusInputBase,
                                                               BatchOperationOutputLong,
                                                               BatchChangeStatusInputBase,
                                                               BatchOperationOutputLong,
                                                               OrderEntity,
                                                               IRepository<OrderEntity, long>,
                                                               OrderManager, QueryTemp<OrderEntity>>

    {
        public WorkOrderAppService(IRepository<OrderEntity, long> repository,
                                   BXJGWorkOrderConfig cfg,
                                   IEmployeeSession empSession,
                                   Lazy<OrderManager> manager,
                                   Lazy<IRepository<CategoryEntity, long>> categoryRepository,
                                   Lazy<AttachmentManager<OrderEntity>> attachmentManager,
                                   WorkOrderTypeManager workOrderTypeManager,
                                   Lazy<CategoryManager> clsManager) : base(repository,
                                                                      empSession,
                                                                      manager,
                                                                      categoryRepository,
                                                                      attachmentManager,
                                                                      clsManager,
                                                                      workOrderTypeManager,
                                                                      CoreConsts.DefaultWorkOrderTypeName)
        {
            if (!cfg.EnableDefaultWorkOrder)
                throw new ApplicationException("BXJGWorkOrderConfig.EnableDefaultWorkOrder=false");
        }
    }


}
