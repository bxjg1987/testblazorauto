using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Expressions;
using Abp.Linq.Extensions;
using BXJG.Common.Dto;
using BXJG.Utils.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace BXJG.GoodsInfo.Application.Common
{
    /// <summary>
    /// 公共的物品信息应用服务
    /// 用户登陆即可访问
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    /// <typeparam name="TGoodsInfoGetTotalInput"></typeparam>
    /// <typeparam name="TGetForSelectInput"></typeparam>
    /// <typeparam name="TRepository"></typeparam>
    /// <typeparam name="TQueryTemp"></typeparam>
    [AbpAuthorize]
    public class BXJGGoodsInfoCommonAppService<TEntity, TDto, TGoodsInfoGetTotalInput, TGetForSelectInput, TRepository, TQueryTemp> : AppServiceBase
        where TEntity : class,IGoodsInfoEntity
        where TDto : GoodsInfoDto
        where TGoodsInfoGetTotalInput : GoodsInfoGetTotalInput, new()
        where TGetForSelectInput : GetGoodsInfoForSelectInput<TGoodsInfoGetTotalInput>
        where TRepository : IGoodsInfoRepository<TEntity>
        where TQueryTemp : QueryTemp<TEntity>, new()
    {
        protected readonly TRepository repository;
        protected readonly IRepository<GoodsInfoCategoryEntity, long> clsRepository;

        public BXJGGoodsInfoCommonAppService(TRepository repository, IRepository<GoodsInfoCategoryEntity, long> clsRepository)
        {
            this.repository = repository;
            this.clsRepository = clsRepository;
        }
        /// <summary>
        /// 获取符合条件的物品数量
        /// </summary>
        /// <returns></returns>
        [UnitOfWork(false)]
        public virtual async Task<long> GetTotalAsync(TGoodsInfoGetTotalInput input)
        {
            var query = await FilterAsync(input);
            return await query.CountAsync();
        }
        /// <summary>
        /// 获取物品信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(false)]
        public virtual async Task<PagedResultDto<TDto>> GetGetForSelectAsync(TGetForSelectInput input)
        {
            var query = await FilterAsync(input.GetTotalInput);
            var total = await query.CountAsync();
            query = await PageByAsync(query, input);
            query = await OrderByAsync(query, input);
            var list = await query.ToListAsync();
            var dtos = new List<TDto>();
            foreach (var item in list)
            {
                var dto = await this.Entity2DtoAsync(item);
                dtos.Add(dto);
            }
            return new PagedResultDto<TDto>(total, dtos);
        }
        /// <summary>
        /// 实体映射为dto。
        /// 默认使用ObjectMapper.Map(entity)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected virtual ValueTask<TDto> Entity2DtoAsync(TQueryTemp item)
        {
            var dto = ObjectMapper.Map<TDto>(item.Entity);
            return ValueTask.FromResult(dto);
        }
        /// <summary>
        /// 通过查询条件生成查询对象
        /// 默认include分类、AsNoTrackingWithIdentityResolution、关键字模糊查询
        /// 你可以重写，应用更多include和条件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual async ValueTask<IQueryable<TQueryTemp>> FilterAsync(TGoodsInfoGetTotalInput input)
        {
            var query = from c in repository.GetAll().Include(c => c.Category).AsNoTrackingWithIdentityResolution()
                        select new TQueryTemp { Entity = c };
            return query.WhereIf(!input.Keywords.IsNullOrWhiteSpace(), await ApplyKeywordsAsync(input.Keywords))
                        .WhereIf(!input.CategoryCode.IsNullOrWhiteSpace(), c => c.Entity.Category.Code.StartsWith(input.CategoryCode))
                        .WhereIf(!input.BrandId.IsNullOrWhiteSpace(), c => c.Entity.BrandId == input.BrandId);
        }
        /// <summary>
        /// 应用关键字条件。
        /// 默认模糊查询物品名称、助记码、分类名称等，
        /// 你可以重写，调用base.ApplyKeywordsAsync后，通过扩展方法Or应用更多字段的模糊查询
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        protected virtual ValueTask<Expression<Func<TQueryTemp, bool>>> ApplyKeywordsAsync(string keywords)
        {
            Expression<Func<TQueryTemp, bool>> where = c => c.Entity.Name.Contains(keywords) ||
                                                            c.Entity.MnemonicCode.Contains(keywords) ||
                                                            c.Entity.Category.DisplayName.Contains(keywords);
            return ValueTask.FromResult(where);
        }
        /// <summary>
        /// 应用分页。
        /// 默认： query.PageBy(input);
        /// </summary>
        /// <param name="query"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual ValueTask<IQueryable<TQueryTemp>> PageByAsync(IQueryable<TQueryTemp> query, TGetForSelectInput input)
        {
            query = query.PageBy(input);
            return ValueTask.FromResult(query);
        }
        /// <summary>
        /// 应用排序。
        /// 默认：query.OrderBy(input.Sorting);
        /// </summary>
        /// <param name="query"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual ValueTask<IQueryable<TQueryTemp>> OrderByAsync(IQueryable<TQueryTemp> query, TGetForSelectInput input)
        {
            query = query.OrderBy(input.Sorting);
            return ValueTask.FromResult(query);
        }
    }
}
