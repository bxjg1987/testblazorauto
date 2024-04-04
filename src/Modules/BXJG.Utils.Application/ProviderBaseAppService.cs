using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Castle.Core;
using Abp.Dependency;
using Castle.MicroKernel.Lifestyle.Scoped;
using Castle.Windsor.MsDependencyInjection;
using Abp.Domain.Uow;
using Abp.Reflection.Extensions;

using BXJG.Utils.Application.Share;
using Abp.Threading;

namespace BXJG.Utils.Application
{
    /*
     * 一种数据除了curd功能等管理功能，还需要为其它功能提供可选数据（通常是下拉框或弹窗选择的数据）
     * 这种数据往往比管理功能中的详情拥有更少的属性，因为仅仅是用来选择，只要用户能准确识别即可。
     * 另外一般需要剔除敏感字段，如：成本价，因为是公共接口，很多功能都需要引用。
     * 具体定义多少字段，定义哪些字段，先得明确这个数据在整个项目中的功能范围，
     * 一般原则是在满足功能范围的情况下，且在用户能根据这些字段识别的情况下，提供尽可能少的字段，
     * 后期根据需求再加都可以
     * 因为一方面这样性能更高，也避免过多地暴露字段。
     * 
     * 无论如何定义，在特殊功能时可能都不够用，这里只是定义最通用的场景所需要的字段
     * 特殊场景应自己单独提供接口。
     * 
     * 简单的情况只需要登陆即可访问，但某些特殊的信息需要使用权限依赖。
     * 比如：商品种类，可以登陆即可访问， 客户资料需要依赖权限，防止供应商或竞争对手看到我司客户资料。
     * 
     * 总的来说，这个定义的前提是确定整个项目的功能范围。
     * 
     * 单独提供可选择的数据的接口不与crud接口放一起，因为它们的场景差别太大，一个是管理时的crud，一个是供别的功能选择的数据。
     * 
     * 这里是针对扁平数据，树形数据有自己的抽象。
     * 
     * 经过测试实现IIocManagerAccessor，定义IocManager属性（必须可写）拿到的容器是全局的
     * 从全局的IocManager拿其它服务还好，拿实现了IDisposable的服务就会有问题，会内存泄漏，已经测试过了。
     * https://learn.microsoft.com/zh-cn/dotnet/core/extensions/dependency-injection-guidelines#general-idisposable-guidelines
     * 
     * 经过测试发现，直接注入IServiceProvider，拿到的容器是局部的，估计是当前请求范围，不会内存泄漏。
     */

    /// <summary>
    /// 抽象的为其它功能提供可选数据的接口
    /// </summary>
    /// <typeparam name="TEntity">实体类</typeparam>
    /// <typeparam name="TKey">主键类型</typeparam>
    /// <typeparam name="TGetAllInput">查询时输入参数的类型</typeparam>
    /// <typeparam name="TEntityDto">可选数据的dto</typeparam>
    [UnitOfWork(false)]
    public abstract class ProviderBaseAppService<TEntity, TGetAllInput, TEntityDto, TKey> : BXJGUtilsBaseAppService, IProviderBaseAppService<TGetAllInput, TEntityDto, TKey>
        where TEntity : class, IEntity<TKey>
    {
        protected virtual string GetAllPermissionName { get; set; }

        //public virtual ICancellationTokenProvider CancellationTokenProvider { get; set; } = NullCancellationTokenProvider.Instance;

        public IRepository<TEntity, TKey> Repository { get; set; }
        /// <summary>
        /// 与当前请求关联的服务容器
        /// 通常你可以使用构造函数或属性注入，框架级别或特殊情况可以使用此对象。
        /// 注：IocManager是全局单例，解析实现IDisposeable的服务时比较危险，此时应使用ServiceProvider
        /// </summary>
        public IServiceProvider ServiceProvider { get; set; }

        ////AsyncCrudAppService

        ////使用反模式，消除子类的构造函数
        //public ProviderBaseAppService(/*IRepository<TEntity, TKey> repository*/)
        //{
        //    //this.Repository = repository;
        //  //  AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        //}
        /// <summary>
        /// 获取可供选择的数据
        /// 通常用来提供下拉框或弹窗选择的数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<PagedResultDto<TEntityDto>> GetAllAsync(TGetAllInput input)
        {
            await CheckGetAllPermission();

            var query = CreateFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<TEntityDto>(
                totalCount,
                entities.Select(MapToEntityDto).ToList()
            );
        }

        protected virtual async Task CheckPermission(string permissionName)
        {
            if (!string.IsNullOrEmpty(permissionName))
            {
                await PermissionChecker.AuthorizeAsync(permissionName);
                // PermissionChecker.Authorize(permissionName);
            }
        }
        /// <summary>
        /// 判断查询下拉或弹窗或关联的单个信息的详情的 权限，获取列表和单个都会调用
        /// </summary>
        /// <returns></returns>
        protected virtual Task CheckGetAllPermission()
        {
            return CheckPermission(GetAllPermissionName);
        }
        /// <summary>
        /// Should apply sorting if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected virtual IQueryable<TEntity> ApplySorting(IQueryable<TEntity> query, TGetAllInput input)
        {
            //Try to sort query if available
            var sortInput = input as ISortedResultRequest;
            if (sortInput != null)
            {
                if (!sortInput.Sorting.IsNullOrWhiteSpace())
                {
                    return query.OrderBy(sortInput.Sorting);
                }
            }

            //IQueryable.Task requires sorting, so we should sort if Take will be used.
            if (input is ILimitedResultRequest)
            {
                return query.OrderByDescending(e => e.Id);
            }

            //No sorting
            return query;
        }

        /// <summary>
        /// 获取列表时分页处理
        /// </summary>
        /// <param name="query"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> ApplyPaging(IQueryable<TEntity> query, TGetAllInput input)
        {
            //Try to use paging if available
            var pagedInput = input as IPagedResultRequest;
            if (pagedInput != null)
            {
                return query.PageBy(pagedInput);
            }

            //Try to limit query result if available
            var limitedInput = input as ILimitedResultRequest;
            if (limitedInput != null)
            {
                return query.Take(limitedInput.MaxResultCount);
            }

            //No paging
            return query;
        }

        /// <summary>
        /// 获取列表时调用，通常重写它来自定义条件过滤
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> CreateFilteredQuery(TGetAllInput input)
        {
            var q = BuildQuery();
            //获取列表时不希望未启用的项显示，但获取单个时不限制，因为可能希望找到这个数据，所以条件放这里
            if (typeof(TEntity).IsImplementInterface<IPassivable>())
            {
                q = q.Where("IsActive == @0", true);
            }
            if (input is IHaveFilter p)
            {
                q = q.ApplyDynamicCondtion(p.Filter);

            }
            return q;
        }

        /// <summary>
        /// 获取单个或列表时都会调用，通常重写它来自定义映射，默认使用AutoMapper
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual TEntityDto MapToEntityDto(TEntity entity)
        {
            return ObjectMapper.Map<TEntityDto>(entity);
        }

        /// <summary>
        /// 获取单个或列表时都会调用的方法，以获取查询对象
        /// 通常重写它来应用Include
        /// </summary>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> BuildQuery()
        {
            return Repository.GetAll().AsNoTrackingWithIdentityResolution();
        }

        /// <summary>
        /// 重写以应用include
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> GetById(TKey id)
        {
            return BuildQuery().Where(c => c.Id.Equals(id));
        }

        public virtual async Task<TEntityDto> Get(EntityDto<TKey> input)
        {
            await CheckGetAllPermission();
            var query = GetById(input.Id);
            var enitity = await AsyncQueryableExecuter.FirstOrDefaultAsync(query);
            return MapToEntityDto(enitity);
        }
    }
}