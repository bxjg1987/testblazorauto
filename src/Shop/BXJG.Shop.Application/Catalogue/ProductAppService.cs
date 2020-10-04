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
        private readonly ProductManager itemManager;
        private readonly TempFileManager tempFileManager;

        public ProductAppService(IRepository<ProductEntity, long> repository,
                                      ProductCategoryManager dictionaryManager,
                                      ProductManager itemManager,
                                      TempFileManager tempFileManager)
        {
            this.repository = repository;
            this.dictionaryManager = dictionaryManager;
            this.itemManager = itemManager;
            this.tempFileManager = tempFileManager;
        }
        /// <summary>
        /// 创建并发布商品信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ProductDto> CreateAsync(ProductCreateDto input)
        {
            input.Images =(await this.tempFileManager.MoveAsync(input.Images)).Select(c=>c.FileRelativePath).ToArray();
            var entity = base.ObjectMapper.Map<ProductEntity>(input);
            entity = await repository.InsertAsync(entity);

            await CurrentUnitOfWork.SaveChangesAsync();

            //这里查两次，有点坑
            //await repository.EnsurePropertyLoadedAsync(entity, c => c.Category);
            //await repository.EnsurePropertyLoadedAsync(entity, c => c.Brand);

            entity = await repository.GetAllIncluding(c => c.Category, c => c.Brand, c => c.Unit).SingleAsync(c => c.Id == entity.Id);
            if (input.Published)
                entity.Publish(input.AvailableStart, input.AvailableEnd);
            //await itemManager.PublishAsync(entity, input.AvailableStart, input.AvailableEnd);

            return ObjectMapper.Map<ProductDto>(entity);
        }
        /// <summary>
        /// 更新商品信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ProductDto> UpdateAsync(ProductUpdateDto input)
        {
            input.Images = (await this.tempFileManager.MoveAsync(input.Images)).Select(c => c.FileRelativePath).ToArray();
            var entity = await this.repository.GetAsync(input.Id);
            ObjectMapper.Map<ProductUpdateDto, ProductEntity>(input, entity);
            if (input.Published)
                entity.Publish(input.AvailableStart, input.AvailableEnd);
            else
                entity.UnPublish();

            await CurrentUnitOfWork.SaveChangesAsync();
            entity = await repository.GetAllIncluding(c => c.Category, c => c.Brand, c => c.Unit).SingleAsync(c => c.Id == entity.Id);
            return ObjectMapper.Map<ProductDto>(entity);
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
                                                            || c.Category.DisplayName.Contains(input.Keywords)
                                                            || c.Specification.Contains(input.Keywords));

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
            foreach (var item in input.Ids)
            {
                try
                {
                    await repository.DeleteAsync(item);
                    result.Ids.Add(item);
                }
                catch (Exception ex)
                {
                    base.Logger.Warn($"删除商品档案失败，设备Id：{item}", ex);
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
            var entity = await repository.GetAsync(input.Id);
            return ObjectMapper.Map<ProductDto>(entity);
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
                Task t;
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
            //如果有问题，就每个明细await吧
            foreach (var item in entities)
            {
                item.UnPublish();
            }
        }
    }
}
