using Abp.Authorization.Users;
using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Customer
{
    public class CustomerManager<TUser> : BXJGShopDomainServiceBase
        where TUser:AbpUserBase
    {
    }
}
