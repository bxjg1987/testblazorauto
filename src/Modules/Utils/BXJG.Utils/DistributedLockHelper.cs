using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Logging;
using Abp.Runtime.Session;
using Abp.Threading;
using Castle.Core.Logging;
using Medallion.Threading;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BXJG.Utils;

/*
 * 锁一般用在应用层，因为应用层是一个用例，多个领域服务应该在一个操作中
 * 它的范围跟uow相关，但通常小于uow，也就是应用服务方法中前面部分一般是判断
 * 中间部分开始用锁，然后在uow提交后释放锁
 * 所以使用aop在方法上用不好。
 * 
 * 但也可以考虑在领域服务方法中用，但总归是在uow释放后 释放锁
 * 
 * 
 */

/// <summary>
/// 分布式锁帮助器
/// 通常在应用服务方法的中间部分用，少数情况在领域服务方法中也可以
/// uow释放后会自动释放锁
/// 应用服务抽象类提供了包装方法，它内部调用这个类
/// </summary>
public class DistributedLockHelper : ITransientDependency
{
    private readonly IUnitOfWorkManager uow;
    private readonly IDistributedLockProvider dlocker;

    private readonly IAbpSession abpSession;
    private readonly ICancellationTokenProvider cancellationTokenProvider;

    private readonly ILogger logger;

    public DistributedLockHelper(IUnitOfWorkManager uow, IDistributedLockProvider dlocker, IAbpSession abpSession, ICancellationTokenProvider cancellationTokenProvider, ILogger logger)
    {
        this.uow = uow;
        this.dlocker = dlocker;
        this.abpSession = abpSession;
        this.cancellationTokenProvider = cancellationTokenProvider;
        this.logger = logger;

        //logger.Debug($"cancellationTokenProvider是否为空：{cancellationTokenProvider==null}");
    }

    public async Task AcquireLockAsync(string key, TimeSpan? timeout = default, CancellationToken ct = default)
    {

        if (ct == default)
            ct = cancellationTokenProvider.Token;
        var lockobj = await dlocker.AcquireLockAsync(key, timeout, ct);
        //事务结束后再释放锁才合理
        uow.Current.Disposed += (obj, arg) =>
        {
            //lockobj.Dispose();
            AsyncHelper.RunSync(async ()=>await lockobj.DisposeAsync());
        };
    }
    public Task AcquireLockTenantAsync(string key, TimeSpan? timeout = default, CancellationToken ct = default)
    {
        return AcquireLockAsync($"{key}_{abpSession.TenantId}", timeout, ct);
    }
    public async Task TryAcquireLockAsync(string key, TimeSpan timeout = default, CancellationToken ct = default)
    {
        if (ct == default)
            ct = cancellationTokenProvider.Token;
        var lockobj = await dlocker.TryAcquireLockAsync(key, timeout, ct);
        //事务结束后再释放锁才合理
        uow.Current.Disposed += (obj, arg) =>
        {
            lockobj.Dispose();
            //AsyncHelper.RunSync(async ()=>await lockobj.DisposeAsync());
        };
    }
    public Task TryAcquireLockTenantAsync(string key, TimeSpan timeout = default, CancellationToken ct = default)
    {
        return TryAcquireLockAsync($"{key}_{abpSession.TenantId}", timeout, ct);
    }
}