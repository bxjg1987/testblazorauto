using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Timing;
using BXJG.Common.Dto;
using BXJG.GeneralTree;
using BXJG.WorkOrder.Employee;
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
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using Abp.Application.Services;
using BXJG.Utils.File;
using BXJG.WorkOrder.WorkOrder;
using BXJG.WorkOrder.EmployeeApplication.Session;
using System.Linq.Dynamic;
using Abp.Linq.Expressions;
using System.Linq.Expressions;
using BXJG.WorkOrder.WorkOrderType;

namespace BXJG.WorkOrder.EmployeeApplication.WorkOrder
{
    /// <summary>
    /// 工单处理人针对工单的相关操作接口，不同类型的工单应提供不同的子类实现
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
    public abstract class EmployeeWorkOrderAppServiceBase<TGetInput,
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
                                                          TManager> : EmployeeAppServiceBase
        #region 泛型约束
        where TGetInput : EntityDto<long>
        where TGetAllInput : GetAllInputBase
        where TGetTotalInput : GetTotalInputBase
        where TDto : WorkOrderDtoBase, new()
        where TBatchAllocateInput : BatchAllocateInputBase
        where TBatchAllocateOutput : BatchAllocateOutputBase, new()
        where TBatchExcuteInput : BatchExecuteInputBase
        where TBatchExcuteOutput : BatchExecuteOutputBase, new()
        where TBatchCompletionInput : BatchCompletionInputBase
        where TBatchCompletionOutput : BatchCompletionOutputBase, new()
        where TBatchRejectInput : BatchRejectInputBase
        where TBatchRejectOutput : BatchRejectOutputBase, new()
        where TEntity : OrderBaseEntity
        where TRepository : IRepository<TEntity, long>
        where TManager : OrderBaseManager<TEntity>
        #endregion
    {
        #region 字段和属性
        protected readonly TRepository repository;
        protected readonly IRepository<CategoryEntity, long> categoryRepository;
        protected readonly TManager manager;
        protected readonly IEmployeeAppService employeeAppService;
        protected readonly CategoryManager clsManager;
        protected readonly AttachmentManager<TEntity> attachmentManager;
        protected readonly WorkOrderTypeDefine workOrderTypeDefine;
        #endregion

        #region 构造函数
        /// <summary>
        /// 工单处理人员端应用服务基类构造函数
        /// </summary>
        /// <param name="repository">工单仓储</param>
        /// <param name="empSession">处理人员session</param>
        /// <param name="manager">工单领域服务</param>
        /// <param name="categoryRepository">工单类别仓储</param>
        /// <param name="employeeAppService">处理人员服务</param>
        /// <param name="clsManager">工单类领域服务</param>
        /// <param name="workOrderType">工单类型</param>
        /// <param name="attachmentManager">附件管理器</param>
        /// <param name="workOrderTypeManager">工单类型定义</param>
        public EmployeeWorkOrderAppServiceBase(TRepository repository,
                                       IEmployeeSession empSession,
                                       TManager manager,
                                       IRepository<CategoryEntity, long> categoryRepository,
                                       AttachmentManager<TEntity> attachmentManager,
                                       IEmployeeAppService employeeAppService,
                                       CategoryManager clsManager,
                                       WorkOrderTypeManager workOrderTypeManager,
                                       string workOrderType) : base(empSession)
        {
            this.repository = repository;
            this.manager = manager;
            this.categoryRepository = categoryRepository;
            this.employeeAppService = employeeAppService;
            this.clsManager = clsManager;
            this.attachmentManager = attachmentManager;
            this.workOrderTypeDefine = workOrderTypeManager[workOrderType];
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
            await CheckPermissionAsync();
            var query = await GetAllFilterAsync(input);
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
            await CheckPermissionAsync();
            var q = await GetFilterAsync(input);
            var entity = await AsyncQueryableExecuter.FirstOrDefaultAsync(q);
            return await EntityToDto(entity);
        }
        protected virtual async ValueTask<IQueryable<TEntity>> GetFilterAsync(TGetInput input)
        {
            var q = repository.GetAll();
            q = await GetAndAllFilterAsync(q);
            q = q.Where(c => c.Id == input.Id);
            return q;
        }
        protected virtual ValueTask<IQueryable<TEntity>> GetAndAllFilterAsync(IQueryable<TEntity> q)
        {
            return ValueTask.FromResult(q);
        }
        /// <summary>
        /// 获取指定所有工单的条件
        /// </summary>
        /// <returns></returns>

        private class sdf
        {
            public TEntity Order { get; set; }
            public CategoryEntity Cls { get; set; }
        }
        protected virtual async Task<IQueryable<TEntity>> GetAllFilterAsync(TGetTotalInput input)
        {
            var query1 = from c in repository.GetAll().AsNoTrackingWithIdentityResolution()
                         join lb in categoryRepository.GetAll() on c.CategoryId equals lb.Id into g
                         from kk in g.DefaultIfEmpty()
                         select new sdf { Order = c, Cls = kk };

            if (input.CategoryCodes != null)
            {
                Expression<Func<sdf, bool>> where = c => false;
                foreach (var item in input.CategoryCodes)
                {
                    where = where.Or(c => c.Cls.Code.StartsWith(item));
                }
                query1 = query1.Where(where);
            }
            var query = query1.Select(c => c.Order);

            if (!input.Keyword.IsNullOrWhiteSpace())
            {
                var empIdsQuery = await employeeAppService.GetIdsByKeywordAsync(input.Keyword);
                query = query.Where(c => empIdsQuery.Contains(c.EmployeeId));
            }
            query = query.WhereIf(input.UrgencyDegrees != null, c => input.UrgencyDegrees.Contains(c.UrgencyDegree))
                         .WhereIf(input.Statuses != null, c => input.Statuses.Contains(c.Status))
                         .WhereIf(input.EmployeeIds != null && input.EmployeeType == EmpType.Contains, c => input.EmployeeIds.Contains(c.EmployeeId))
                         .WhereIf(input.EmployeeIds != null && input.EmployeeType == EmpType.Exclude, c => !input.EmployeeIds.Contains(c.EmployeeId))
                         .WhereIf(input.EmployeeType == EmpType.OnlyMe, c => c.EmployeeId == CurrentEmployeeId)
                         .WhereIf(input.EstimatedExecutionTimeStart.HasValue, c => c.EstimatedExecutionTime >= input.EstimatedExecutionTimeStart)
                         .WhereIf(input.EstimatedExecutionTimeEnd.HasValue, c => c.EstimatedExecutionTime < input.EstimatedExecutionTimeEnd)
                         .WhereIf(input.EstimatedCompletionTimeStart.HasValue, c => c.EstimatedCompletionTime >= input.EstimatedCompletionTimeStart)
                         .WhereIf(input.EstimatedCompletionTimeEnd.HasValue, c => c.EstimatedCompletionTime < input.EstimatedCompletionTimeEnd)
                         .WhereIf(input.ExecutionTimeStart.HasValue, c => c.ExecutionTime >= input.ExecutionTimeStart)
                         .WhereIf(input.ExecutionTimeEnd.HasValue, c => c.ExecutionTime < input.ExecutionTimeEnd)
                         .WhereIf(input.CompletionTimeStart.HasValue, c => c.CompletionTime >= input.CompletionTimeStart)
                         .WhereIf(input.CompletionTimeEnd.HasValue, c => c.CompletionTime < input.CompletionTimeEnd)
                         .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), c => c.Title.Contains(input.Keyword) || c.Description.Contains(input.Keyword));

            query = await GetAndAllFilterAsync(query);

            return query;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(false)]
        public virtual async Task<PagedResultDto<TDto>> GetAll1Async(TGetAllInput input)
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

            await CheckPermissionAsync();
            var query = await GetAllFilterAsync(input as TGetTotalInput);
            var count = await AsyncQueryableExecuter.CountAsync(query);
            query = OrderBy(query, input);
            query = PageBy(query, input);
            var list = await AsyncQueryableExecuter.ToListAsync(query);

            var cIds = list.Select(c => c.CategoryId);
            var cQuery = categoryRepository.GetAll().Where(c => cIds.Contains(c.Id));
            var cls = await AsyncQueryableExecuter.ToListAsync(cQuery);

            var empIds = list.Where(c => !c.EmployeeId.IsNullOrWhiteSpace()).Select(c => c.EmployeeId);

            IEnumerable<EmployeeDto> emps = null;
            if (empIds != null && empIds.Count() > 0)
            {
                emps = await employeeAppService.GetByIdsAsync(empIds.ToArray());
            }

            var images = await attachmentManager.GetFirstAttachmentsAsync(list.Select(c => c.Id.ToString()).ToArray());
            var images2 = images.ToDictionary(c => c.Key, c => new List<AttachmentEntity> { c.Value });

            var state = await GetStateAsync(list);
            var items = new List<TDto>();
            foreach (var item in list)
            {
                var ttt = EntityToDto(item, cls, emps, images2, state);
                items.Add(ttt);
            }
            return new PagedResultDto<TDto>(count, items);
        }
        ///// <summary>
        ///// 批量调整工单状态
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //public virtual async Task<TBatchExcuteOutput> ChangeStatusAsync(TBatchExcuteInput input)
        //{
        //    //await CheckConfirmePermissionAsync();
        //    var query = repository.GetAll().Where(c => input.Ids.Contains(c.Id));
        //    var list = await AsyncQueryableExecuter.ToListAsync(query);
        //    var r = new TBatchExcuteOutput();
        //    foreach (var item in list)
        //    {
        //        try
        //        {
        //            await item.ChangeStatus(input.Status,
        //                                    input.StatusChangedTime ?? Clock.Now,
        //                                    input.Description,
        //                                    item.EmployeeId,
        //                                    item.EstimatedExecutionTime,
        //                                    item.EstimatedCompletionTime,
        //                                    item.ExecutionTime,
        //                                    item.CompletionTime,
        //                                    d => CheckToBeonfirmedPermissionAsync(),
        //                                    d => CheckConfirmePermissionAsync(),
        //                                    d => CheckAllocatePermissionAsync(),
        //                                    d => CheckExecutePermissionAsync(),
        //                                    d => CheckConfirmePermissionAsync(),
        //                                    d => CheckRejectPermissionAsync());
        //            await CurrentUnitOfWork.SaveChangesAsync();
        //            r.Ids.Add(item.Id);
        //        }
        //        catch (UserFriendlyException ex)
        //        {
        //            r.ErrorMessage.Add(new BatchOperationErrorMessage(item.Id, ex.Message));
        //        }
        //        catch (Exception ex)
        //        {
        //            r.ErrorMessage.Add(item.Id.Message500());
        //            Logger.Warn(L("执行失败！"), ex);
        //        }
        //    }
        //    return r;
        //}
        ///// <summary>
        ///// 批量分配工单
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //public virtual async Task<TBatchAllocateOutput> AllocateAsync(TBatchAllocateInput input)
        //{
        //    //await CheckAllocatePermissionAsync();
        //    var query = repository.GetAll().Where(c => input.Ids.Contains(c.Id));
        //    var list = await AsyncQueryableExecuter.ToListAsync(query);
        //    var r = new TBatchAllocateOutput();
        //    foreach (var item in list)
        //    {
        //        try
        //        {
        //            if (item.Status >= Status.ToBeProcessed)
        //            {
        //                await item.BackOff(input.StatusChangedTime ?? Clock.Now,
        //                                   status: Status.ToBeAllocated,
        //                                   toBeConfirmed: d => CheckToBeonfirmedPermissionAsync(),
        //                                   toBeAllocated: d => CheckConfirmePermissionAsync(),
        //                                   toBeProcessed: d => CheckAllocatePermissionAsync(),
        //                                   processing: d => CheckExecutePermissionAsync());
        //            }
        //            //item.AllocateRetain(Clock.Now, input.EmployeeId, input.EstimatedExecutionTime, input.EstimatedCompletionTime);
        //            await item.Skip(input.StatusChangedTime ?? Clock.Now,
        //                            status: Status.ToBeProcessed,
        //                            empId: input.EmployeeId,
        //                            estimatedExecutionTime: input.EstimatedExecutionTime,
        //                            estimatedCompletionTime: input.EstimatedCompletionTime,
        //                            toBeAllocated: d => CheckConfirmePermissionAsync(),
        //                            toBeProcessed: d => CheckAllocatePermissionAsync());

        //            await CurrentUnitOfWork.SaveChangesAsync();
        //            r.Ids.Add(item.Id);
        //        }
        //        catch (UserFriendlyException ex)
        //        {
        //            r.ErrorMessage.Add(new BatchOperationErrorMessage(item.Id, ex.Message));
        //        }
        //        catch (Exception ex)
        //        {
        //            r.ErrorMessage.Add(item.Id.Message500());
        //            Logger.Warn(L("分配工单失败！"), ex);
        //        }
        //    }
        //    return r;
        //}
        /// <summary>
        /// GetAll的分页
        /// </summary>
        /// <param name="query"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> PageBy(IQueryable<TEntity> query, TGetAllInput input)
        {
            return query.PageBy(input);
        }
        /// <summary>
        /// GetAll的排序
        /// </summary>
        /// <param name="query"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> OrderBy(IQueryable<TEntity> query, TGetAllInput input)
        {
            return query.OrderBy(input.Sorting);
        }
        /// <summary>
        /// 实体映射到dto
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="categories"></param>
        /// <param name="employees"></param>
        /// <param name="images"></param>
        /// <param name="state">子类可能需要聚合更多外键</param>
        /// <returns></returns>
        protected virtual TDto EntityToDto(TEntity entity, IEnumerable<CategoryEntity> categories, IEnumerable<EmployeeDto> employees, IDictionary<string, List<AttachmentEntity>> images, object state = null)
        {
            var dto = base.ObjectMapper.Map<TDto>(entity);
            //dto.CategoryId = entity.CategoryId;
            if (categories != null)
            {
                dto.CategoryDisplayName = categories.SingleOrDefault(c => c.Id == entity.CategoryId)?.DisplayName;
            }
            //dto.CompletionTime = entity.CompletionTime;
            //dto.CreationTime = entity.CreationTime;
            //dto.CreatorUserId = entity.CreatorUserId;
            //dto.DeleterUserId = entity.DeleterUserId;
            //dto.DeletionTime = entity.DeletionTime;
            //dto.Description = entity.Description;
            //dto.EmployeeId = entity.EmployeeId;
            if (employees != null)
            {
                var emp = employees.SingleOrDefault(c => c.Id == entity.EmployeeId);
                dto.EmployeeName = emp?.Name;
                dto.EmployeePhone = emp?.Phone;
            }
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
            if (images.ContainsKey(entity.Id.ToString()))
                dto.Images = ObjectMapper.Map<List<AttachmentDto>>(images[entity.Id.ToString()]);
            return dto;
        }
        /// <summary>
        /// 将单个实体映射为dto
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual async Task<TDto> EntityToDto(TEntity entity)
        {
            var category = await categoryRepository.GetAsync(entity.CategoryId);
            IEnumerable<EmployeeDto> emps = null;
            if (!entity.EmployeeId.IsNullOrWhiteSpace())
            {
                emps = await employeeAppService.GetByIdsAsync(entity.EmployeeId);
            }
            var state = await GetStateAsync(new TEntity[] { entity });
            var images = await attachmentManager.GetAttachmentsAsync(entity.Id.ToString());

            return EntityToDto(entity, new CategoryEntity[] { category }, emps, images, state);
        }
        /// <summary>
        /// 子类可能需要聚合更多外键
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        protected virtual ValueTask<object> GetStateAsync(IEnumerable<TEntity> entities)
        {
            return ValueTask.FromResult<object>(null);
        }

        protected virtual Task CheckPermissionAsync()
        {
            return base.CheckPermissionAsync(CoreConsts.EmployeeWorkOrderManager);
        }
    }

    /// <summary>
    /// 后台管理默认工单应用服务接口
    /// </summary>
    public class EmployeeWorkOrderAppService : EmployeeWorkOrderAppServiceBase<EntityDto<long>,
                                                                               GetAllEmployeeWorkOrderInput,
                                                                               GetEmployeeWorkOrderTotalInput,
                                                                               WorkOrderDto,
                                                                               BatchAllocateInputBase,
                                                                               BatchAllocateOutputBase,
                                                                               BatchExecuteInputBase,
                                                                               BatchExecuteOutputBase,
                                                                               BatchCompletionInputBase,
                                                                               BatchCompletionOutputBase,
                                                                               BatchRejectInputBase,
                                                                               BatchRejectOutputBase,
                                                                               OrderEntity,
                                                                               IRepository<OrderEntity, long>,
                                                                               OrderManager>

    {
        public EmployeeWorkOrderAppService(IRepository<OrderEntity, long> repository,
                                   BXJGWorkOrderConfig cfg,
                                   IEmployeeSession empSession,
                                   OrderManager manager,
                                   IRepository<CategoryEntity, long> categoryRepository,
                                   AttachmentManager<OrderEntity> attachmentManager,
                                   IEmployeeAppService employeeAppService,
                                   WorkOrderTypeManager workOrderTypeManager,
                                   CategoryManager clsManager) : base(repository,
                                                                      empSession,
                                                                      manager,
                                                                      categoryRepository,
                                                                      attachmentManager,
                                                                      employeeAppService,
                                                                      clsManager,
                                                                      workOrderTypeManager,
                                                                      CoreConsts.DefaultWorkOrderTypeName)
        {
            if (!cfg.EnableDefaultWorkOrder)
                throw new ApplicationException("BXJGWorkOrderConfig.EnableDefaultWorkOrder=false");
        }
    }
}
