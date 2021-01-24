using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using BXJG.Shop.Catalogue;
using BXJG.Shop.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.ShoppingCart.Customer
{
    /// <summary>
    /// 顾客端针对自己的购物车的应用服务
    /// </summary>
    public class CustomerShoppingCartAppService : CustomerAppServiceBase, ICustomerShoppingCartAppService
    {
        protected readonly IRepository<ShoppingCartEntity, long> shoppingCartRepository;
        protected readonly IRepository<ProductEntity, long> productRepository;
        protected readonly ShoppingCartManager shoppingCartManager;

        public CustomerShoppingCartAppService(ICustomerSession customerSession, IRepository<ShoppingCartEntity, long> shoppingCartRepository, IRepository<ProductEntity, long> productRepository, ShoppingCartManager shoppingCartManager) : base(customerSession)
        {
            this.shoppingCartRepository = shoppingCartRepository;
            this.productRepository = productRepository;
            this.shoppingCartManager = shoppingCartManager;
        }

        public virtual async Task<AddItemOutput> AddItem(AddItemInput input)
        {
            var entity = await GetShoppingCart();
            var product = await AsyncQueryableExecuter.FirstOrDefaultAsync(productRepository.GetAllIncluding(c => c.Skus).Where(c => c.Id == input.ProductId));
            entity.AddItem(new ShoppingCartItemEntity(entity, product, product.Skus.SingleOrDefault(c => c.Id == input.SkuId), input.Quantity));
            return new AddItemOutput();
        }

        public async Task<ChangeItemQuantityOutput> ChangeItemQuantity(ChangeItemQuantityInput input)
        {
            var entity = await GetShoppingCart();
            entity.Items.Single(c => c.Id == input.Id).Quantity += input.Quantity;
            return new ChangeItemQuantityOutput();
        }

        public async Task<ClearOutput> Clear(ClearInput input)
        {
            var entity = await GetShoppingCart();
            entity.ClearItems();
            return new ClearOutput();
        }

        public Task<PagedResultDto<GetOutput>> Get(GetInput input)
        {
            throw new NotImplementedException();
        }

        public async Task<RemoveItemOutput> RemoveItem(RemoveItemInput input)
        {
            var entity = await GetShoppingCart();
            entity.RemoveItem(input.Ids);
            return new RemoveItemOutput();
        }

        /// <summary>
        /// 获取当前顾客的购物车
        /// <br />包含关联的顾客和明细(包含明细关联的商品、sku)
        /// </summary>
        /// <returns></returns>
        private async Task<ShoppingCartEntity> GetShoppingCart()
        {
            /*
             * 为了不在应用层依赖ef，这里使用多次查询方式。多次查询性能也并不一定比关联查询差，看系统架构
             * 也可以考虑将此操作放在自定义的仓储中来实现
             */

            var customerId = await base.GetCurrentCustomerIdAsync();
            return await shoppingCartManager.GetShoppingCartAsync(customerId);

            //var entity = await AsyncQueryableExecuter.FirstOrDefaultAsync(shoppingCartRepository.GetAllIncluding(c => c.Items, c => c.Customer).Where(c => c.CustomerId == customerId));
            //var productIds = entity.Items.Select(c => c.ProductId);
            ////var skuIds = entity.Items.Select(c => c.SkuId); //sku不是聚合根，没有仓储
            ///*var productWithSkus = */
            //await AsyncQueryableExecuter.ToListAsync(productRepository.GetAllIncluding(c => c.Skus).Where(c => productIds.Contains(c.Id)));
            ////ef查询后默认会建立关联关系，若换其它仓储实现，可以考虑这里重组关联关系，由于某些属性在领域实体是私有的，应该重新new
            //return entity;



        }
    }
}
