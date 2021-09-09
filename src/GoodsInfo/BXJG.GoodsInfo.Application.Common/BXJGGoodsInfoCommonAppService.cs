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
    public class BXJGGoodsInfoCommonAppService<TEntity, TDto, TGoodsInfoGetTotalInput, TGetForSelectInput, TRepository, TQueryTemp> : ApplicationService
        where TEntity : GoodsInfoEntity
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
        public virtual async Task<PagedResultDto<TDto>> GetAllAsync(TGetForSelectInput input)
        {
            throw new NotImplementedException();
        }

        protected virtual async ValueTask<IQueryable<TQueryTemp>> FilterAsync(TGoodsInfoGetTotalInput input)
        {
            var query = from c in repository.GetAll().Include(c => c.Category).AsNoTrackingWithIdentityResolution()
                        select new TQueryTemp { GoodsInfo = c };
            return query.WhereIf(!input.Keywords.IsNullOrWhiteSpace(), await ApplyKeywordsAsync(input.Keywords));
        }
        protected virtual ValueTask<Expression<Func<TQueryTemp, bool>>> ApplyKeywordsAsync(string keywords)
        {
            Expression<Func<TQueryTemp, bool>> where = c => c.GoodsInfo.Name.Contains(keywords) || c.GoodsInfo.Category.DisplayName.Contains(keywords);
            return ValueTask.FromResult(where);
        }
        protected virtual ValueTask<IQueryable<TQueryTemp>> PageBy(IQueryable<TQueryTemp> query, TGetForSelectInput input)
        {
            query = query.PageBy(input);
            return ValueTask.FromResult(query);
        }
        protected virtual ValueTask<IQueryable<TQueryTemp>> OrderBy(IQueryable<TQueryTemp> query, TGetForSelectInput input)
        {
            query = query.OrderBy(input.Sorting);
            return ValueTask.FromResult(query);
        }
    }
}
