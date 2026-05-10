using Abp.Modules;
using Abp.Reflection.Extensions;
using System;
using System.Reflection;
using Abp.Dependency;
using Abp.Configuration.Startup;
using Abp.EntityHistory;
using BXJG.Utils.EFCore.EntityHistory;
using Abp.Domain.Uow;
//using DotNetCore.CAP.Transport;
//using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Castle.Core;
//using BXJG.Utils.EFCore.CAP;
using Abp.Application.Services;
using Abp.EntityFrameworkCore.Linq;
using Abp.EntityFrameworkCore;
using Abp.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Threading;
using Abp.Domain.Repositories;
using BXJG.Utils.Share.DataPermission;
namespace BXJG.Utils.EFCore
{
    [DependsOn(typeof(BXJGUtilsModule))]
    public class EFCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            // IocManager.Register<BXJGEFCoreConfiguration>();
            // 应用层去注册
            //IocManager.IocContainer.Kernel.ComponentRegistered += (key, handler) =>
            //{
            //    if (typeof(IApplicationService).IsAssignableFrom(handler.ComponentModel.Implementation) || typeof(ICapSubscribe).IsAssignableFrom(handler.ComponentModel.Implementation))
            //    {
            //        handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(AbpAsyncDeterminationInterceptor<AbpCapTranInterceptor>)));
            //    }
            //};


            //abp仓储的默认实现目前的删除是查询出来之后再删除，数据量大时有问题，已经提交了issue，这里是临时解决方式
            BXJG.Utils.Extensions.LinqExt.BatchDeleteImpl = (x,ct) => {
                //  x.GetType().GetMethods
                // Abp.Reflection.Extensions.TypeExtensions.
                //  RelationalQueryableExtensions.ExecuteDelete()

                //method = method.MakeGenericMethod(typeof(SomeClass));
                //var result = method.Invoke(service, null);
                
                return typeof(RelationalQueryableExtensions).GetMethod("ExecuteDeleteAsync", BindingFlags.Static | BindingFlags.Public).MakeGenericMethod(x.GetType().GetGenericArguments()[0]).Invoke(null,[ x,ct]) as Task<int>;
            };
        }
        public override void Initialize()
        {
            
            //貌似abp9.3解决了
       //  base.Configuration.ReplaceService< IAsyncQueryableExecuter ,sdfsdf >(DependencyLifeStyle.Transient);
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            // 应用层去注册
            //IocManager.Register(typeof(AbpAsyncDeterminationInterceptor<AbpCapTranInterceptor>), DependencyLifeStyle.Transient);
            Configuration.ReplaceService<IEntityHistoryHelper, BXJGEntityHistoryHelper>(DependencyLifeStyle.Transient);
        }

        //public override void PostInitialize()
        //{
        //    Configuration.ReplaceService<IEntityHistoryHelper, BXJGEntityHistoryHelper>(DependencyLifeStyle.Transient);
        //}
    }
    ///// <summary>
    ///// https://github.com/aspnetboilerplate/aspnetboilerplate/issues/6920
    ///// </summary>
    //public class sdfsdf : EfCoreAsyncQueryableExecuter,ISingletonDependency
    //{
    //    public ICancellationTokenProvider CancellationTokenProvider { get; set; }=NullCancellationTokenProvider.Instance;
    //    public Task<int> CountAsync<T>(IQueryable<T> queryable)
    //    {
    //        return queryable.CountAsync(CancellationTokenProvider.Token);
    //    }

    //    public Task<List<T>> ToListAsync<T>(IQueryable<T> queryable)
    //    {
    //        return queryable.ToListAsync(CancellationTokenProvider.Token);
    //    }

    //    public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> queryable)
    //    {
    //        return queryable.FirstOrDefaultAsync(CancellationTokenProvider.Token);
    //    }

    //    public Task<bool> AnyAsync<T>(IQueryable<T> queryable)
    //    {
    //        return queryable.AnyAsync(CancellationTokenProvider.Token);
    //    }
    //}
}
