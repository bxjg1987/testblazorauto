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

namespace BXJG.Shop.Customer
{
    //简单起见先直接用具体类，不提炼接口。获取当前用户关联的顾客id场景单一，变化的可能性不大，所以可能也没必要定义接口

    /// <summary>
    /// 包装IAbpSession以提供当前顾客Id的获取
    /// 一次请求一个实例
    /// </summary>
    public class BXJGShopCustomerSession<TUser,TArea> : IPerWebRequestDependency
        where TUser : AbpUserBase
        where TArea : GeneralTreeEntity<TArea>, IAdministrative
    {
        private readonly IAbpSession abpSession;
        private readonly IRepository<CustomerEntity<TUser,TArea>, long> repository;
        ///// <summary>
        ///// 获取顾客id，
        ///// </summary>
        //public readonly Lazy<long> CustomerId;
        private long customerId;

        public BXJGShopCustomerSession(IAbpSession abpSession, IRepository<CustomerEntity<TUser, TArea>, long> repository)
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
        public async Task<long> GetCurrentCustomerIdAsync()
        {
            //反正当前类是一个请求一个实例，所以不用考虑线程同步
            //将来考虑使用缓存来减小数据的查询次数

            if (customerId == 0)
                customerId = await repository.GetAll().Where(c => c.UserId == abpSession.UserId).Select(c => c.Id).SingleAsync();

            return customerId;
        }
    }
}
