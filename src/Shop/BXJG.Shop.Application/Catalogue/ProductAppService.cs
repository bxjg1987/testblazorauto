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
using Microsoft.VisualBasic;

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
        private readonly IRepository<DynamicPropertyValue> repository1;
        private readonly IRepository<DynamicEntityProperty> repository2;

        private readonly IAbpSession abpSession;

        public ProductAppService(IRepository<ProductEntity, long> repository,
                                 ProductCategoryManager dictionaryManager,
                                 ProductManager itemManager,
                                 TempFileManager tempFileManager,
                                 IDynamicEntityPropertyManager dynamicEntityPropertyManager,
                                 IDynamicEntityPropertyValueManager dynamicEntityPropertyValueManager,
                                 IAbpSession abpSession, IRepository<DynamicPropertyValue> repository1,
                                 IRepository<DynamicEntityProperty> repository2)
        {
            this.repository = repository;
            this.dictionaryManager = dictionaryManager;
            this.productManager = itemManager;
            this.tempFileManager = tempFileManager;
            this.dynamicEntityPropertyManager = dynamicEntityPropertyManager;
            this.dynamicEntityPropertyValueManager = dynamicEntityPropertyValueManager;
            this.abpSession = abpSession;
            this.repository1 = repository1;
            this.repository2 = repository2;
        }
        /// <summary>
        /// 创建并发布商品信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ProductDto> CreateAsync(ProductUpdateDto input)
        {
            //***********将来考虑将部分逻辑封装到领域服务中************

            //将图片从临时目录移动到正式目录
            input.Images = (await this.tempFileManager.MoveAsync(input.Images)).Select(c => c.FileRelativePath).ToArray();

            //处理详细描述中的图片，未考虑异常情况，先用着吧
            input.DescriptionFull = tempFileManager.ReplaceImagePath(input.DescriptionFull);
            await tempFileManager.MoveAsync(tempFileManager.GetMatchedImagePath(input.DescriptionFull).ToArray());

            //输入模型映射给实体模型赋值
            var entity = base.ObjectMapper.Map<ProductEntity>(input);

            #region 处理sku
            await NewMethod(entity);
            #endregion

            //保存
            entity = await repository.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();




            //发布
            if (input.Published)
                entity.Publish(input.AvailableStart, input.AvailableEnd);

            //entity = await repository.GetAllIncluding(c => c.Category, c => c.Brand, c => c.Unit, c => c.Skus).SingleAsync(c => c.Id == entity.Id);
            //return ObjectMapper.Map<ProductDto>(entity);
            return await GetOneAsync(entity.Id);//这里又去查，性能不太好
        }

        private async ValueTask NewMethod(ProductEntity entity)
        {
            if (entity.Skus.Count == 0)
                return;

            var dynamicEntityProperties = await repository2
                            .GetAllIncluding(c => c.DynamicProperty.DynamicPropertyValues)
                            .Where(c => c.EntityFullName == typeof(SkuEntity).FullName)
                            .ToListAsync();

            foreach (var item in entity.Skus)
            {
                var dp1 = dynamicEntityProperties.SingleOrDefault(c => c.Id == item.DynamicEntityProperty1Id)?.DynamicProperty;
                if (dp1 != null)
                {
                    item.DynamicProperty1Name = dp1.PropertyName;
                    item.DynamicEntityProperty1Text = dp1.DynamicPropertyValues?.SingleOrDefault(c => c.Id.ToString() == item.DynamicEntityProperty1Value)?.Value;
                }
                var dp2 = dynamicEntityProperties.SingleOrDefault(c => c.Id == item.DynamicEntityProperty2Id)?.DynamicProperty;
                if (dp2 != null)
                {
                    item.DynamicProperty2Name = dp2.PropertyName;
                    item.DynamicEntityProperty2Text = dp2.DynamicPropertyValues?.SingleOrDefault(c => c.Id.ToString() == item.DynamicEntityProperty2Value)?.Value;
                }
                var dp3 = dynamicEntityProperties.SingleOrDefault(c => c.Id == item.DynamicEntityProperty3Id)?.DynamicProperty;
                if (dp3 != null)
                {
                    item.DynamicProperty3Name = dp3.PropertyName;
                    item.DynamicEntityProperty3Text = dp3.DynamicPropertyValues?.SingleOrDefault(c => c.Id.ToString() == item.DynamicEntityProperty3Value)?.Value;
                }
                var dp4 = dynamicEntityProperties.SingleOrDefault(c => c.Id == item.DynamicEntityProperty4Id)?.DynamicProperty;
                if (dp4 != null)
                {
                    item.DynamicProperty4Name = dp4.PropertyName;
                    item.DynamicEntityProperty4Text = dp4.DynamicPropertyValues?.SingleOrDefault(c => c.Id.ToString() == item.DynamicEntityProperty4Value)?.Value;
                }
                var dp5 = dynamicEntityProperties.SingleOrDefault(c => c.Id == item.DynamicEntityProperty5Id)?.DynamicProperty;
                if (dp5 != null)
                {
                    item.DynamicProperty5Name = dp5.PropertyName;
                    item.DynamicEntityProperty5Text = dp5.DynamicPropertyValues?.SingleOrDefault(c => c.Id.ToString() == item.DynamicEntityProperty5Value)?.Value;
                }
            }
        }

        /// <summary>
        /// 更新商品信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ProductDto> UpdateAsync(ProductUpdateDto input)
        {
            //***********将来考虑将部分逻辑封装到领域服务中************

            //目前下面的处理存在隐患。若移动或删除图片成功后，提交数据库更新失败，则图片文件将丢失。

            //获取原来的实体
            var entity = await this.repository.GetAllIncluding(c => c.Skus).SingleAsync(c => c.Id == input.Id);

            var oldImags = entity.GetImages().Select(c => c.Value);
            var needremove = oldImags.Except(input.Images);
            await tempFileManager.RemoveAsync(needremove.ToArray());
            //将图片从临时目录移动到正式目录
            input.Images = (await this.tempFileManager.MoveAsync(input.Images)).Select(c => c.FileRelativePath).ToArray();


            //处理详细描述中的图片，未考虑异常情况，先用着吧
            var tempDesFull = tempFileManager.GetMatchedImagePath(input.DescriptionFull);
            input.DescriptionFull = tempFileManager.ReplaceImagePath(input.DescriptionFull);
            await tempFileManager.MoveAsync(tempDesFull);

            var desnewFullImages = tempFileManager.GetMatchedImagePath(input.DescriptionFull);
            if (!entity.DescriptionFull.IsNullOrWhiteSpace())
            {
                var desFullImags = tempFileManager.GetMatchedImagePath(entity.DescriptionFull);
                var needremovedesFullImags = desFullImags.Except(desnewFullImages);
                await tempFileManager.RemoveAsync(needremovedesFullImags.ToArray());
            }


            //更新现有属性
            ObjectMapper.Map(input, entity);
            await NewMethod(entity);
            //发布处理
            if (input.Published)
                entity.Publish(input.AvailableStart, input.AvailableEnd);
            else
                entity.UnPublish();

            //保存下以生成sku的自增id
            await CurrentUnitOfWork.SaveChangesAsync();

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
                    var entity = await this.repository.GetAllIncluding(c => c.Skus).SingleAsync(c => c.Id == id);

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
                    await repository.DeleteAsync(entity);
                    await tempFileManager.RemoveAsync(entity.GetImages().Select(c => c.Value).ToArray());
                    result.Ids.Add(id);
                }
                catch (Exception ex)
                {
                    base.Logger.Warn($"删除商品档案失败，Id：{id}", ex);
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
            var entity = await repository.GetAllIncluding(c => c.Category, c => c.Brand, c => c.Unit, c => c.Skus)
                //.AsNoTracking()
                .Where(c => c.Id == id)
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

            var dto = ObjectMapper.Map<ProductDto>(entity);
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
