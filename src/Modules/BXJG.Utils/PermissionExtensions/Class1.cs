using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Threading;
using Medallion.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Authorization
{
    public static class Class1
    {
        private const string sdf = "Dependences";
        public static Permission DependencePermissions(this Permission permission, params string[] permissions)
        {
            permission[sdf] = permissions;
            return permission;
        }
        public static IEnumerable<Permission> GetDependencePermissions(this Permission permission)
        {
            if (permission.Properties.TryGetValue(sdf, out var r))
            {
                var persmissions = IocManager.Instance.Resolve<IPermissionManager>().GetAllPermissions();
                var ary = r as string[];
                return persmissions.Where(c => ary.Contains(c.Name));
            }
            return new Permission[0];
        }
        public static IEnumerable<Permission> GetDependentedPermissions(this Permission permission)
        {
            var persmissions = IocManager.Instance.Resolve<IPermissionManager>().GetAllPermissions();
            foreach (var item in persmissions)
            {
                if (item.Properties.TryGetValue(sdf, out var r))
                {
                    var ary = r as string[];
                    if (ary.Contains(permission.Name))
                        yield return item;
                }
            }
        }

        //public static async Task SetDependence<TRole, TUser>(this AbpRoleManager<TRole, TUser> roleManager, TRole role, IEnumerable<Permission> permissions)
        //    where TUser : AbpUser<TUser>
        //    where TRole : AbpRole<TUser>,new()
        //{
        //    //var defines = IocManager.Instance.Resolve<IPermissionManager>().GetAllPermissions().Where(c=> permi c.Name);
        //    //await  roleManager.SetGrantedPermissionsAsync(roleId, permissions);
        //    var byld = permissions.SelectMany(c => c.GetDependencePermissions()).GroupBy(c=>c.Name).SelectMany(c=>c);
        //    if(byld.Any())
        //   await  roleManager.SetGrantedPermissionsAsync(role, byld);
        //}
    }

    //在这里搞虽然是个理想的位置，但由于设置权限是批量的，这里搞性能太低，因为有大量遍历
    //public class Sdfdf : IAsyncEventHandler< EntityChangingEventData<RolePermissionSetting>>,
    //                     IAsyncEventHandler<EntityChangingEventData<UserPermissionSetting>>, 
    //                     ITransientDependency
    //{
    //    ICancellationTokenProvider cancellationTokenProvider;
    //    IPermissionManager permissionManager;
    //    IDistributedLockProvider distributedLockProvider;
    //    IRepository<RolePermissionSetting, long> rolePermissionRepository;

    //    IAsyncQueryableExecuter asyncQueryExecutor;

    //    public Sdfdf(IPermissionManager permissionManager, IDistributedLockProvider distributedLockProvider, ICancellationTokenProvider cancellationTokenProvider, IRepository<RolePermissionSetting, long> rolePermissionRepository, IAsyncQueryableExecuter asyncQueryExecutor)
    //    {
    //        this.permissionManager = permissionManager;
    //        this.distributedLockProvider = distributedLockProvider;
    //        this.cancellationTokenProvider = cancellationTokenProvider;
    //        this.rolePermissionRepository = rolePermissionRepository;
    //        this.asyncQueryExecutor = asyncQueryExecutor;
    //    }

    //    public async Task HandleEventAsync(EntityChangingEventData<RolePermissionSetting> eventData)
    //    {
    //        //a、b都依赖c
    //        //并发时a需要c b不需要c，导致并发错误
    //        //考虑分布式 乐观并发并不好处理
    //        //分布式锁 或 确保单线程执行
    //        var define = permissionManager.GetPermission(eventData.Entity.Name);
    //        if (define.GetDependentedPermissions().Any()) {
    //            return;
    //        }
    //        var defines = define.GetDependencePermissions();
    //        await using (await distributedLockProvider.AcquireLockAsync(this.GetType().FullName, default, cancellationTokenProvider.Token))
    //        {
    //            var names = defines.Select(c => c.Name);
    //            var q = rolePermissionRepository.GetAll().Where(c => names.Contains(c.Name) && c.RoleId == eventData.Entity.RoleId);
    //            var qx = await asyncQueryExecutor.ToListAsync(q);

    //        }
    //    }

    //    public Task HandleEventAsync(EntityChangingEventData<UserPermissionSetting> eventData)
    //    {
    //        //throw new NotImplementedException();
    //        return Task.CompletedTask;
    //    }
    //}
}