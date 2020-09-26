using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using BXJG.GeneralTree;
using BXJG.Shop.Common;
using BXJG.Common;

namespace BXJG.Shop.Customer
{
    /// <summary>
    /// 包装IAbpSession以提供当前顾客Id的获取
    /// 一次请求一个实例，请查看BXJGShopCoreModule中的注册逻辑
    /// </summary>
    public class CustomerSession : ICustomerSession//, IPerWebRequestDependency
    {
        private readonly IAbpSession abpSession;
        private readonly IRepository<CustomerEntity, long> repository;
        ///// <summary>
        ///// 获取顾客id，
        ///// </summary>
        //public readonly Lazy<long> CustomerId;
        private long customerId;

        public CustomerSession(IAbpSession abpSession, IRepository<CustomerEntity, long> repository)
        {
            this.abpSession = abpSession;
            this.repository = repository;

            //好像不太容易实现异步，所以用方法来实现
            //CustomerId = new Lazy<long>(() =>
            //{   
            //    //将来考虑用缓存
            //    return repository.GetAll().Where(c => c.UserId == abpSession.UserId).Select(c => c.Id).Single();
            //});
        }

        /// <summary>
        /// 获取当前登录的顾客的Id
        /// </summary>
        /// <returns></returns>
        public async ValueTask<long> GetCurrentCustomerIdAsync()
        {
            //反正当前类是一个请求一个实例，所以不用考虑线程同步
            //将来考虑使用缓存来减小数据的查询次数

            if (customerId == default)
                customerId = await repository.GetAll().Where(c => c.UserId == abpSession.UserId).Select(c => c.Id).SingleAsync();

            return customerId;
        }
    }
}
