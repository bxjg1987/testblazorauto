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
using System.Linq;

namespace BXJG.Shop.Customer
{
    /// <summary>
    /// 为顾客仓储接口定义一些扩展，简化CustomerManager和CustomerAppService的处理
    /// 注意这些方法没有使用AsNoChangeTracking，若你的查询只是返回给调用方，建议编写自己的AsNoChangeTracking查询以提高性能
    /// </summary>
    public static class ICustomerRepositoryExtensions
    {
        /// <summary>
        /// 根据用户id获取顾客实体
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Task<CustomerEntity> SingleByUserIdAsync(this IRepository<CustomerEntity, long> repository, long userId)
        {
            return repository.GetAllIncluding(c=>c.Area).SingleAsync(c => c.UserId == userId);
        }
        /// <summary>
        /// 根据用户id获取顾客实体
        /// </summary>
        /// <typeparam name="TUser"></typeparam>
        /// <param name="repository"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Task<CustomerEntity> SingleOrDefaultByUserIdAsync<TUser>(this IRepository<CustomerEntity, long> repository, long userId)
        {
            return repository.GetAllIncluding(c => c.Area).SingleOrDefaultAsync(c => c.UserId == userId);
        }
        /// <summary>
        /// 根据用户Id获取关联的顾客Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static async ValueTask<long> GetCustomerIdByUserIdAsync(this IRepository<CustomerEntity, long> repository, long userId)
        {
            return await repository.GetAll().Where(c => c.UserId == userId).Select(c => c.Id).SingleAsync();
        }
    }
}
