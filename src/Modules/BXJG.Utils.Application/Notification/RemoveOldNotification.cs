using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Notifications;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BXJG.Utils.Notification
{
    /// <summary>
    /// 删除过期通知
    /// 需要调用方提供子类，给出TUser类型
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public abstract class RemoveOldNotification<TUser> : PeriodicBackgroundWorkerBase, ISingletonDependency
        where TUser : AbpUserBase
    {
        public IUserNotificationManager UserNotificationManager { get; set; }

        public IRepository<TUser, long> UserRepository { get; set; }


        public RemoveOldNotification(AbpTimer timer) : base(timer)
        {
            Timer.Period = 1000 * 60 * 60 * 24;//24小时，时间短点，要确保服务器连续开机1天才会执行一次？也许首次运行程序也会执行一次
        }
        const int pageSize = 50;
        //[DisableAuditing]
        //[UnitOfWork]
        protected override async void DoWork()
        {
            base.Logger.Info("开始删除过期通知");
            var pageIndex = 1;
            while (true)
            {
                Thread.Sleep(1);
                using var uow = base.UnitOfWorkManager.Begin();
                var users = await UserRepository.GetAll().AsNoTrackingWithIdentityResolution()
                                                .Select(c => new { c.TenantId, c.FullName, c.Id })
                                                .Skip((pageIndex - 1) * pageSize)
                                                .Take(pageSize)
                                                .ToArrayAsync();
                if (!users.Any())
                    break;

                pageIndex++;

                foreach (var user in users)
                {
                    try
                    {
                        //删除50年前到半年前的，已读的通知
                        await UserNotificationManager.DeleteAllUserNotificationsAsync(new Abp.UserIdentifier(user.TenantId, user.Id),
                                                                                      UserNotificationState.Read,
                                                                                      DateTime.Now.AddYears(-50),
                                                                                      DateTime.Now.AddMonths(-6));
                    }
                    catch (Exception ex)
                    {
                        Logger.Debug($"删除用户{user.FullName}({user.Id})的过期已读消息失败！", ex);
                    }
                }
                await uow.CompleteAsync();
            }
            base.Logger.Info("删除过期通知完成");
        }
    }
}
