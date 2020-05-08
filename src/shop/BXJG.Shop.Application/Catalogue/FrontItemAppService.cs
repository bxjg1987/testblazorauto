using Abp.Application.Services;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using BXJG.Shop.Catalogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Abp.Linq.Extensions;
using Abp.ObjectMapping;
using Microsoft.EntityFrameworkCore;
using Abp;

namespace BXJG.Shop.Catalogue
{
    public class BXJGShopFrontItemAppService : AbpServiceBase, IBXJGShopFrontItemAppService
    {
        private readonly IRepository<ItemEntity, long> repository;

        public BXJGShopFrontItemAppService(IRepository<ItemEntity, long> repository)
        {
            this.repository = repository;
        }
        public async Task<List<FrontItemDto>> GetAllAsync(GetAllFrontItemInput input)
        {
            var now = DateTimeOffset.Now;
            var query = repository.GetAllIncluding(c => c.Category)
                   .WhereIf(input.BrandId.HasValue, c => c.BrandId == input.BrandId.Value)
                   .WhereIf(input.CategoryId.HasValue, c => c.CategoryId == input.CategoryId.Value)
                   .Where(c => c.Published && (!c.AvailableStart.HasValue || c.AvailableStart.Value <= now) && (!c.AvailableEnd.HasValue || c.AvailableEnd.Value > now))
                   .WhereIf(input.PriceMin.HasValue, c => c.Price >= input.PriceMin.Value)
                   .WhereIf(input.PriceMax.HasValue, c => c.Price < input.PriceMax.Value)
                   .WhereIf(input.Hot.HasValue, c => c.Hot == input.Hot)
                   .WhereIf(input.New.HasValue, c => c.New == input.New)
                   .WhereIf(input.Home.HasValue, c => c.Home == input.Home)
                   .WhereIf(input.Focus.HasValue, c => c.Focus == input.Focus)
                   .WhereIf(!input.Keywords.IsNullOrEmpty(), c => c.Title.Contains(input.Keywords) || c.Sku.Contains(input.Keywords))
                   .OrderBy(input.Sorting)
                   .PageBy(input);

            var list = await query.ToListAsync();
            return ObjectMapper.Map<List<FrontItemDto>>(list);
        }
    }
}
