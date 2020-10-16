using Abp.Application.Services;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Linq;
using Abp.MultiTenancy;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Linq.Extensions;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic;
using Abp.Extensions;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.Linq.Extensions;
using BXJG.Utils.File;
using BXJG.Common.Dto;
using Abp;
using Abp.DynamicEntityProperties;
using Abp.DynamicEntityProperties.Extensions;
using Abp.Runtime.Session;

namespace BXJG.Shop.Catalogue
{
    /*
     * 设计此功能时还没有引用Microsoft.EntityFrameworkCore，它包含很多好用的扩展方法，使用的AsyncQueryableExecuter + system.linq.dynamic 的方式
     * 而abp zero是直接引用的它
     * 可以考虑直接引用Microsoft.EntityFrameworkCore
     */

    /// <summary>
    /// 商品上架信息应用服务
    /// </summary>
    public class ProductAppService : AbpServiceBase, IProductAppService
    {
        private readonly IRepository<ProductEntity, long> repository;
        private readonly ProductCategoryManager dictionaryManager;
        private readonly ProductManager productManager;
        private readonly TempFileManager tempFileManager;

        //private readonly IDynamicPropertyManager dynamicPropertyManager;
        //private readonly IDynamicPropertyValueManager dynamicPropertyValueManager;
        private readonly IDynamicEntityPropertyManager dynamicEntityPropertyManager;
        private readonly IDynamicEntityPropertyValueManager dynamicEntityPropertyValueManager;

        private readonly IAbpSession abpSession;

        public ProductAppService(IRepository<ProductEntity, long> repository,
                                 ProductCategoryManager dictionaryManager,
                                 ProductManager itemManager,
                                 TempFileManager tempFileManager,
                                 IDynamicEntityPropertyManager dynamicEntityPropertyManager,
                                 IDynamicEntityPropertyValueManager dynamicEntityPropertyValueManager,
                                 IAbpSession abpSession)
        {
            this.repository = repository;
            this.dictionaryManager = dictionaryManager;
            this.productManager = itemManager;
            this.tempFileManager = tempFileManager;
            this.dynamicEntityPropertyManager = dynamicEntityPropertyManager;
            this.dynamicEntityPropertyValueManager = dynamicEntityPropertyValueManager;
            this.abpSession = abpSession;
        }
        /// <summary>
        /// 创建并发布商品信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ProductDto> CreateAsync(ProductUpdateDto input)
        {
            //将图片从临时目录移动到正式目录
            input.Images = (await this.tempFileManager.MoveAsync(input.Images)).Select(c => c.FileRelativePath).ToArray();
            var entity = base.ObjectMapper.Map<ProductEntity>(input);

            //保存
            entity = await repository.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            //这里查两次，有点坑
            //await repository.EnsurePropertyLoadedAsync(entity, c => c.Category);
            //await repository.EnsurePropertyLoadedAsync(entity, c => c.Brand);

            //处理sku。这里使用顺序不是很严谨
            //var dep = await dynamicEntityPropertyManager.GetAllAsync<SkuEntity, long>();        //获取与SkuEntity关联的动实体态属性集合
            //for (int i = 0; i < entity.Skus.Count; i++)
            //{
            //    var skuInput = input.Skus[i];    //用户提交的 动态实体属性id和值
            //    var sku = entity.Skus[i];
            //    foreach (var dynamicEntityPropertyValue in skuInput.DynamicEntityPropertyValues)
            //    {
            //        var dynamicEntityProperty = dep.Single(c => c.Id == dynamicEntityPropertyValue.Key);
            //        await dynamicEntityPropertyValueManager.AddAsync(new DynamicEntityPropertyValue(dynamicEntityProperty, sku.Id.ToString(), dynamicEntityPropertyValue.Value, abpSession.TenantId));
            //    }
            //}

            //发布
            entity = await repository.GetAllIncluding(c => c.Category, c => c.Brand, c => c.Unit, c => c.Skus).SingleAsync(c => c.Id == entity.Id);
            if (input.Published)
                entity.Publish(input.AvailableStart, input.AvailableEnd);
            //await itemManager.PublishAsync(entity, input.AvailableStart, input.AvailableEnd);

            //return ObjectMapper.Map<ProductDto>(entity);
            return await GetOneAsync(entity.Id);//这里又去查，性能不太好
        }
        /// <summary>
        /// 更新商品信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ProductDto> UpdateAsync(ProductUpdateDto input)
        {
            //获取原来的实体
            var entity = await this.repository.GetAllIncluding(c => c.Skus).SingleAsync(c => c.Id == input.Id);

            ////删除原来的sku
            //foreach (var item in entity.Skus)
            //{
            //    //单个sku动态属性值
            //    var val = await dynamicEntityPropertyValueManager.GetValuesAsync<SkuEntity, long>(item.Id.ToString());
            //    foreach (var item1 in val)
            //    {
            //        //await dynamicEntityPropertyValueManager.CleanValues(3, "");
            //        await dynamicEntityPropertyValueManager.DeleteAsync(item1.Id);
            //    }
            //}
            //entity.Skus.Clear();
            //await CurrentUnitOfWork.SaveChangesAsync();

            //将图片从临时目录移动到正式目录
            input.Images = (await this.tempFileManager.MoveAsync(input.Images)).Select(c => c.FileRelativePath).ToArray();
            //更新现有属性
            ObjectMapper.Map(input, entity);

            //发布处理
            if (input.Published)
                entity.Publish(input.AvailableStart, input.AvailableEnd);
            else
                entity.UnPublish();

            //保存下以生成sku的自增id
            await CurrentUnitOfWork.SaveChangesAsync();

            //添加新的sku
            //var dep = await dynamicEntityPropertyManager.GetAllAsync<SkuEntity, long>();        //获取与SkuEntity关联的动实体态属性集合
            //for (int i = 0; i < entity.Skus.Count; i++)
            //{
            //    var skuInput = input.Skus[i];    //用户提交的 动态实体属性id和值
            //    var sku = entity.Skus[i];
            //    foreach (var dynamicEntityPropertyValue in skuInput.DynamicEntityPropertyValues)
            //    {
            //        var dynamicEntityProperty = dep.Single(c => c.Id == dynamicEntityPropertyValue.Key);
            //        await dynamicEntityPropertyValueManager.AddAsync(new DynamicEntityPropertyValue(dynamicEntityProperty, sku.Id.ToString(), dynamicEntityPropertyValue.Value, abpSession.TenantId));
            //    }
            //}

            //entity = await repository.GetAllIncluding(c => c.Category, c => c.Brand, c => c.Unit, c => c.Skus).SingleAsync(c => c.Id == entity.Id);
            //return ObjectMapper.Map<ProductDto>(entity);
            return await GetOneAsync(entity.Id);//这里又去查，性能不太好
        }
        /// <summary>
        /// 管理页面用来获取所有商品信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ProductDto>> GetAllAsync(GetAllProductInput input)
        {
            string clsCode = input.CategoryCode;
            if (clsCode.IsNullOrWhiteSpace() && input.CategoryId.HasValue)
                clsCode = await dictionaryManager.GetCodeAsync(input.CategoryId.Value);

            var query = repository.GetAllIncluding(c => c.Category, c => c.Brand, c => c.Unit).AsNoTracking()
                .WhereIf(input.BrandId.HasValue, c => c.BrandId == input.BrandId.Value)
                .WhereIf(!clsCode.IsNullOrWhiteSpace(), c => c.Category.Code.StartsWith(clsCode))
                .WhereIf(input.Published.HasValue, c => c.Published == input.Published.Value)
                .WhereIf(input.AvailableStart.HasValue, c => c.AvailableStart >= input.AvailableStart.Value)
                .WhereIf(input.AvailableEnd.HasValue, c => c.AvailableEnd < input.AvailableEnd.Value)
                .WhereIf(!input.Keywords.IsNullOrEmpty(), c => c.Title.Contains(input.Keywords)
                                                            || c.DescriptionShort.Contains(input.Keywords)
                                                            || c.Brand.DisplayName.Contains(input.Keywords)
                                                            || c.Category.DisplayName.Contains(input.Keywords));

            var count = await query.CountAsync();
            var list = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();
            return new PagedResultDto<ProductDto>(count, ObjectMapper.Map<IReadOnlyList<ProductDto>>(list));
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<BatchOperationResultLong> DeleteAsync(BatchOperationInputLong input)
        {
            var result = new BatchOperationResultLong();
            foreach (var id in input.Ids)
            {
                try
                {
                    //var entity = await this.repository.GetAllIncluding(c => c.Skus).SingleAsync(c => c.Id == id);

                    //删除原来的sku
                    //foreach (var item in entity.Skus)
                    //{
                    //    //单个sku动态属性值
                    //    var val = await dynamicEntityPropertyValueManager.GetValuesAsync<SkuEntity, long>(item.Id.ToString());
                    //    foreach (var item1 in val)
                    //    {
                    //        //await dynamicEntityPropertyValueManager.CleanValues(3, "");
                    //        await dynamicEntityPropertyValueManager.DeleteAsync(item1.Id);
                    //    }
                    //}
                    //entity.Skus.Clear();//这里应该是有级联删除的
                    await repository.DeleteAsync(id);
                    result.Ids.Add(id);
                }
                catch (Exception ex)
                {
                    base.Logger.Warn($"删除商品档案失败，设备Id：{id}", ex);
                }
            }
            return result;
        }
        /// <summary>
        /// 根据id获取商品信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ProductDto> GetAsync(EntityDto<long> input)
        {
            return await GetOneAsync(input.Id);
        }
        /// <summary>
        /// 批量发布商品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task PublishAsync(BatchPublishInput input)
        {
            var entities = await repository.GetAllListAsync(d => input.Ids.Contains(d.Id));
            foreach (var item in entities)
            {
                if (input.AvailableEndSeconds != default)
                    item.PublishDuration(input.AvailableStart, input.AvailableEndSeconds.Value);
                else
                    item.Publish(input.AvailableStart, input.AvailableEnd);
            }
        }
        /// <summary>
        /// 批量取消商品的发布
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UnPublishAsync(BatchOperationInputLong input)
        {
            var entities = await repository.GetAllListAsync(d => input.Ids.Contains(d.Id));
            foreach (var item in entities)
            {
                item.UnPublish();
            }
        }
        /// <summary>
        /// 获取单个商品，包括相关外键属性，及其sku和每个sku关联的动态属性
        /// </summary>
        /// <param name="id">商品id</param>
        /// <returns></returns>
        private async Task<ProductDto> GetOneAsync(long id)
        {
            var entity = await repository.GetAllIncluding(c => c.Category, c => c.Brand, c => c.Unit)
                .Include(c => c.Skus).ThenInclude(c => c.DynamicEntityProperty1).ThenInclude(c => c.DynamicProperty)
                .Include(c => c.Skus).ThenInclude(c => c.DynamicEntityProperty2).ThenInclude(c => c.DynamicProperty)
                .Include(c => c.Skus).ThenInclude(c => c.DynamicEntityProperty3).ThenInclude(c => c.DynamicProperty)
                .Include(c => c.Skus).ThenInclude(c => c.DynamicEntityProperty4).ThenInclude(c => c.DynamicProperty)
                .Include(c => c.Skus).ThenInclude(c => c.DynamicEntityProperty5).ThenInclude(c => c.DynamicProperty)
                .SingleAsync(c => c.Id == id);
            var dto = ObjectMapper.Map<ProductDto>(entity);

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
