using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Timing;
using BXJG.Common.Dto;
using BXJG.GeneralTree;
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
using System.Linq.Expressions;
using Abp.Linq.Expressions;
using Abp.EntityHistory;

namespace BXJG.WorkOrder.WorkOrder
{
    /// <summary>
    /// 工单后台管理应用服务基类
    /// </summary>
    /// <typeparam name="TCreateInput">新增时的输入模型</typeparam>
    /// <typeparam name="TUpdateInput">修改时的输入模型</typeparam>
    /// <typeparam name="TBatchDeleteInput">批量删除的输入模型</typeparam>
    /// <typeparam name="TBatchDeleteOutput">批量删除时的输出模型</typeparam>
    /// <typeparam name="TGetInput">获取单个信息的输入模型</typeparam>
    /// <typeparam name="TGetAllInput">列表页查询时的输入模型</typeparam>
    /// <typeparam name="TGetTotalInput"></typeparam>
    /// <typeparam name="TEntityDto">列表页显示模型</typeparam>
    /// <typeparam name="TBatchChangeStatusInput">批量状态修改时的输入模型</typeparam>
    /// <typeparam name="TBatchChangeStatusOutput">批量状态修改时的输出模型</typeparam>
    /// <typeparam name="TBatchAllocateInput">批量分配时的输入模型</typeparam>
    /// <typeparam name="TBatchAllocateOutput">批量分配时的输出模型</typeparam>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TRepository">实体仓储类型</typeparam>
    /// <typeparam name="TCreateDto"></typeparam>
    /// <typeparam name="TManager">领域服务类型</typeparam>
    /// <typeparam name="TCategoryRepository">分类仓储</typeparam>
    /// <typeparam name="TQueryTemp">分类仓储</typeparam>
    public abstract class WorkOrderAppServiceBase<TCreateInput,
                                                  TUpdateInput,
                                                  TBatchDeleteInput,
                                                  TBatchDeleteOutput,
                                                  TGetInput,
                                                  TGetTotalInput,
                                                  TGetAllInput,
                                                  TEntityDto,
                                                  TBatchChangeStatusInput,
                                                  TBatchChangeStatusOutput,
                                                  TBatchConfirmeInput,
                                                  TBatchConfirmeOutput,
                                                  TBatchAllocateInput,
                                                  TBatchAllocateOutput,
                                                  TEntity,
                                                  TRepository,
                                                  TCreateDto,
                                                  TManager,
                                                  TCategoryRepository,
                                                  TQueryTemp> : AppServiceBase
        #region MyRegion
        where TCreateInput : CreateInputBase
        where TUpdateInput : UpdateInputBase
        where TBatchDeleteInput : BatchOperationInputLong
        where TBatchDeleteOutput : BatchOperationOutputLong, new()
        where TGetInput : EntityDto<long>
        where TGetTotalInput : GetTotalInputBase, new()
        where TGetAllInput : GetAllInputBase<TGetTotalInput>
        where TEntityDto : DtoBase, new()
        where TBatchChangeStatusInput : BatchChangeStatusInputBase
        where TBatchChangeStatusOutput : BatchOperationOutputLong, new()
        where TBatchConfirmeInput : BatchConfirmeBaseInput
        where TBatchConfirmeOutput : BatchOperationOutputLong, new()
        where TBatchAllocateInput : BatchAllocateInputBase
        where TBatchAllocateOutput : BatchOperationOutputLong, new()
        where TEntity : OrderBaseEntity
        where TRepository : IRepository<TEntity, long>
        where TCreateDto : WorkOrderCreateDtoBase, new()
        where TManager : OrderBaseManager<TEntity>
        where TCategoryRepository : IRepository<CategoryEntity, long>
        where TQueryTemp : QueryTemp<TEntity>, new()
        #endregion
    {
        #region MyRegion
        protected readonly TRepository repository;
        protected readonly Lazy<TCategoryRepository> categoryRepository;
        protected readonly Lazy<TManager> manager;
        protected readonly Lazy<IRepository<CategoryEntity, long>> clsRepository;
        protected readonly Lazy<CategoryManager> clsManager;
        protected readonly string workOrderType;
        protected readonly Lazy<AttachmentManager<TEntity>> attachmentManager;
        protected readonly string createPermissionName, updatePermissionName, deletePermissionName, getPermissionName, toBeConfirmedPermissionName, confirmePermissionName, allocatePermissionName, executePermissionName, completionPermissionName, rejectPermissionName;
        #endregion

        #region MyRegion
        /// <summary>
        /// 工单后台管理应用服务基类构造函数
        /// </summary>
        /// <param name="repository">工单仓储</param>
        /// <param name="manager">工单领域服务</param>
        /// <param name="categoryRepository">工单类别仓储</param>
        /// <param name="clsRepository">工单类别仓储</param>
        /// <param name="clsManager">工单类领域服务</param>
        /// <param name="workOrderType">工单类型</param>
        /// <param name="attachmentManager">附件管理器</param>
        /// <param name="createPermissionName">新增权限名称</param>
        /// <param name="updatePermissionName">修改权限名称</param>
        /// <param name="deletePermissionName">删除权限名称</param>
        /// <param name="getPermissionName">获取权限名称</param>
        /// <param name="toBeConfirmedPermissionName">待确认权限名称</param>
        /// <param name="confirmePermissionName">确认权限名称</param>
        /// <param name="allocatePermissionName">分配权限名称</param>
        /// <param name="executePermissionName">执行权限名称</param>
        /// <param name="completionPermissionName">完成权限名称</param>
        /// <param name="rejectPermissionName">拒绝权限名称</param>
        public WorkOrderAppServiceBase(TRepository repository,
                                       Lazy<TManager> manager,
                                       Lazy<TCategoryRepository> categoryRepository,
                                       Lazy<AttachmentManager<TEntity>> attachmentManager,
                                       Lazy<IRepository<CategoryEntity, long>> clsRepository,
                                       Lazy<CategoryManager> clsManager,
                                       string workOrderType,
                                       string createPermissionName = default,
                                       string updatePermissionName = default,
                                       string deletePermissionName = default,
                                       string getPermissionName = default,
                                       string toBeConfirmedPermissionName = default,
                                       string confirmePermissionName = default,
                                       string allocatePermissionName = default,
                                       string executePermissionName = default,
                                       string completionPermissionName = default,
                                       string rejectPermissionName = default)
        {
            this.repository = repository;
            this.manager = manager;
            this.categoryRepository = categoryRepository;
            this.getPermissionName = getPermissionName;
            this.allocatePermissionName = allocatePermissionName;
            this.executePermissionName = executePermissionName;
            this.completionPermissionName = completionPermissionName;
            this.rejectPermissionName = rejectPermissionName;
            this.createPermissionName = createPermissionName;
            this.updatePermissionName = updatePermissionName;
            this.deletePermissionName = deletePermissionName;
            this.confirmePermissionName = confirmePermissionName;
            this.toBeConfirmedPermissionName = toBeConfirmedPermissionName;
            this.clsRepository = clsRepository;
            this.clsManager = clsManager;
            this.workOrderType = workOrderType;
            this.attachmentManager = attachmentManager;
        }
        #endregion

        #region MyRegion
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UseCase(Description = "新增")]
        public virtual async Task<TEntityDto> CreateAsync(TCreateInput input)
        {
            await CheckCreatePermissionAsync();
            var entity = await manager.Value.CreateAsync(await CreateInputToCreateDto(input));
            //entity.Points = input.Points;
            await BeforeCreateAsync(entity, input);
            await CurrentUnitOfWork.SaveChangesAsync();
            var cls = await clsRepository.Value.GetAsync(entity.CategoryId);
            TEntityDto r = default;
            await attachmentManager.Value.SetAttachmentsAsync(entity.Id, input.Images, async c => r = await EntityToDto(new TQueryTemp { Order = entity, Category = cls }));
            return r;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UseCase(Description = "修改")]
        public virtual async Task<TEntityDto> UpdateAsync(TUpdateInput input)
        {
            await CheckUpdatePermissionAsync();
            var entity = await repository.GetAsync(input.Id);
            await BeforeEditAsync(entity, input);
            await CurrentUnitOfWork.SaveChangesAsync();
            var cls = await clsRepository.Value.GetAsync(entity.CategoryId);
            TEntityDto r = default;
            await attachmentManager.Value.SetAttachmentsAsync(entity.Id, input.Images, async c => r = await EntityToDto(new TQueryTemp { Order = entity, Category = cls }));
            return r;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UseCase(Description = "删除")]
        public virtual async Task<TBatchDeleteOutput> DeleteAsync(TBatchDeleteInput input)
        {
            await CheckDeletePermissionAsync();
            var query = repository.GetAll().Where(c => input.Ids.Contains(c.Id));
            var list = await AsyncQueryableExecuter.ToListAsync(query);
            var r = new TBatchDeleteOutput();
            foreach (var item in list)
            {
                try
                {
                    await manager.Value.DeleteAsync(item);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    await attachmentManager.Value.SetAttachmentsAsync(item.Id, null);
                    r.Ids.Add(item.Id);
                }
                catch (UserFriendlyException ex)
                {
                    r.ErrorMessage.Add(new BatchOperationErrorMessage(item.Id, ex.Message));
                }
                catch (Exception ex)
                {
                    r.ErrorMessage.Add(item.Id.Message500());
                    Logger.Warn("删除工单失败！".BXJGWorkOrderL(), ex);
                }
            }
            return r;
        }
        /// <summary>
        /// 获取指定工单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(false)]
        public virtual async Task<TEntityDto> GetAsync(TGetInput input)
        {
            await CheckGetPermissionAsync();
            var q = await GetFilterAsync(input);
            var entity = await AsyncQueryableExecuter.FirstOrDefaultAsync(q);
            return await EntityToDto(entity);
        }
        /// <summary>
        /// 获取工单列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(false)]
        public virtual async Task<PagedResultDto<TEntityDto>> GetAllAsync(TGetAllInput input)
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
            var list = await AsyncQueryableExecuter.ToListAsync(query);



            var images = await attachmentManager.Value.GetAttachmentsAsync(entityIds: list.Select(c => c.Order.Id.ToString()).ToArray());

            var state = await GetStateAsync(list);
            var items = new List<TEntityDto>();
            foreach (var item in list)
            {
                var ttt = EntityToDto(item, images, state);
                items.Add(ttt);
            }
            return new PagedResultDto<TEntityDto>(count, items);
        }
        /// <summary>
        /// 批量调整工单状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UseCase(Description = "调整状态")]
        public virtual async Task<TBatchChangeStatusOutput> ChangeStatusAsync(TBatchChangeStatusInput input)
        {
            //await CheckConfirmePermissionAsync();
            var query = repository.GetAll().Where(c => input.Ids.Contains(c.Id));
            var list = await AsyncQueryableExecuter.ToListAsync(query);
            var r = new TBatchChangeStatusOutput();
            foreach (var item in list)
            {
                try
                {
                    await item.ChangeStatus(input.Status,
                                            input.StatusChangedTime ?? Clock.Now,
                                            input.Description,
                                            item.EmployeeId,
                                            item.EstimatedExecutionTime,
                                            item.EstimatedCompletionTime,
                                            item.ExecutionTime,
                                            item.CompletionTime,
                                            d => CheckToBeonfirmedPermissionAsync(),
                                            d => CheckConfirmePermissionAsync(),
                                            d => CheckAllocatePermissionAsync(),
                                            d => CheckExecutePermissionAsync(),
                                            d => CheckConfirmePermissionAsync(),
                                            d => CheckRejectPermissionAsync());
                    await CurrentUnitOfWork.SaveChangesAsync();
                    r.Ids.Add(item.Id);
                }
                catch (UserFriendlyException ex)
                {
                    r.ErrorMessage.Add(new BatchOperationErrorMessage(item.Id, ex.Message));
                }
                catch (Exception ex)
                {
                    r.ErrorMessage.Add(item.Id.Message500());
                    Logger.Warn("执行失败！".BXJGWorkOrderL(), ex);
                }
            }
            return r;
        }

        [UseCase(Description = "确认")]
        public virtual ValueTask ConfirmeAsync(TEntity entity, DateTimeOffset? dateTimeOffset = default, string desc = "确认", params object[] ps)
        {
            //业务判断
            entity.Confirme(dateTimeOffset ?? Clock.Now, desc);
            return ValueTask.CompletedTask;
            //后续处理
        }

        /// <summary>
        /// 批量分配工单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UseCase(Description = "分配")]
        public virtual async Task<TBatchAllocateOutput> AllocateAsync(TBatchAllocateInput input)
        {
            //await CheckAllocatePermissionAsync();
            var query = repository.GetAll().Where(c => input.Ids.Contains(c.Id));
            var list = await AsyncQueryableExecuter.ToListAsync(query);
            var r = new TBatchAllocateOutput();
            foreach (var item in list)
            {
                try
                {
                    if (item.Status >= Status.ToBeProcessed)
                    {
                        await item.BackOff(input.StatusChangedTime ?? Clock.Now,
                                           status: Status.ToBeAllocated,
                                           toBeConfirmed: d => CheckToBeonfirmedPermissionAsync(),
                                           toBeAllocated: d => CheckConfirmePermissionAsync(),
                                           toBeProcessed: d => CheckAllocatePermissionAsync(),
                                           processing: d => CheckExecutePermissionAsync());
                    }
                    //item.AllocateRetain(Clock.Now, input.EmployeeId, input.EstimatedExecutionTime, input.EstimatedCompletionTime);
                    await item.Skip(input.StatusChangedTime ?? Clock.Now,
                                    status: Status.ToBeProcessed,
                                    empId: input.EmployeeId,
                                    estimatedExecutionTime: input.EstimatedExecutionTime,
                                    estimatedCompletionTime: input.EstimatedCompletionTime,
                                    toBeAllocated: d => CheckConfirmePermissionAsync(),
                                    toBeProcessed: d => CheckAllocatePermissionAsync());

                    await CurrentUnitOfWork.SaveChangesAsync();
                    r.Ids.Add(item.Id);
                }
                catch (UserFriendlyException ex)
                {
                    r.ErrorMessage.Add(new BatchOperationErrorMessage(item.Id, ex.Message));
                }
                catch (Exception ex)
                {
                    r.ErrorMessage.Add(item.Id.Message500());
                    Logger.Warn("分配工单失败！".BXJGWorkOrderL(), ex);
                }
            }
            return r;
        }
        #endregion

        #region MyRegion
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
            return ValueTask.FromResult(q);
        }
        //protected virtual ValueTask<IQueryable<TQueryTemp>> GetAndAllFilterAsync(IQueryable<TQueryTemp> q)
        //{
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
        /// keyword条件过滤比较特殊，若需要更多关键字模糊查询请重写<see cref="ApplyKeyword(string)"/>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual ValueTask<IQueryable<TQueryTemp>> ApplyOther(IQueryable<TQueryTemp> query, TGetTotalInput input)
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
        /// <summary>
        /// 获取工单和分类join后的查询对象
        /// 此方法会被<see cref="GetAllAsync(TGetAllInput)"/>和<see cref="GetAllAsync(TGetAllInput)"/>调用
        /// 可以重写此方法include更多导航属性
        /// </summary>
        /// <returns></returns>
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

            //应用基本条件过滤
            query = await ApplyOther(query, input);
            //关键字条件过滤比较特殊，定义在单独的方法中，允许重写
            query = query.WhereIf(!input.Keyword.IsNullOrWhiteSpace(), await ApplyKeyword(input.Keyword));
            //query = await GetAndAllFilterAsync(query);
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
        protected virtual TEntityDto EntityToDto(TQueryTemp entity, IDictionary<string, List<AttachmentEntity>> images, object state = null)
        {
            var dto = base.ObjectMapper.Map<TEntityDto>(entity.Order);
            //dto.CategoryId = entity.CategoryId;
            dto.CategoryDisplayName = entity.Category?.DisplayName;
            //dto.CompletionTime = entity.CompletionTime;
            //dto.CreationTime = entity.CreationTime;
            //dto.CreatorUserId = entity.CreatorUserId;
            //dto.DeleterUserId = entity.DeleterUserId;
            //dto.DeletionTime = entity.DeletionTime;
            //dto.Description = entity.Description;
            //dto.EmployeeId = entity.EmployeeId;
            //if (employees != null)
            //{
            //    var emp = employees.SingleOrDefault(c => c.Id == entity.EmployeeId);
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
            if (images.ContainsKey(entity.Order.Id.ToString()))
                dto.Images = ObjectMapper.Map<List<AttachmentDto>>(images[entity.Order.Id.ToString()]);
            return dto;
        }
        /// <summary>
        /// 将实体映射为dto
        /// 新增、修改、get时回调
        /// 它内处理附件后回调<see cref="EntityToDto(TQueryTemp, IDictionary{string, List{AttachmentEntity}}, object)"/>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual async Task<TEntityDto> EntityToDto(TQueryTemp entity)
        {
            //var category = await categoryRepository.GetAsync(entity.CategoryId);
            //IEnumerable<EmployeeDto> emps = null;
            //if (!entity.EmployeeId.IsNullOrWhiteSpace())
            //{
            //    emps = await employeeAppService.GetByIdsAsync(entity.EmployeeId);
            //}
            var state = await GetStateAsync(new TQueryTemp[] { entity });
            var images = await attachmentManager.Value.GetAttachmentsAsync( entityIds: entity.Order.Id.ToString());

            return EntityToDto(entity, images, state);
        }
        protected virtual ValueTask<TCreateDto> CreateInputToCreateDto(TCreateInput input)
        {
            var dto = new TCreateDto
            {
                CategoryId = input.CategoryId,
                Description = input.Description,
                EstimatedCompletionTime = input.EstimatedCompletionTime,
                EstimatedExecutionTime = input.EstimatedExecutionTime,
                Title = input.Title,
                EmployeeId = input.EmployeeId,
                UrgencyDegree = input.UrgencyDegree
                //Time = Clock.Now
            };
            if (!input.UrgencyDegree.HasValue)
                dto.UrgencyDegree = OrderBaseEntity.DefaultUrgencyDegree;
            return ValueTask.FromResult(dto);
        }
        protected virtual async ValueTask BeforeCreateAsync(TEntity entity, TCreateInput input)
        {
            if (input.Status.HasValue && input.Status > entity.Status)
            {
                await entity.Skip(Clock.Now,
                                  input.Status,
                                  input.StatusChangedDescription,
                                  input.EmployeeId,
                                  input.EstimatedExecutionTime,
                                  input.EstimatedCompletionTime,
                                  excuteTime: input.ExecutionTime,
                                  completeTime: input.CompletionTime,
                                  toBeAllocated: d => CheckConfirmePermissionAsync(),
                                  toBeProcessed: d => CheckAllocatePermissionAsync(),
                                  processing: d => CheckExecutePermissionAsync(),
                                  completed: d => CheckCompletionPermissionAsync(),
                                  rejected: d => CheckRejectPermissionAsync());
            }
        }

        protected virtual ValueTask BeforeEditAsync(TEntity entity, TUpdateInput input)
        {
            if (input.CategoryId.HasValue)
                entity.CategoryId = input.CategoryId.Value;
            entity.Title = input.Title;
            entity.Description = input.Description;
            entity.StatusChangedDescription = input.StatusChangedDescription;
            entity.UrgencyDegree = input.UrgencyDegree ?? OrderBaseEntity.DefaultUrgencyDegree;
            entity.EmployeeId = input.EmployeeId;
            //entity.Points = input.Points;
            entity.ChangeEstimatedTime(input.EstimatedExecutionTime, input.EstimatedCompletionTime);
            return ValueTask.CompletedTask;
        }

        /// <summary>
        /// 获取列表，在遍历生成dto集合前调用此方法
        /// 在遍历生成dto列表过程中会传递给<see cref="EntityToDto(TQueryTemp, IDictionary{string, List{AttachmentEntity}}, object)"/>
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        protected virtual ValueTask<object> GetStateAsync(IEnumerable<TQueryTemp> entities)
        {
            return ValueTask.FromResult<object>(null);
        }
        #endregion

        #region MyRegion
        protected virtual Task CheckCreatePermissionAsync()
        {
            return CheckPermissionAsync(createPermissionName);
        }
        protected virtual Task CheckUpdatePermissionAsync()
        {
            return CheckPermissionAsync(updatePermissionName);
        }
        protected virtual Task CheckDeletePermissionAsync()
        {
            return CheckPermissionAsync(deletePermissionName);
        }
        protected virtual Task CheckRejectPermissionAsync()
        {
            return CheckPermissionAsync(rejectPermissionName);
        }
        protected virtual Task CheckConfirmePermissionAsync()
        {
            return CheckPermissionAsync(confirmePermissionName);
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
        protected virtual Task CheckToBeonfirmedPermissionAsync()
        {
            return CheckPermissionAsync(toBeConfirmedPermissionName);
        }
        #endregion
    }

    ///// <summary>
    ///// 后台管理默认工单应用服务接口
    ///// </summary>
    //public class WorkOrderAppService : WorkOrderAppServiceBase<WorkOrderCreateInput,
    //                                                           WorkOrderUpdateInput,
    //                                                           BatchOperationInputLong,
    //                                                           BatchOperationOutputLong,
    //                                                           EntityDto<long>,
    //                                                           GetAllWorkOrderInput,
    //                                                           WorkOrderDto,
    //                                                           BatchChangeStatusInputBase,
    //                                                           WorkOrderBatchChangeStatusOutputBase,
    //                                                           BatchAllocateInputBase,
    //                                                           WorkOrderBatchAllocateOutputBase,
    //                                                           OrderEntity,
    //                                                           IRepository<OrderEntity, long>,
    //                                                           WorkOrderCreateDto,
    //                                                           OrderManager,
    //                                                           IRepository<CategoryEntity, long>>

    //{
    //    public WorkOrderAppService(IRepository<OrderEntity, long> repository,
    //                               OrderManager manager,
    //                               BXJGWorkOrderConfig cfg,
    //                               IRepository<CategoryEntity, long> clsRepository,
    //                               CategoryManager clsManager,
    //                               IRepository<CategoryEntity, long> categoryRepository,
    //                               AttachmentManager<OrderEntity> attachmentManager,
    //                               IEmployeeAppService employeeAppService) : base(repository,
    //                                                                              manager,
    //                                                                              categoryRepository,
    //                                                                              attachmentManager,
    //                                                                              employeeAppService,
    //                                                                              clsRepository,
    //                                                                              clsManager,
    //                                                                              CoreConsts.DefaultWorkOrderTypeName,
    //                                                                              CoreConsts.WorkOrderCreate,
    //                                                                              CoreConsts.WorkOrderUpdate,
    //                                                                              CoreConsts.WorkOrderDelete,
    //                                                                              CoreConsts.WorkOrderManager,
    //                                                                              CoreConsts.WorkOrderConfirme,
    //                                                                              CoreConsts.WorkOrderAllocate,
    //                                                                              CoreConsts.WorkOrderExecute,
    //                                                                              CoreConsts.WorkOrderCompletion,
    //                                                                              CoreConsts.WorkOrderReject)
    //    {
    //        if (!cfg.EnableDefaultWorkOrder)
    //            throw new ApplicationException("BXJGWorkOrderConfig.EnableDefaultWorkOrder=false");
    //    }


    //    protected override async ValueTask BeforeCreateAsync(OrderEntity entity, WorkOrderCreateInput input)
    //    {
    //        await base.BeforeCreateAsync(entity, input);

    //        entity.ExtendedField1 = input.ExtendedField1;
    //        entity.ExtendedField2 = input.ExtendedField2;
    //        entity.ExtendedField3 = input.ExtendedField3;
    //        entity.ExtendedField4 = input.ExtendedField4;
    //        entity.ExtendedField5 = input.ExtendedField5;
    //        if (input.ExtensionData != null)
    //        {
    //            foreach (var item in input.ExtensionData)
    //            {
    //                entity.SetData(item.Key, item.Value);
    //            }
    //        }
    //    }

    //    protected override async ValueTask BeforeEditAsync(OrderEntity entity, WorkOrderUpdateInput input)
    //    {
    //        await base.BeforeEditAsync(entity, input);
    //        entity.ExtendedField1 = input.ExtendedField1;
    //        entity.ExtendedField2 = input.ExtendedField2;
    //        entity.ExtendedField3 = input.ExtendedField3;
    //        entity.ExtendedField4 = input.ExtendedField4;
    //        entity.ExtendedField5 = input.ExtendedField5;
    //        if (input.ExtensionData != null)
    //        {
    //            foreach (var item in input.ExtensionData)
    //            {
    //                entity.SetData(item.Key, item.Value);
    //            }
    //        }
    //    }

    //    //使用了映射，省了
    //    //protected override WorkOrderDto EntityToDto(OrderEntity entity, IEnumerable<CategoryEntity> categories, IEnumerable<EmployeeDto> employees, IDictionary<object, List<AttachmentEntity>> images, object state = default)
    //    //{
    //    //    var dto = base.EntityToDto(entity, categories, employees,images, state);
    //    //    dto.ExtendedField1 = entity.ExtendedField1;
    //    //    dto.ExtendedField2 = entity.ExtendedField2;
    //    //    dto.ExtendedField3 = entity.ExtendedField3;
    //    //    dto.ExtendedField4 = entity.ExtendedField4;
    //    //    dto.ExtendedField5 = entity.ExtendedField5;
    //    //    if (!entity.ExtensionData.IsNullOrWhiteSpace())
    //    //    {
    //    //        dto.ExtensionData = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(entity.ExtensionData);
    //    //    }
    //    //    return dto;
    //    //}
    //}
}
