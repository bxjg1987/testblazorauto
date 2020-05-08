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
    public class BXJGShopItemAppService<TTenant, TUser, TRole, TTenantManager, TUserManager>
        : BXJGShopAppServiceBase<TTenant, TUser, TRole, TTenantManager, TUserManager>, IBXJGShopItemAppService
        where TUser : AbpUser<TUser>
        where TRole : AbpRole<TUser>, new()
        where TTenant : AbpTenant<TUser>
        where TTenantManager : AbpTenantManager<TTenant, TUser>
        where TUserManager : AbpUserManager<TRole, TUser>
    {
        private readonly IRepository<ItemEntity, long> repository;


        public BXJGShopItemAppService(IRepository<ItemEntity, long> repository)
        {
            this.repository = repository;
        }

        public async Task<ItemDto> CreateAsync(ItemCreateDto input)
        {
            var entity = base.ObjectMapper.Map<ItemEntity>(input);
            entity = await repository.InsertAsync(entity);
            await repository.EnsurePropertyLoadedAsync(entity, c => c.Category);
            return ObjectMapper.Map<ItemDto>(entity);
        }

        public async Task<ItemDto> UpdateAsync(ItemUpdateDto input)
        {
            var entity = await AsyncQueryableExecuter.FirstOrDefaultAsync(repository.GetAllIncluding(c => c.Category));
            ObjectMapper.Map<ItemUpdateDto, ItemEntity>(input, entity);
            return ObjectMapper.Map<ItemDto>(entity);
        }

        public async Task<IList<ItemDto>> GetAllAsync(GetAllItemsInput input)
        {
            var query = repository.GetAllIncluding(c => c.Category)
                .WhereIf(input.BrandId.HasValue, c => c.BrandId == input.BrandId.Value)
                .WhereIf(input.CategoryId.HasValue, c => c.CategoryId == input.CategoryId.Value)
                .WhereIf(input.Published.HasValue, c => c.Published == input.Published.Value)
                .WhereIf(input.AvailableStart.HasValue, c => c.AvailableStart >= input.AvailableStart.Value)
                .WhereIf(input.AvailableEnd.HasValue, c => c.AvailableEnd < input.AvailableEnd.Value)
                .WhereIf(!input.Keywords.IsNullOrEmpty(), c => c.Title.Contains(input.Keywords) || c.Sku.Contains(input.Keywords))
                .OrderBy(input.Sorting)
                .PageBy(input);

            var list = await AsyncQueryableExecuter.ToListAsync(query);
            return ObjectMapper.Map<IList<ItemDto>>(list);
        }

        public Task DeleteAsync(DeleteInput input)
        {
            return repository.DeleteAsync(c => input.Ids.Contains(c.Id));
        }

        public async Task<ItemDto> GetAsync(EntityDto<long> input)
        {
            var entity = await repository.GetAsync(input.Id);
            return ObjectMapper.Map<ItemDto>(entity);
        }
    }
}
