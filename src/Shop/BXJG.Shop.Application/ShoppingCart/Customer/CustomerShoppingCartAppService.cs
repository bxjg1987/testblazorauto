using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
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

        //protected readonly ShoppingCartManager shoppingCartManager;

        public CustomerShoppingCartAppService(ICustomerSession customerSession, IRepository<ShoppingCartEntity, long> shoppingCartRepository, IRepository<ProductEntity, long> productRepository/*, ShoppingCartManager shoppingCartManager*/) : base(customerSession)
        {
            this.shoppingCartRepository = shoppingCartRepository;
            this.productRepository = productRepository;
            //this.shoppingCartManager = shoppingCartManager;
        }
        /// <summary>
        /// 顾客将商品添加到购物车
        /// </summary>
        /// <param name="input">购物车明细信息（商品和数量）</param>
        /// <returns></returns>
        public virtual async Task<AddItemOutput> AddItem(AddItemInput input)
        {
            if (input.SkuId == 0)
                input.SkuId = null;
            if (input.Quantity == 0)
                input.Quantity = 1;

            var entity = await GetShoppingCart();

            ShoppingCartItemEntity item;
            if (input.SkuId.HasValue)
            {
                var product = await AsyncQueryableExecuter.FirstOrDefaultAsync(productRepository.GetAllIncluding(c => c.Skus).Where(c => c.Id == input.ProductId));
                item = new ShoppingCartItemEntity(entity, input.ProductId, input.SkuId, product: product, sku: product.Skus.Single(c => c.Id == input.SkuId.Value), quantity: input.Quantity);
            }
            else
            {
                var product = await productRepository.GetAsync(input.ProductId);
                item = new ShoppingCartItemEntity(entity, input.ProductId, product: product, quantity: input.Quantity);
            }

            foreach (var i in input.ExtensionData)
            {
                item.SetData(i.Key, i.Value);
            }

            entity.AddItem(item);
            return new AddItemOutput();
        }
        /// <summary>
        /// 调整购物车明细数量
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ChangeItemQuantityOutput> ChangeItemQuantity(ChangeItemQuantityInput input)
        {
            var entity = await GetShoppingCart();
            entity.Items.Single(c => c.Id == input.Id).Quantity += input.Quantity;
            return new ChangeItemQuantityOutput();
        }
        /// <summary>
        /// 清空购物车
        /// <br />对应Clear，使用Remove动态生成的api将匹配http方法delete
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ClearOutput> RemoveAll(ClearInput input)
        {
            var entity = await GetShoppingCart();
            entity.ClearItems();
            return new ClearOutput();
        }
        /// <summary>
        /// 顾客获取自己的购物车
        /// <br />若是刚刚登陆时获取购物车，需要提供本地购物车数据，服务端将尝试将本地购物车和服务端的购物车合并后返回，否则只返回服务端购物车数据
        /// </summary>
        /// <param name="input">客户端本地购物车信息</param>
        /// <returns></returns>
        public Task<PagedResultDto<GetOutput>> Get(GetInput input)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 从购物车中移除明细
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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
            var customerId = await base.GetCurrentCustomerIdAsync();
            //return await shoppingCartManager.GetShoppingCartAsync(customerId);

            //var entity = await AsyncQueryableExecuter.FirstOrDefaultAsync(shoppingCartRepository.GetAllIncluding(c => c.Items, c => c.Customer).Where(c => c.CustomerId == customerId));
            //var productIds = entity.Items.Select(c => c.ProductId);
            ////var skuIds = entity.Items.Select(c => c.SkuId); //sku不是聚合根，没有仓储
            ///*var productWithSkus = */
            //await AsyncQueryableExecuter.ToListAsync(productRepository.GetAllIncluding(c => c.Skus).Where(c => productIds.Contains(c.Id)));
            ////ef查询后默认会建立关联关系，若换其它仓储实现，可以考虑这里重组关联关系，由于某些属性在领域实体是私有的，应该重新new
            //return entity;

            var shoppingCart = await AsyncQueryableExecuter.FirstOrDefaultAsync(shoppingCartRepository.GetAllIncluding(c => c.Items).Where(c => c.CustomerId == customerId));
            shoppingCart.RegisterValueChangedEvent();
            return shoppingCart;
        }
    }
}
