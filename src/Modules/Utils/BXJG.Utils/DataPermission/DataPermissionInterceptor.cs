using Abp.Application.Services;
using Abp.Authorization.Users;
using Abp.CachedUniqueKeys;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Organizations;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Abp.Threading;
using Abp.Timing;
using BXJG.Utils.Share.DataPermission;
using Castle.Core;
using Castle.Core.Logging;
using Castle.DynamicProxy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BXJG.Utils.DataPermission
{
    /// <summary>
    /// 数据权限拦截器
    /// 它为dbcontext中的全局数据拦截器准备数据
    /// </summary>
    public class DataPermissionInterceptor : AbpInterceptorBase, ITransientDependency
    {
        public ILogger Logger { get; set; }
        public IAbpSession AbpSession { get; set; }
        public DataPermissionInterceptor()
        {
            Logger = NullLogger.Instance;
        }


        protected override async Task InternalInterceptAsynchronous(IInvocation invocation)
        {
            var proceedInfo = invocation.CaptureProceedInfo();

            // 1. 最高优先级：检查方法上是否有 Disable 属性
            if (invocation.Method.IsDefined(typeof(DisableDataPermissionAttribute), true))
            {
                proceedInfo.Invoke();
                await (Task)invocation.ReturnValue;
            }

            await LoadDataPermission();
            using var df = CurrentUnitOfWorkProvider.Current.EnableFilter(DataPermissionConsts.DataPermission);
            proceedInfo.Invoke();
            await (Task)invocation.ReturnValue;


        }
        protected override async Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation)
        {
            var proceedInfo = invocation.CaptureProceedInfo();

            // 1. 最高优先级：检查方法上是否有 Disable 属性
            if (invocation.Method.IsDefined(typeof(DisableDataPermissionAttribute), true))
            {
                proceedInfo.Invoke();
                return await (Task<TResult>)invocation.ReturnValue; ;
            }

            //Before method execution
            //var stopwatch = Stopwatch.StartNew();
            await LoadDataPermission();
            using var df = CurrentUnitOfWorkProvider.Current.EnableFilter(DataPermissionConsts.DataPermission);

            proceedInfo.Invoke();

            return await (Task<TResult>)invocation.ReturnValue;

        }

        public override void InterceptSynchronous(IInvocation invocation)
        {
            // base.InterceptSynchronous(invocation);
            invocation.Proceed();
            //throw new NotImplementedException();
        }


        public IRepository<DataPermissionEntity, Guid> DataPermissionRepository { get; set; }
        public IRepository<UserRole, long> UserRoleRepository { get; set; }
        public IRepository<UserOrganizationUnit, long> UserOrganizationUnitRepository { get; set; }
        public ICancellationTokenProvider CancellationTokenProvider { get; set; }
        public IRepository<OrganizationUnit, long> OrganizationUnitRepository { get; set; }
        public ICurrentUnitOfWorkProvider CurrentUnitOfWorkProvider { get; set; }
        //const string ck
        public ICacheManager CacheManager { get; set; }

        /// <summary>
        /// 准备数据权限相关数据
        /// </summary>
        /// <returns></returns>
        async Task LoadDataPermission()
        {
            //DataPermissionDto

            if (!AbpSession.UserId.HasValue)
                return;

            // 获取缓存实例并设置过期时间
            var cache = CacheManager.GetCache<string, Dictionary<string, DataPermissionDto>>(DataPermissionConsts.DataPermission);
            var ck = AbpSession.UserId.ToString() + "@" + AbpSession.TenantId;
            var dtos = await cache.GetOrDefaultAsync(ck);
            if (dtos == null)
            {
                dtos = await LoadCore();
                //人员角色、组织单位变化时，应该删除缓存，数据权限变动时也应该删除缓存，然后这里再改为滑动过期。目前未实现
                await cache.SetAsync(ck, dtos, absoluteExpireTime: Clock.Now.AddMinutes(3));
            }

            //var dtos = await CacheManager.GetCache<string, List<DataPermissionDto>>(DataPermissionConsts.DataPermission)
            //                     .GetAsync(AbpSession.UserId.Value.ToString(), LoadCore);

            CurrentUnitOfWorkProvider.Current.Items[DataPermissionConsts.DataPermission] = dtos;
        }

        async Task<Dictionary<string, DataPermissionDto>> LoadCore()
        {
            var uid = AbpSession.UserId;
            var dpq = await DataPermissionRepository.GetAllReadonlyAsync();
            var urq = await UserRoleRepository.GetAllReadonlyAsync();
            var uoq = await UserOrganizationUnitRepository.GetAllReadonlyAsync();

            // 左连接用户角色和用户组织单位查询数据权限
            var query = (from dp in dpq
                         join ur in urq on dp.RoleId equals ur.RoleId into urGroup
                         from ur in urGroup.DefaultIfEmpty()
                         join uo in uoq on dp.UserOrganizationUnit equals uo.UserId into uoGroup
                         from uo in uoGroup.DefaultIfEmpty()
                         where dp.UserId == uid || ur.UserId == uid || uo.UserId == uid
                         select new
                         {
                             dp.EntityTypeFullName,
                             dp.DataOrganizationUnit,
                             dp.GrantType
                         }).Distinct();

            var result = await query.ToListAsync(CancellationTokenProvider.Token);

            var dtos = new Dictionary<string, DataPermissionDto>();
            var gp = result.GroupBy(x => x.EntityTypeFullName);
            //原则是，拒绝优先
            foreach (var item in gp)
            {
                var dto = new DataPermissionDto();
                if (item.Any(x => x.GrantType == DataPermissionGrantType.Rejected))
                {
                    dto.GrantType = DataPermissionGrantType.Rejected;
                }
                else if (item.Any(x => x.GrantType == DataPermissionGrantType.OnlyMe))
                {
                    dto.GrantType = DataPermissionGrantType.OnlyMe;
                }
                else if (item.Any(x => x.GrantType == DataPermissionGrantType.OrganizationUnit))
                {
                    dto.GrantType = DataPermissionGrantType.OrganizationUnit;
                    dto.OrganizationUnitIds = item.Where(x => x.GrantType == DataPermissionGrantType.OrganizationUnit && x.DataOrganizationUnit.HasValue).Select(x => x.DataOrganizationUnit.Value).Distinct();
                }
                else if (item.Any(x => x.GrantType == DataPermissionGrantType.OrganizationUnitRecursive))
                {
                    dto.GrantType = DataPermissionGrantType.OrganizationUnitRecursive;
                    var qt = item.Where(x => x.GrantType == DataPermissionGrantType.OrganizationUnitRecursive && x.DataOrganizationUnit.HasValue).Select(x => x.DataOrganizationUnit.Value).Distinct();
                    if (qt.Any())
                    {
                        var ouq = await OrganizationUnitRepository.GetAllReadonlyAsync();
                        dto.OrganizationUnitCodes = await ouq.Where(x => qt.Contains(x.Id)).Select(x => x.Code).ToListAsync(CancellationTokenProvider.Token);
                    }
                }
                else// if (item.Any(x => x.GrantType == DataPermissionGrantType.All))
                {
                    dto.GrantType = DataPermissionGrantType.All;
                }
                //else
                //{
                //    dto.GrantType = DataPermissionGrantType.OrganizationUnit | DataPermissionGrantType.OrganizationUnitRecursive;
                //    dto.OrganizationUnitIds = item.Where(x => x.GrantType == DataPermissionGrantType.OrganizationUnit && x.DataOrganizationUnit.HasValue).Select(x => x.DataOrganizationUnit.Value).Distinct();

                //    var qt = item.Where(x => x.GrantType == DataPermissionGrantType.OrganizationUnitRecursive && x.DataOrganizationUnit.HasValue).Select(x => x.DataOrganizationUnit.Value).Distinct();
                //    if (qt.Any())
                //    {
                //        var ouq = await OrganizationUnitRepository.GetAllReadonlyAsync();
                //        dto.OrganizationUnitCodes = await ouq.Where(x => qt.Contains(x.Id)).Select(x => x.Code).ToListAsync(CancellationTokenProvider.Token);
                //    }
                //}
                dtos.Add(item.Key, dto);
            }


            return dtos;
        }



        public static void Initialize(IIocManager iocManager)
        {
            iocManager.IocContainer.Kernel.ComponentRegistered += (key, handler) =>
            {
                if (ShouldIntercept(handler.ComponentModel.Implementation))
                {
                    handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(AbpAsyncDeterminationInterceptor<DataPermissionInterceptor>)));
                }
            };
        }
        private static bool ShouldIntercept(Type type)
        {
            //// 只拦截应用服务
            //if (!typeof(IApplicationService).IsAssignableFrom(type))
            //{
            //    return false;
            //}

            // 1. 检查类上是否有 DataPermissionAttribute
            if (type.GetTypeInfo().IsDefined(typeof(DataPermissionAttribute), true))
            {
                return true;
            }

            // 2. 检查是否有任何方法上有 DataPermissionAttribute
            if (type.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Any(m => m.IsDefined(typeof(DataPermissionAttribute), true)))
            {
                return true;
            }

            return false;
        }

    }
}
