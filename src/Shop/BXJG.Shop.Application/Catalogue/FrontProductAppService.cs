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
using Abp.DynamicEntityProperties;

namespace BXJG.Shop.Catalogue
{
    public class FrontProductAppService : AbpServiceBase, IFrontProductAppService
    {
        private readonly IRepository<ProductEntity, long> repository;
        private readonly ProductCategoryManager dictionaryManager;
        private readonly IRepository<DynamicPropertyValue,long> repository1;
        public FrontProductAppService(IRepository<ProductEntity, long> repository, ProductCategoryManager dictionaryManager, IRepository<DynamicPropertyValue, long> repository1)
        {
            this.repository = repository;
            this.dictionaryManager = dictionaryManager;
            this.repository1 = repository1;
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
        /// <summary>
        /// 获取单个商品，包括相关外键属性，及其sku和每个sku关联的动态属性
        /// </summary>
        /// <param name="input">商品id</param>
        /// <returns></returns>
        public async Task<FrontProductDto> GetAsync(EntityDto<long> input)
        {
            var entity = await repository.GetAllIncluding(c => c.Category, c => c.Brand, c => c.Unit, c => c.Skus)
              //.AsNoTracking()
              .Where(c => c.Id == input.Id)
                //.Include(c => c.Skus).ThenInclude(c => c.DynamicEntityProperty1)
                //.Include(c => c.Skus).ThenInclude(c => c.DynamicEntityProperty2)
                //.Include(c => c.Skus).ThenInclude(c => c.DynamicEntityProperty3)
                //.Include(c => c.Skus).ThenInclude(c => c.DynamicEntityProperty4)
                //.Include(c => c.Skus).ThenInclude(c => c.DynamicEntityProperty5)

              //上面的加载DynamicPropertyValues性能不好，但是efcore5才开始支持以下写法；包括AsSignleQuery
              //.Include(c => c.Skus).ThenInclude(c => c.DynamicEntityProperty1.DynamicProperty.DynamicPropertyValues.SingleOrDefault(d => d.Id.ToString() == c.DynamicEntityProperty1Value))
              //.Include(c => c.Skus).ThenInclude(c => c.DynamicEntityProperty2.DynamicProperty.DynamicPropertyValues.SingleOrDefault(d => d.Id.ToString() == c.DynamicEntityProperty2Value))
              //.Include(c => c.Skus).ThenInclude(c => c.DynamicEntityProperty3.DynamicProperty.DynamicPropertyValues.SingleOrDefault(d => d.Id.ToString() == c.DynamicEntityProperty3Value))
              //.Include(c => c.Skus).ThenInclude(c => c.DynamicEntityProperty4.DynamicProperty.DynamicPropertyValues.SingleOrDefault(d => d.Id.ToString() == c.DynamicEntityProperty4Value))
              //.Include(c => c.Skus).ThenInclude(c => c.DynamicEntityProperty5.DynamicProperty.DynamicPropertyValues.SingleOrDefault(d => d.Id.ToString() == c.DynamicEntityProperty5Value))
              .SingleAsync();

            //var sdfff = entity.Skus.SelectMany(c => new HashSet<int?> {
            //    c.DynamicEntityProperty1?.DynamicPropertyId,
            //    c.DynamicEntityProperty2?.DynamicPropertyId ,
            //    c.DynamicEntityProperty3?.DynamicPropertyId,
            //    c.DynamicEntityProperty4?.DynamicPropertyId,
            //    c.DynamicEntityProperty5?.DynamicPropertyId})
            //    .Where(c => c.HasValue)
            //    .Distinct()
            //    .ToArray();

            //var aaa = await repository1
            //    .GetAllIncluding(c => c.DynamicProperty)
            //    //.AsNoTracking()
            //    .Where(c => sdfff.Contains(c.DynamicPropertyId))
            //    .ToListAsync();

            var dto = ObjectMapper.Map<FrontProductDto>(entity);
            dto.Skus = dto.Skus
                .OrderBy(c => c.DynamicEntityProperty1Value)
                .ThenBy(c => c.DynamicEntityProperty2Value)
                .ThenBy(c => c.DynamicEntityProperty3Value)
                .ThenBy(c => c.DynamicEntityProperty4Value)
                .ThenBy(c => c.DynamicEntityProperty5Value)
                .ToList();
            //这里暂时用土办法，最好的办法是一次性查询多个sku的动态属性值
            //foreach (var item in entity.Skus)
            //{
            //    //单个sku动态属性值
            //    var val = await dynamicEntityPropertyValueManager.GetValuesAsync<SkuEntity, long>(item.Id.ToString());

            //    //单个sku
            //    var sku = dto.Skus.Single(c => c.Id == item.Id);
            //    sku.DynamicEntityPropertyValues = new Dictionary<int, string>();
            //    foreach (var item2 in val)
            //    {
            //        sku.DynamicEntityPropertyValues.Add(item2.DynamicEntityPropertyId, item2.Value);
            //    }
            //}

            return dto;
        }
    }
}
