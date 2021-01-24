using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.EntityFrameworkCore.Repositories;
using Abp.EntityFrameworkCore;

namespace BXJG.Shop.ShoppingCart
{
    public interface IRepository : IRepository<ShoppingCartEntity, long>
    { 
        //获取购物车及其关联的顾客、明细、及明细关联的商品、sku等
    }
    public class Repository<TDbContext> : EfCoreRepositoryBase<TDbContext, ShoppingCartEntity, long>, IRepository where TDbContext:AbpDbContext
    {
        public Repository(IDbContextProvider<TDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
