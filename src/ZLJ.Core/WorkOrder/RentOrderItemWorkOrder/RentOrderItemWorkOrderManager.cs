using BXJG.WorkOrder.WorkOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using BXJG.WorkOrder;
using ZLJ.Rent.Order;
using BXJG.WorkOrder.WorkOrderCategory;
using System.Threading.Channels;
using Abp.Threading.BackgroundWorkers;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Threading.Timers;
using System.Threading;
using Abp.Threading.Extensions;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using Abp.Events.Bus.Handlers;
using Abp.Events.Bus.Entities;
using Nito.AsyncEx;

namespace ZLJ.WorkOrder.RentOrderItemWorkOrder
{
    public class RentOrderItemWorkOrderCreateDto : WorkOrderCreateDtoBase
    {
        public long RentOrderItemId { get; set; }
    }

    /// <summary>
    /// 设备维修工单领域服务
    /// </summary>
    public class RentOrderItemWorkOrderManager : OrderBaseManager<RentOrderItemWorkOrderEntity>//,
                                                 //IAsyncEventHandler<EntityUpdatingEventData<RentOrderItemWorkOrderEntity>>,
                                                 //IAsyncEventHandler<EntityCreatingEventData<RentOrderItemWorkOrderEntity>>
    {
        //IRepository<ZLJ.Rent.Order.OrderItemEntity, long> rentOrderItemRepository;
        //IRepository<ZLJ.Rent.Order.OrderEntity, long> rentOrderRepository;

        public RentOrderItemWorkOrderManager(IRepository<RentOrderItemWorkOrderEntity, long> repository,
                                             IRepository<CategoryEntity, long> clsRepository,
                                             CategoryManager clsManager,BXJG.WorkOrder.WorkOrder.OrderNoGenerator orderNoGenerator) : base(repository,
                                                                                clsRepository,
                                                                                clsManager,
                                                                                WorkOrderConsts.RentOrderItemWorkOrder, orderNoGenerator)
        { }

        protected override ValueTask<RentOrderItemWorkOrderEntity> Create(WorkOrderCreateDtoBase input)
        {
            var dto = input as RentOrderItemWorkOrderCreateDto;
            var entity = new RentOrderItemWorkOrderEntity(dto.Time.Value,
                                                    dto.CategoryId.Value,
                                                    dto.Title,
                                                    dto.RentOrderItemId,
                                                    dto.Description,
                                                    dto.UrgencyDegree.Value,
                                                    dto.EmployeeId,
                                                    dto.EstimatedExecutionTime,
                                                    dto.EstimatedCompletionTime);
            return ValueTask.FromResult(entity);
        }

        /// <summary>
        /// 尝试获取指定设备的工单
        /// 一个租赁明细，也就是一个客户那里的设备，只能允许有一个未完成且未拒绝的工单，此方法尝试获取这个工单
        /// </summary>
        /// <param name="RentOrderItemId">租赁明细id</param>
        /// <returns></returns>
        public async Task<List<long>> GetUnfinishedAsync(long rentOrderItemId)
        {
            var pc = new BXJG.WorkOrder.WorkOrder.Status[] { BXJG.WorkOrder.WorkOrder.Status.Rejected, BXJG.WorkOrder.WorkOrder.Status.Completed };
            var q = repository.GetAll().Where(c => c.RentOrderItemId == rentOrderItemId && !pc.Contains(c.Status)).Select(c => c.Id);
            return await AsyncQueryableExecuter.ToListAsync(q);
        }
        ///// <summary>
        ///// 同一设备，若已存在拒绝绝和未完成的工单，则不允许重复
        ///// 目前使用这个锁的方式实现，因为lock(obj)的方式不支持await
        ///// 分布式环境需要重构，详情参考根目录下“必读”
        ///// </summary>
        //private static readonly AsyncLock _mutex = new AsyncLock();//注意全局静态
        ///// <summary>
        ///// 设备维修工单修改前触发
        ///// 它与修改操作在同一事务中
        ///// </summary>
        ///// <param name="eventData"></param>
        ///// <returns></returns>
        //public async Task HandleEventAsync(EntityUpdatingEventData<RentOrderItemWorkOrderEntity> eventData)
        //{
        //    #region 重复性检查
        //    //为毛不用工单状态改变事件？那样的事件不是更精确吗？现在的方式即使没有修改状态也会触发，比较浪费
        //    //在后台管理工单时，工单状态可以跳跃(从待执行直接变成已完成)，但在实体内部会先 变成已执行，然后再变成已完成，会触发两次状态改变事件
        //    //在跳跃性前进或回退工单状态时，状态变化事件触发太频繁，在重复性检查场景中使用此事件不合适。

        //    //Updating事件有个缺陷，无法在这里获取修改前的值
        //    //貌似它先执行的数据更新，然后触发Updating事件，所以即使using beginUnitOfWork获取到的也是修改后的值
        //    //所以这里的代码很垃圾，暂时就这么用了
        //    //需重构
        //    //BXJG.WorkOrder.WorkOrder.Status oldStatus = default;
        //    //using (var uow = unitOfWorkManager.Begin(  System.Transactions.TransactionScopeOption.Suppress))
        //    //{
        //    //    var pp = await repository.GetAll().Where(c => c.Id == eventData.Entity.Id).SingleAsync();
        //    //    var sss = pp.Equals(eventData.Entity);

        //    //    oldStatus = await repository.GetAll().Where(c => c.Id == eventData.Entity.Id).Select(c => c.Status ).SingleAsync();
        //    //}

        //    //if (oldStatus != eventData.Entity.Status)
        //    //{
        //    //var pc = new BXJG.WorkOrder.WorkOrder.Status[] { BXJG.WorkOrder.WorkOrder.Status.Rejected, BXJG.WorkOrder.WorkOrder.Status.Completed };
        //    await NewMethod(eventData.Entity.Id, eventData.Entity.RentOrderItemId);
        //    //}
        //    #endregion
        //}
        ///// <summary>
        ///// 设备维修工单新增前触发
        ///// 它与新增操作在同一事务中
        ///// </summary>
        ///// <param name="eventData"></param>
        ///// <returns></returns>
        //public async Task HandleEventAsync(EntityCreatingEventData<RentOrderItemWorkOrderEntity> eventData)
        //{
        //    await NewMethod(eventData.Entity.Id, eventData.Entity.RentOrderItemId);
        //}

        //async Task NewMethod(long entityId, long itemId)
        //{
        //    #region 重复性检查
        //    using (await _mutex.LockAsync())
        //    {
        //        var ids = await GetUnfinishedAsync(itemId);
        //        //Creating事件是先操作数据库，因此ids中一定会包含正要新增的数据
        //        if (ids.Count == 1 && ids.First() == entityId || ids==null|| ids.Count==0)
        //            return;
        //        throw new RentOrderItemWorkOrderOnlyException("同一设备存在未完结的工单！", ids); //throw new RentOrderItemWorkOrderOnlyException();
        //                                                                             //反正新增前也会判断一次的，这里只是兜底
        //    }
        //    #endregion
        //}
    }
}
