using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BXJG.GeneralTree;
using BXJG.Shop.Common;
using BXJG.Common;

namespace BXJG.Shop.Customer
{
    /*
     * Repository该不该持有session？
     * 
     */

    /// <summary>
    /// 为顾客仓储接口定义一些扩展，简化CustomerManager和CustomerAppService的处理
    /// 注意这些方法没有使用AsNoChangeTracking，若你的查询只是返回给调用方，建议编写自己的AsNoChangeTracking查询以提高性能
    /// </summary>
    public static class ICustomerRepositoryExtensions
    {
        public static Task<CustomerEntity<TUser>> SingleByUserIdWithUserAsync<TUser >(this IRepository<CustomerEntity<TUser >, long> repository, long userId)
            where TUser : AbpUserBase
        {
            return repository.GetAllIncluding(c => c.User).SingleAsync(c => c.UserId == userId);
        }
        public static Task<CustomerEntity<TUser >> SingleByUserIdWithoutUserAsync<TUser >(this IRepository<CustomerEntity<TUser >, long> repository, long userId)
            where TUser : AbpUserBase
        {
            return repository.SingleAsync(c => c.UserId == userId);
        }
        public static Task<CustomerEntity<TUser >> SingleOrDefaultByUserIdWithUserAsync<TUser >(this IRepository<CustomerEntity<TUser >, long> repository, long userId)
            where TUser : AbpUserBase
        {
            return repository.GetAllIncluding(c => c.User).SingleOrDefaultAsync(c => c.UserId == userId);
        }
        public static Task<CustomerEntity<TUser >> SingleOrDefaultByUserIdWithoutUserAsync<TUser >(this IRepository<CustomerEntity<TUser >, long> repository, long userId)
            where TUser : AbpUserBase
        {
            return repository.GetAll().SingleOrDefaultAsync(c => c.UserId == userId);
        }
    }
}
