using Abp.Domain.Repositories;
using BXJG.Shop.Customer;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop
{
    public class BXJGShopRepository : EfRepositoryBase, IRepository<CustomerEntity, long>
    {
    }
}
