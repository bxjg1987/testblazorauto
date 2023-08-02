using Abp.Authorization;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.App.Common
{
    /*
     * 一种数据除了curd功能等管理功能，还需要为其它功能提供可选数据（通常是下拉框或弹窗选择的数据）
     * 这种数据往往比管理功能中的详情拥有更少的属性，因为仅仅是用来选择，只要用户能准确选择即可。
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
     */

    /// <summary>
    /// 抽象的为其它功能提供可选数据的接口
    /// </summary>
    /// <typeparam name="TEntity">实体类</typeparam>
    /// <typeparam name="TKey">主键类型</typeparam>
    /// <typeparam name="TGetAllInput">查询时输入参数的类型</typeparam>
    /// <typeparam name="TEntityDto">可选数据的dto</typeparam>
    public abstract class AsyncProviderCommonBaseAppService<TEntity,TKey, TGetAllInput, TEntityDto> : CommonBaseApplicationService where TEntity:class,IEntity<TKey>
    {
        protected readonly IRepository<TEntity, TKey> Repository;

        protected AsyncProviderCommonBaseAppService(IRepository<TEntity, TKey> repository)
        {
            this.Repository = repository;
        }

        public virtual async Task<PagedResultDto<TEntityDto>> GetAllAsync(TGetAllInput input)
        {
            CheckGetAllPermission();

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
        protected virtual void CheckPermission(string permissionName)
        {
            if (!string.IsNullOrEmpty(permissionName))
            {
                PermissionChecker.Authorize(permissionName);
            }
        }
        protected virtual void CheckGetAllPermission()
        {
            CheckPermission(GetAllPermissionName);
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
        /// Should apply paging if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
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
        /// This method should create <see cref="IQueryable{TEntity}"/> based on given input.
        /// It should filter query if needed, but should not do sorting or paging.
        /// Sorting should be done in <see cref="ApplySorting"/> and paging should be done in <see cref="ApplyPaging"/>
        /// methods.
        /// </summary>
        /// <param name="input">The input.</param>
        protected virtual IQueryable<TEntity> CreateFilteredQuery(TGetAllInput input)
        {
            return Repository.GetAll();
        }

        protected virtual string GetAllPermissionName { get; set; }
        protected virtual TEntityDto MapToEntityDto(TEntity entity)
        {
            return ObjectMapper.Map<TEntityDto>(entity);
        }
    }
}
