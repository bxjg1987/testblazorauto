using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.Events.Bus;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using BXJG.Common;
using BXJG.GeneralTree;
using BXJG.Shop.Sale;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.Customer
{
    /// <summary>
    /// 顾客领域服务
    /// </summary>
    public class CustomerManager : DomainServiceBase
    {
        protected readonly IRepository<CustomerEntity, long> repository;

        //protected readonly IAbpSession session;//领域层 不应该访问Session

        public CustomerManager(IRepository<CustomerEntity, long> repository)
        {
            this.repository = repository;
        }


    

        //直接在仓储中提供了扩展方法
        ///// <summary>
        ///// 根据用户Id获取关联的顾客实体
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //public Task<CustomerEntity> GetByUserIdAsync(long userId)
        //{
        //    return repository.SingleByUserIdAsync(userId);
        //}
        ///// <summary>
        ///// 根据用户Id获取关联的顾客Id
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //public async ValueTask<long> GetCustomerIdByUserIdAsync(long userId)
        //{
        //    return await repository.GetAll().Where(c => c.UserId == userId).Select(c => c.Id).SingleAsync();
        //}
    }
}
