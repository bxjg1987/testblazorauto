using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZLJ.WorkOrder.RentOrderItemWorkOrder;

namespace ZLJ.WorkOrder.RentOrderItemWorkOrder
{
    /// <summary>
    /// 设备维修工单的事件处理器
    /// </summary>
    public class EventHandler : IAsyncEventHandler<EntityUpdatingEventData<RentOrderItemWorkOrderEntity>>,
                                IAsyncEventHandler<EntityCreatingEventData<RentOrderItemWorkOrderEntity>>,
                                ITransientDependency //ioc注册必须加，通常是单例，但为了节省内存，我一般使用ITransientDependency
    {
        readonly IRepository<RentOrderItemWorkOrderEntity, long> repository;
        //readonly IUnitOfWorkManager unitOfWorkManager;

        public EventHandler(IRepository<RentOrderItemWorkOrderEntity, long> repository/*, IUnitOfWorkManager unitOfWorkManager*/)
        {
            this.repository = repository;
           // this.unitOfWorkManager = unitOfWorkManager;
        }
        /// <summary>
        /// 同一设备，若已存在拒绝绝和未完成的工单，则不允许重复
        /// 目前使用这个锁的方式实现，因为lock(obj)的方式不支持await
        /// 分布式环境需要重构，详情参考根目录下“必读”
        /// </summary>
        private static readonly AsyncLock _mutex = new AsyncLock();//注意全局静态
        /// <summary>
        /// 设备维修工单修改前触发
        /// 它与修改操作在同一事务中
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public async Task HandleEventAsync(EntityUpdatingEventData<RentOrderItemWorkOrderEntity> eventData)
        {
            #region 重复性检查
            //为毛不用工单状态改变事件？那样的事件不是更精确吗？现在的方式即使没有修改状态也会触发，比较浪费
            //在后台管理工单时，工单状态可以跳跃(从待执行直接变成已完成)，但在实体内部会先 变成已执行，然后再变成已完成，会触发两次状态改变事件
            //在跳跃性前进或回退工单状态时，状态变化事件触发太频繁，在重复性检查场景中使用此事件不合适。
   
            //Updating事件有个缺陷，无法在这里获取修改前的值
            //貌似它先执行的数据更新，然后触发Updating事件，所以即使using beginUnitOfWork获取到的也是修改后的值
            //所以这里的代码很垃圾，暂时就这么用了
            //需重构
            //BXJG.WorkOrder.WorkOrder.Status oldStatus = default;
            //using (var uow = unitOfWorkManager.Begin(  System.Transactions.TransactionScopeOption.Suppress))
            //{
            //    var pp = await repository.GetAll().Where(c => c.Id == eventData.Entity.Id).SingleAsync();
            //    var sss = pp.Equals(eventData.Entity);

            //    oldStatus = await repository.GetAll().Where(c => c.Id == eventData.Entity.Id).Select(c => c.Status ).SingleAsync();
            //}
        
            //if (oldStatus != eventData.Entity.Status)
            //{
                var pc = new BXJG.WorkOrder.WorkOrder.Status[] { BXJG.WorkOrder.WorkOrder.Status.Rejected, BXJG.WorkOrder.WorkOrder.Status.Completed };
                using (await _mutex.LockAsync())
                {
                    //同一设备、且工单状态不是未拒绝和未完成、且不是当前工单（新增时不需要最后这个判断）
                    var exist = await repository.GetAll().AnyAsync(c => c.RentOrderItemId == eventData.Entity.RentOrderItemId && !pc.Contains(c.Status) && c.Id != eventData.Entity.Id);
                    if (exist)
                        throw new UserFriendlyException("相同设备存在未完结的工单！请联系管理员。");
                }
            //}
            #endregion
        }
        /// <summary>
        /// 设备维修工单新增前触发
        /// 它与新增操作在同一事务中
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public async Task HandleEventAsync(EntityCreatingEventData<RentOrderItemWorkOrderEntity> eventData)
        {
            #region 重复性检查
            var pc = new BXJG.WorkOrder.WorkOrder.Status[] { BXJG.WorkOrder.WorkOrder.Status.Rejected, BXJG.WorkOrder.WorkOrder.Status.Completed };
            using (await _mutex.LockAsync())
            {
                var exist = await repository.GetAll().AnyAsync(c => c.RentOrderItemId == eventData.Entity.RentOrderItemId && !pc.Contains(c.Status));
                if (exist)
                    throw new UserFriendlyException("相同设备存在未完结的工单！请联系管理员。"); //throw new RentOrderItemWorkOrderOnlyException();
                //反正新增前也会判断一次的，这里只是兜底
            }
            #endregion
        }
    }
}
