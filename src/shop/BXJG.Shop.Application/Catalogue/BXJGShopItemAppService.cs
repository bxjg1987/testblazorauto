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
using BXJG.Shop.Common;

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
    /// <typeparam name="TTenant"></typeparam>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TRole"></typeparam>
    /// <typeparam name="TTenantManager"></typeparam>
    /// <typeparam name="TUserManager"></typeparam>
    public abstract class BXJGShopItemAppService<TTenant, TUser, TRole, TTenantManager, TUserManager>
        : BXJGShopAppServiceBase<TTenant, TUser, TRole, TTenantManager, TUserManager>, IBXJGShopItemAppService
        where TUser : AbpUser<TUser>
        where TRole : AbpRole<TUser>, new()
        where TTenant : AbpTenant<TUser>
        where TTenantManager : AbpTenantManager<TTenant, TUser>
        where TUserManager : AbpUserManager<TRole, TUser>
    {
        private readonly IRepository<ItemEntity, long> repository;
        private readonly BXJGShopDictionaryManager dictionaryManager;
        private readonly ItemManager itemManager;

        private readonly IRepository<BXJGShopDictionaryEntity, long> respDic;

        public BXJGShopItemAppService(IRepository<ItemEntity, long> repository, BXJGShopDictionaryManager dictionaryManager, IRepository<BXJGShopDictionaryEntity, long> respDic, ItemManager itemManager)
        {
            this.repository = repository;
            this.dictionaryManager = dictionaryManager;
            this.respDic = respDic;
            this.itemManager = itemManager;
        }
        /// <summary>
        /// 创建并发布商品信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ItemDto> CreateAsync(ItemCreateDto input)
        {
            var entity = base.ObjectMapper.Map<ItemEntity>(input);
            entity = await repository.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            //这里查两次，有点坑
            //await repository.EnsurePropertyLoadedAsync(entity, c => c.Category);
            //await repository.EnsurePropertyLoadedAsync(entity, c => c.Brand);

            entity = await repository.GetAllIncluding(c => c.Category, c => c.Brand).SingleAsync(c => c.Id == entity.Id);

            await itemManager.PublishAsync(entity, input.AvailableStart, input.AvailableEnd);
            return ObjectMapper.Map<ItemDto>(entity);
        }
        /// <summary>
        /// 更新商品信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ItemDto> UpdateAsync(ItemUpdateDto input)
        {
            var entity = await AsyncQueryableExecuter.FirstOrDefaultAsync(repository.GetAllIncluding(c => c.Category));
            ObjectMapper.Map<ItemUpdateDto, ItemEntity>(input, entity);
            return ObjectMapper.Map<ItemDto>(entity);
        }
        /// <summary>
        /// 管理页面用来获取所有商品信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ItemDto>> GetAllAsync(GetAllItemsInput input)
        {
            string clsCode = input.CategoryCode;
            if (clsCode.IsNullOrWhiteSpace() && input.CategoryId.HasValue)
                clsCode = await dictionaryManager.GetCodeAsync(input.CategoryId.Value);

            var query = repository.GetAllIncluding(c => c.Category, c => c.Brand).AsNoTracking()
                .WhereIf(input.BrandId.HasValue, c => c.BrandId == input.BrandId.Value)
                .WhereIf(!clsCode.IsNullOrWhiteSpace(), c => c.Category.Code.StartsWith(clsCode))
                .WhereIf(input.Published.HasValue, c => c.Published == input.Published.Value)
                .WhereIf(input.AvailableStart.HasValue, c => c.AvailableStart >= input.AvailableStart.Value)
                .WhereIf(input.AvailableEnd.HasValue, c => c.AvailableEnd < input.AvailableEnd.Value)
                .WhereIf(!input.Keywords.IsNullOrEmpty(), c =>
                    c.Title.Contains(input.Keywords)
                    || c.DescriptionShort.Contains(input.Keywords)
                    || c.Brand.DisplayName.Contains(input.Keywords)
                    || c.Category.DisplayName.Contains(input.Keywords)
                    || c.Sku.Contains(input.Keywords));

            var count = await query.CountAsync();
            var list = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();
            return new PagedResultDto<ItemDto>(count, ObjectMapper.Map<IReadOnlyList<ItemDto>>(list));
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task DeleteAsync(DeleteInput input)
        {
            return repository.DeleteAsync(c => input.Ids.Contains(c.Id));
        }
        /// <summary>
        /// 根据id获取商品信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ItemDto> GetAsync(EntityDto<long> input)
        {
            var entity = await repository.GetAsync(input.Id);
            return ObjectMapper.Map<ItemDto>(entity);
        }
        /// <summary>
        /// 批量发布商品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task PublishAsync(BatchPublishInput input)
        {
            var entities = await repository.GetAllListAsync(d=>input.Ids.Contains(d.Id));
            //如果有问题，就每个明细await吧
            var ts = new HashSet<Task>();
            foreach (var item in entities)
            {
                Task t;
                if (input.AvailableEndSeconds != default)
                    t = itemManager.PublishDurationAsync(item, input.AvailableStart, input.AvailableEndSeconds.Value);
                else
                    t = itemManager.PublishAsync(item, input.AvailableStart, input.AvailableEnd);
                ts.Add(t);
            }
            await Task.WhenAll(ts.ToArray());
        }
    }
}
