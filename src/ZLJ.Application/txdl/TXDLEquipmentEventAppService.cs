using System.Collections.Generic;
using Abp.Application.Services;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using txdl;
using ZLJ.BaseInfo.EquipmentInfo;
using ZLJ.Equipments.EquipmentInstances;
using Abp.Auditing;
using System;
using Abp.Domain.Uow;
using Abp.Timing;
using Abp.Dependency;
using Microsoft.Extensions.Options;
using ZLJ.Configuration;
//using ZLJ.txdl;
using System.Threading;
using System.Diagnostics;
using ZLJ.Customer;
using Abp.Organizations;
using Abp.Authorization.Users;
using Medallion.Threading;
using DocumentFormat.OpenXml.Office2010.Excel;
using Abp.Notifications;
using System.Linq;
using OpenXmlPowerTools;

namespace ZLJ.App.txdl
{
    [DisableAuditing]
    public class TXDLEquipmentEventAppService : ApplicationService, ISingletonDependency
    {
        IRepository<CJJLEntity, Guid> cjjlRepository;
        IRepository<CustomerStaffCjjlEntity, Guid> cjjl2Repository;
        IRepository<EquipmentInstanceEntity, string> equipmentInstanceRepository;
        //EquipmentControlCenterConfig appOptions;
        IDistributedLockProvider distributedLockProvider;
        ICancellationTokenProvider cancellationToken;
        INotificationPublisher notificationPublisher;
        public TXDLEquipmentEventAppService(IRepository<EquipmentInstanceEntity, string> equipmentInstanceRepository,
                                            //IOptions<EquipmentControlCenterConfig> appOptions,
                                            IRepository<CJJLEntity, Guid> cjjlRepository,
                                            IRepository<CustomerStaffCjjlEntity, Guid> cjjl2Repository,
                                            IDistributedLockProvider distributedLockProvider,
                                            ICancellationTokenProvider cancellationToken,
                                            INotificationPublisher notificationPublisher)
        {
            this.equipmentInstanceRepository = equipmentInstanceRepository;
            //this.appOptions = appOptions.Value;
            this.cjjlRepository = cjjlRepository;
            this.cjjl2Repository = cjjl2Repository;
            this.distributedLockProvider = distributedLockProvider;
            this.cancellationToken = cancellationToken;
            this.notificationPublisher = notificationPublisher;
        }

        /*
         * 经过测试，不修改连接字符串中的连接池大小，也就是保持默认，这里按默认开启事务，会提示连接池不够用，因为采集任务一直在请求，导致整个应用一直无法访问数据库
         * 直接关闭事务，保持连接池的默认设置，好像没问题。正常来说应该开启事务，但是即便错误 问题也不大，为了提高性能 关闭事务
         * 开启事务，将连接池设置为500，好像也没问题，保险起见设置超时为3秒
         * 
         * 但现在确实需要事务
         * 
         * txdl可能因为设计错误，可能不按顺序或同一次采集来两个线程
         * 为了确保万无一失，这里应该考虑并发问题
         */

        [UnitOfWork(IsDisabled = true)]
        public async Task SnapshotChanged(EquipmentSnapshotWrapper<EquipmentStatusSnapshot, EquipmentConsumableSnapshot> input)
        {
            await using (var lodker = await distributedLockProvider.AcquireLockAsync($"ccjl{input.MachineNo}", TimeSpan.FromSeconds(20), cancellationToken.Token))
            {
                using (var tran = UnitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.RequiresNew))
                {
                    UnitOfWorkManager.Current.DisableTenantFilter();
                    //this.DisableTenantFilter();
                    Logger.Debug($"收到设备【{input.MachineNo}】同步信息：{JsonConvert.SerializeObject(input)}");
                    var item = await equipmentInstanceRepository.GetAllIncluding(x => x.Alarms)
                                                                .SingleAsync(x => x.Id == input.MachineNo);

                    UnitOfWorkManager.Current.SetTenantId(item.TenantId);

                    //不要直接返回数据，因为不是每个用户当前都关心这个事件，谁关心，谁再来查
                    await notificationPublisher.PublishAsync(notificationName: "equipmentInstanceStateChanged",
                                                             entityIdentifier: new EntityIdentifier(item.GetType(), item.Id),
                                                             tenantIds: new int?[] { item.TenantId });




                    item.LastCollectTime = Clock.Now;




                    if (input.EquipmentSnapshot != null)
                    {
                        item.Description = input.EquipmentSnapshot.Description;
                        item.LockStatus = input.EquipmentSnapshot.SwitchStatus ? LockStatus.Unlock : LockStatus.Locked;
                        //Logger.Warn($"设备【{input.MachineNo}】锁定状态{item.LockStatus}");
                        //item.gxzxzt(appOptions.sbslzxpdsc, Clock.Now);//这里应该搞到配置里去，且与txdl保持一直
                        item.Alarms = ObjectMapper.Map<List<EquipmentInstanceAlarmEntity>>(input.EquipmentSnapshot.Alarms ?? new List<EquipmentStatusItemSnapshot>());
                    }

                    if (item.LockStatus == LockStatus.Locked)
                    {
                        await tran.CompleteAsync();
                        return;
                    }

                    if (input.EquipmentSnapshot2 != null)
                    {
                        item.BlackTonerAmount = input.EquipmentSnapshot2.BlackTonerAmount ?? item.BlackTonerAmount;
                        item.CyanTonerAmount = input.EquipmentSnapshot2.CyanTonerAmount ?? item.CyanTonerAmount;
                        item.MagentaTonerAmount = input.EquipmentSnapshot2.MagentaTonerAmount ?? item.MagentaTonerAmount;
                        item.YellowTonerAmount = input.EquipmentSnapshot2.YellowTonerAmount ?? item.YellowTonerAmount;

                        item.A3BlackAndWhiteCount = input.EquipmentSnapshot2.A3BlackAndWhiteCount ?? item.A3BlackAndWhiteCount;
                        item.A3TrueColorCount = input.EquipmentSnapshot2.A3TrueColorCount ?? item.A3TrueColorCount;
                        item.A4BlackAndWhiteCount = input.EquipmentSnapshot2.A4BlackAndWhiteCount ?? item.A4BlackAndWhiteCount;
                        item.A4TrueColorCount = input.EquipmentSnapshot2.A4TrueColorCount ?? item.A4TrueColorCount;
                        item.jszl();

                        if (item.Count <= 0)
                        {
                            Logger.Warn($"设备【{input.MachineNo}】清零");
                            await ql(item);
                            await tran.CompleteAsync();
                            return;
                        }

                        var cjjl = new CJJLEntity
                        {
                            A3BlackAndWhiteCount = item.A3BlackAndWhiteCount,
                            A3TrueColorCount = item.A3TrueColorCount,
                            A4BlackAndWhiteCount = item.A4BlackAndWhiteCount,
                            A4TrueColorCount = item.A4TrueColorCount,
                            Id = Guid.NewGuid(),
                            Instance = item,
                            InstanceId = item.Id,
                            //TenantId = item.TenantId,//前面已设置当前uow的租户id
                        };
                        cjjl.ComputeCount();

                        var pre = await cjjlRepository.GetAll()
                                                      .Include(c => c.StaffJl)
                                                      .Where(c => c.InstanceId == cjjl.InstanceId)
                                                      .OrderByDescending(c => c.CreationTime)
                                                      .FirstOrDefaultAsync();

                        if (pre == default)
                            pre = new CJJLEntity();

                        cjjl.A3BlackAndWhiteUse = cjjl.A3BlackAndWhiteCount - pre.A3BlackAndWhiteCount;
                        cjjl.A3TrueColorUse = cjjl.A3TrueColorCount - pre.A3TrueColorCount;
                        cjjl.A4BlackAndWhiteUse = cjjl.A4BlackAndWhiteCount - pre.A4BlackAndWhiteCount;
                        cjjl.A4TrueColorUse = cjjl.A4TrueColorCount - pre.A4TrueColorCount;
                        cjjl.ComputeUse();

                        //存在上一个记录，但用量为0，说明数量没变化
                        if (cjjl.Use == 0)
                        {
                            pre.CreationTime = Clock.Now;
                            //var items = await cjjl2Repository.GetAll().Where(c => c.CjjlId == pre.Id).ToListAsync();
                            foreach (var itemq in pre.StaffJl)
                            {
                                itemq.CreationTime = Clock.Now;
                            }
                            await tran.CompleteAsync();
                            return;
                        }

                        if (cjjl.Use < 0)
                        {
                            Logger.Warn($"设备{item.Id}采集用量出现负数");
                            //await ql(item);
                            return;
                        }

                        await cjjlRepository.InsertAsync(cjjl);

                        //var userIds = input.EquipmentSnapshot2.UserConsumables.Select(c => long.Parse(c.UserId));

                        //var qq = from c in ouRepository.GetAll().AsNoTrackingWithIdentityResolution()
                        //         join b in ouUserRepository.GetAll().AsNoTrackingWithIdentityResolution() on c.Id equals b.OrganizationUnitId into ouss
                        //         from b in ouss.DefaultIfEmpty()
                        //         where userIds.Contains(b.UserId)
                        //         select new { b.UserId, b.OrganizationUnitId, c.DisplayName, c.CustomerId };
                        //var ous = await qq.ToListAsync();

                        // 定时任务删除采集记录时，由于外键，会自动删除用户数量记录
                        foreach (var item3 in input.EquipmentSnapshot2.UserConsumables)
                        {
                            var cjjl2 = new CustomerStaffCjjlEntity
                            {
                                A3BlackAndWhiteCount = item3.A3BlackAndWhiteCount.Value,
                                A3TrueColorCount = item3.A3TrueColorCount.Value,
                                A4BlackAndWhiteCount = item3.A4BlackAndWhiteCount.Value,
                                A4TrueColorCount = item3.A4TrueColorCount.Value,
                                CustomerStaffInfoId = long.Parse(item3.UserId),
                                InstanceId = item.Id,
                                CjjlId = cjjl.Id,
                                Id = Guid.NewGuid(),
                                //TenantId = item.TenantId, 当前uow已设置租户
                                //CustomerOuId = ous.SingleOrDefault(c => c.UserId.ToString() == item3.UserId)?.OrganizationUnitId,
                                //CustomerId = ous.First().CustomerId
                            };
                            cjjl2.ComputeCount();
                            //注意排除增量
                            var oldcjjl = pre.StaffJl.SingleOrDefault(c => c.CustomerStaffInfoId == cjjl2.CustomerStaffInfoId && c.Count > 0);
                            if (oldcjjl == default)
                                oldcjjl = new CustomerStaffCjjlEntity();

                            if (cjjl2.Count == oldcjjl.Count)
                                continue;

                            cjjl2.A3BlackAndWhiteUse = cjjl2.A3BlackAndWhiteCount - oldcjjl.A3BlackAndWhiteCount;
                            cjjl2.A3TrueColorUse = cjjl2.A3TrueColorCount - oldcjjl.A3TrueColorCount;
                            cjjl2.A4BlackAndWhiteUse = cjjl2.A4BlackAndWhiteCount - oldcjjl.A4BlackAndWhiteCount;
                            cjjl2.A4TrueColorUse = cjjl2.A4TrueColorCount - oldcjjl.A4TrueColorCount;
                            cjjl2.ComputeUse();
                            //if (cjjl2.Use == 0)
                            //    continue;
                            await cjjl2Repository.InsertAsync(cjjl2);
                        }
                        //await UnitOfWorkManager.Current.SaveChangesAsync();
                        await zl(item, cjjl.Id);
                    }
                    await tran.CompleteAsync();
                }
            }
        }

        async Task ql(EquipmentInstanceEntity eq)
        {
            eq.jsql();
            await cjjl2Repository.DeleteAsync(c => c.InstanceId == eq.Id);
            await UnitOfWorkManager.Current.SaveChangesAsync();
            await cjjlRepository.DeleteAsync(c => c.InstanceId == eq.Id);
            //若同一设备来个并发，因为有锁，前面一个是清空，后一个执行时又被覆盖了。
            //所以要求采集时若出现负数，就放弃
        }

        async Task zl(EquipmentInstanceEntity eq, Guid id)
        {
            var query = cjjl2Repository.GetAll().AsNoTrackingWithIdentityResolution().Where(c => c.InstanceId == eq.Id);
            #region MyRegion
            //当前设备2，按用户统计用量
            /*
             * 设备     用户      a3用量合计    a4用量合计
             * 2        a           12          54
             * 2        b           21          31
             */
            var tj = await query.GroupBy(c => c.InstanceId)
                                .Select(c => new
                                {
                                    A3BlackAndWhiteUse = c.Sum(d => d.A3BlackAndWhiteUse),
                                    A3TrueColorUse = c.Sum(d => d.A3TrueColorUse),
                                    A4BlackAndWhiteUse = c.Sum(d => d.A4BlackAndWhiteUse),
                                    A4TrueColorUse = c.Sum(d => d.A4TrueColorUse),
                                })
                                .SingleOrDefaultAsync();
            if (tj == default)
                tj = new { A3BlackAndWhiteUse = 0, A3TrueColorUse = 0, A4BlackAndWhiteUse = 0, A4TrueColorUse = 0 };
            #endregion
            #region 用量合计
            //var A3BlackAndWhiteUse = tj.Sum(d => d.A3BlackAndWhiteUse);
            //var A3TrueColorUse = tj.Sum(d => d.A3TrueColorUse);
            //var A4BlackAndWhiteUse = tj.Sum(d => d.A4BlackAndWhiteUse);
            //var A4TrueColorUse = tj.Sum(d => d.A4TrueColorUse);
            #endregion
            #region 用量增量
            var A3BlackAndWhiteZL = eq.A3BlackAndWhiteCount - tj.A3BlackAndWhiteUse;
            var A3TrueColorZL = eq.A3TrueColorCount - tj.A3TrueColorUse;
            var A4BlackAndWhiteZL = eq.A4BlackAndWhiteCount - tj.A4BlackAndWhiteUse;
            var A4TrueColorZL = eq.A4TrueColorCount - tj.A4TrueColorUse;
            if (A3BlackAndWhiteZL + A3TrueColorZL + A4BlackAndWhiteZL + A4TrueColorZL == 0)
                return;
            #endregion
            #region 最后增量

            var sczl = await query.Where(c => c.Count == 0)
                                  .OrderByDescending(c => c.CreationTime)
                                  .Select(c => new { c.CjjlId, c.CreationTime })
                                  .FirstOrDefaultAsync();

            var tg = await query.WhereIf(sczl != default, c => c.CreationTime > sczl.CreationTime && c.CjjlId != sczl.CjjlId)
                                .GroupBy(c => c.CustomerStaffInfoId)
                                .Select(c => new
                                {
                                    CustomerStaffInfoId = c.Key,
                                    A3BlackAndWhiteUse = c.Sum(d => d.A3BlackAndWhiteUse),
                                    A3TrueColorUse = c.Sum(d => d.A3TrueColorUse),
                                    A4BlackAndWhiteUse = c.Sum(d => d.A4BlackAndWhiteUse),
                                    A4TrueColorUse = c.Sum(d => d.A4TrueColorUse),
                                })
                                .ToListAsync();
            #endregion
            #region 准备插入
            var A3BlackAndWhiteZLUse = tg.Sum(d => d.A3BlackAndWhiteUse);
            var A3TrueColorZLUse = tg.Sum(d => d.A3TrueColorUse);
            var A4BlackAndWhiteZLUse = tg.Sum(d => d.A4BlackAndWhiteUse);
            var A4TrueColorZLUse = tg.Sum(d => d.A4TrueColorUse);

            var lscc = new List<CustomerStaffCjjlEntity>();
            foreach (var item in tg)
            {
                int yl1, yl2, yl3, yl4;

                if (A3BlackAndWhiteZLUse > 0)
                {
                    var bl1 = item.A3BlackAndWhiteUse * 1m / A3BlackAndWhiteZLUse * 1m;
                    yl1 = Convert.ToInt32(A3BlackAndWhiteZL * bl1);
                }
                else
                    yl1 = A3BlackAndWhiteZL / tg.Count;

                if (A3TrueColorZLUse > 0)
                {
                    var bl2 = item.A3TrueColorUse * 1m / A3TrueColorZLUse * 1m;
                    yl2 = Convert.ToInt32(A3TrueColorZL * bl2);
                }
                else
                    yl2 = A3TrueColorZL / tg.Count;

                if (A4BlackAndWhiteZLUse > 0)
                {
                    var bl3 = item.A4BlackAndWhiteUse * 1m / A4BlackAndWhiteZLUse * 1m;
                    yl3 = Convert.ToInt32(A4BlackAndWhiteZL * bl3);
                }
                else
                    yl3 = A4BlackAndWhiteZL/ tg.Count;


                if (A4TrueColorZLUse > 0)
                {
                    var bl4 = item.A4TrueColorUse * 1m / A4TrueColorZLUse * 1m;
                    yl4 = Convert.ToInt32(A4TrueColorZL * bl4);
                }else
                    yl4 = A4TrueColorZL / tg.Count;


                var zlEntity = new CustomerStaffCjjlEntity
                {
                    A3BlackAndWhiteUse = yl1,
                    A3TrueColorUse = yl2,
                    A4BlackAndWhiteUse = yl3,
                    A4TrueColorUse = yl4,
                    CjjlId = id,
                    //TenantId = eq.TenantId,//当前uow已设置
                    CustomerStaffInfoId = item.CustomerStaffInfoId,
                    InstanceId = eq.Id,
                    Id = Guid.NewGuid(),
                };
                zlEntity.ComputeUse();
                if (zlEntity.Use == 0)
                    continue;
                lscc.Add(zlEntity);
            }
            if (lscc.Count == 0)
                return;
            #endregion

            #region 除不尽的处理
            lscc.MaxBy(c => c.A3TrueColorUse).A3TrueColorUse += (A3TrueColorZL - lscc.Sum(c => c.A3TrueColorUse));
            lscc.MaxBy(c => c.A4TrueColorUse).A4TrueColorUse += (A4TrueColorZL - lscc.Sum(c => c.A4TrueColorUse));
            lscc.MaxBy(c => c.A3BlackAndWhiteUse).A3BlackAndWhiteUse += (A3BlackAndWhiteZL - lscc.Sum(c => c.A3BlackAndWhiteUse));
            lscc.MaxBy(c => c.A4BlackAndWhiteUse).A4BlackAndWhiteUse += (A4BlackAndWhiteZL - lscc.Sum(c => c.A4BlackAndWhiteUse));
            #endregion
            #region MyRegion
            foreach (var item in lscc)
            {
                await cjjl2Repository.InsertAsync(item);
            }
            #endregion
        }
    }
}