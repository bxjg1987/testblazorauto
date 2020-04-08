using Abp.Application.Services;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Linq;
using Abp.MultiTenancy;
using BXJG.GeneralTree;
using BXJG.Shop.Catalogue.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Linq.Extensions;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic;
using Abp.Extensions;
using BXJG.Shop.Customer.Dto;

namespace BXJG.Shop.Customer
{
    /*
     * 设计此功能时还没有引用Microsoft.EntityFrameworkCore，它包含很多好用的扩展方法，使用的AsyncQueryableExecuter + system.linq.dynamic 的方式
     * 而abp zero是直接引用的它
     * 可以考虑直接引用Microsoft.EntityFrameworkCore
     */

    /// <summary>
    /// 商城会员（顾客）信息
    /// </summary>
    /// <typeparam name="TTenant"></typeparam>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TRole"></typeparam>
    /// <typeparam name="TTenantManager"></typeparam>
    /// <typeparam name="TUserManager"></typeparam>
    public class CustomerAppService<TTenant, TUser, TRole, TTenantManager, TUserManager>
        : BXJGShopAppServiceBase<TTenant, TUser, TRole, TTenantManager, TUserManager>, ICustomerAppService
        where TUser : AbpUser<TUser>
        where TRole : AbpRole<TUser>, new()
        where TTenant : AbpTenant<TUser>
        where TTenantManager : AbpTenantManager<TTenant, TUser>
        where TUserManager : AbpUserManager<TRole, TUser>
    {
        private readonly IRepository<CustomerEntity<TUser>, long> repository;

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }//属性注入

        public CustomerAppService(IRepository<CustomerEntity<TUser>, long> repository)
        {
            this.repository = repository;
            this.AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        }

        //public async Task<ItemDto> CreateAsync(ItemCreateDto input)
        //{
        //    var entity = base.ObjectMapper.Map<CustomerEntity<TUser>>(input);
        //    entity = await repository.InsertAsync(entity);
        //    await repository.EnsurePropertyLoadedAsync(entity, c => c.Category);
        //    return ObjectMapper.Map<ItemDto>(entity);
        //}

        public async Task<CustomerDto> UpdateAsync(CustomerUpdateDto input)
        {
            //var entity = await AsyncQueryableExecuter.FirstOrDefaultAsync(repository.GetAllIncluding(c => c.Category));
            //ObjectMapper.Map<ItemUpdateDto, CustomerEntity<TUser>>(input, entity);
            //return ObjectMapper.Map<ItemDto>(entity);
            return null;
        }

        public async Task<IList<CustomerDto>> GetListAsync(GetAllCustomersInput input)
        {
            var query = repository.GetAllIncluding(c => c.User)
                .WhereIf(!input.Keywords.IsNullOrEmpty(), c => c.User.Name.Contains(input.Keywords) || c.User.PhoneNumber.Contains(input.Keywords))
                .OrderBy(input.Sorting)
                .PageBy(input);

            var list = await AsyncQueryableExecuter.ToListAsync(query);
            return ObjectMapper.Map<IList<CustomerDto>>(list);

        }


        //public Task DeleteAsync(params long[] ids)
        //{
        //  return  repository.DeleteAsync(c => ids.Contains(c.Id));
        //}
    }
}
