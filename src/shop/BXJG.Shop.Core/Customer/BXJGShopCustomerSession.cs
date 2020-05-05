using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BXJG.Shop.Customer
{
    /// <summary>
    /// 
    /// </summary>
    public class BXJGShopCustomerSession<TUser> : ITransientDependency
        where TUser : AbpUserBase
    {
        //后面考虑用缓存

        public readonly Lazy<long> CustomerId;

        //private readonly IAbpSession abpSession;
        //private readonly IRepository<CustomerEntity<TUser>, long> repository;

        public BXJGShopCustomerSession(IAbpSession abpSession, IRepository<CustomerEntity<TUser>, long> repository)
        {
            //this.abpSession = abpSession;
            //this.repository = repository;
            CustomerId = new Lazy<long>(() =>
            {
               return repository.GetAll().Where(c => c.UserId == abpSession.UserId).Select(c => c.Id).Single();
            });
        }
    }
}
