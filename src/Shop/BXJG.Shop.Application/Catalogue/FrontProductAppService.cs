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
using Abp.Application.Services.Dto;
using BXJG.GeneralTree;

namespace BXJG.Shop.Catalogue
{
    public class FrontProductAppService : AbpServiceBase, IFrontProductAppService
    {
        private readonly IRepository<ProductEntity, long> repository;
        private readonly ProductCategoryManager dictionaryManager;

        public FrontProductAppService(IRepository<ProductEntity, long> repository, ProductCategoryManager dictionaryManager)
        {
            this.repository = repository;
            this.dictionaryManager = dictionaryManager;
        }
        public async Task<PagedResultDto<FrontProductDto>> GetAllAsync(GetAllFrontProductInput input)
        {
            string clsCode = input.CategoryCode;
            if (clsCode.IsNullOrWhiteSpace() && input.CategoryId.HasValue)
                clsCode = await dictionaryManager.GetCodeAsync(input.CategoryId.Value);

            var now = DateTimeOffset.Now;
            var query = repository.GetAllIncluding(c => c.Category,c=>c.Brand)
                   .WhereIf(input.BrandId.HasValue, c => c.BrandId == input.BrandId.Value)
                   .WhereIf(!clsCode.IsNullOrWhiteSpace(), c => c.Category.Code.StartsWith(clsCode))
                   .Where(c => c.Published && (!c.AvailableStart.HasValue || c.AvailableStart.Value <= now) && (!c.AvailableEnd.HasValue || c.AvailableEnd.Value > now))
                   .WhereIf(input.PriceMin.HasValue, c => c.Price >= input.PriceMin.Value)
                   .WhereIf(input.PriceMax.HasValue, c => c.Price < input.PriceMax.Value)
                   .WhereIf(input.Hot.HasValue, c => c.Hot == input.Hot)
                   .WhereIf(input.New.HasValue, c => c.New == input.New)
                   .WhereIf(input.Home.HasValue, c => c.Home == input.Home)
                   .WhereIf(input.Focus.HasValue, c => c.Focus == input.Focus)
                   .WhereIf(!input.Keywords.IsNullOrEmpty(), c => c.Title.Contains(input.Keywords)
                                                               || c.DescriptionShort.Contains(input.Keywords)
                                                               || c.Brand.DisplayName.Contains(input.Keywords)
                                                               || c.Category.DisplayName.Contains(input.Keywords));
            var count = await query.CountAsync();

            var list = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();

            return new PagedResultDto<FrontProductDto>(count, ObjectMapper.Map<IReadOnlyList<FrontProductDto>>(list));
        }
    }
}
