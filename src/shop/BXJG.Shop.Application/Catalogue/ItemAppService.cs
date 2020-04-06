using Abp.Application.Services;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using BXJG.GeneralTree;
using BXJG.Shop.Catalogue.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.Catalogue
{
    /// <summary>
    /// 上架信息应用服务
    /// </summary>
    /// <typeparam name="TTenant"></typeparam>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TRole"></typeparam>
    /// <typeparam name="TTenantManager"></typeparam>
    /// <typeparam name="TUserManager"></typeparam>
    public class ItemAppService<TTenant, TUser, TRole, TTenantManager, TUserManager>
        : BXJGShopAppServiceBase<TTenant, TUser, TRole, TTenantManager, TUserManager>, IItemAppService
        where TUser : AbpUser<TUser>
        where TRole : AbpRole<TUser>, new()
        where TTenant : AbpTenant<TUser>
        where TTenantManager : AbpTenantManager<TTenant, TUser>
        where TUserManager : AbpUserManager<TRole, TUser>
    {
        private readonly IRepository<ItemEntity, long> repository;

        public ItemAppService(IRepository<ItemEntity, long> repository)
        {
            this.repository = repository;
        }

        public async Task<ItemDto> CreateAsync(ItemCreateDto input)
        {
            var entity = base.ObjectMapper.Map<ItemEntity>(input);
            entity = await repository.InsertAsync(entity);
            return ObjectMapper.Map<ItemDto>(entity);
        }

        public Task<ItemDto> UpdateAsync(ItemUpdateDto input)
        {
            throw new NotImplementedException();
        }
    }
}
