using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using BXJG.Shop.Catalogue;
using BXJG.Shop.Customer;
using Microsoft.EntityFrameworkCore;
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
        public virtual async Task<AddItemOutput> AddItemAsync(AddItemInput input)
        {
            //正常情况下，应该考虑更多业务判断，目前没有细想

            if (input.SkuId == 0)
                input.SkuId = null;
            if (input.Quantity == 0)
                input.Quantity = 1;

            var entity = await GetShoppingCart();

            ShoppingCartItemEntity item;
            if (input.SkuId.HasValue)
            {
                var product = await productRepository.GetAllIncluding(c => c.Skus).Where(c => c.Id == input.ProductId).SingleAsync();
                item = new ShoppingCartItemEntity(entity, product, product.Skus.Single(c => c.Id == input.SkuId.Value), input.Quantity);
            }
            else
            {
                var product = await productRepository.GetAsync(input.ProductId);
                item = new ShoppingCartItemEntity(entity, product, quantity: input.Quantity);
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
        public virtual async Task<ChangeItemQuantityOutput> ChangeItemQuantityAsync(ChangeItemQuantityInput input)
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
        public virtual async Task<ClearOutput> RemoveAllAsync(ClearInput input)
        {
            var entity = await GetShoppingCart();
            entity.ClearItems();
            return new ClearOutput();
        }
        /// <summary>
        /// 从购物车中移除明细
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<RemoveItemOutput> RemoveItemAsync(RemoveItemInput input)
        {
            var entity = await GetShoppingCart();
            entity.RemoveItem(input.Ids);
            return new RemoveItemOutput();
        }
        /// <summary>
        /// 顾客获取自己的购物车
        /// <br />若是刚刚登陆时获取购物车，需要提供本地购物车数据，服务端将尝试将本地购物车和服务端的购物车合并后返回，否则只返回服务端购物车数据
        /// </summary>
        /// <param name="input">客户端本地购物车信息</param>
        /// <returns></returns>
        public virtual async Task<GetOutput> MergeAndGetAsync(GetInput input)
        {
            //正常情况下，合并购物车应该考虑更多业务判断，目前没有细想
            //合并操作可以考虑封装到领域服务中

            var entity = await GetShoppingCart();

            foreach (var item in input.Items)
            {
                ShoppingCartItemEntity shoppingCartItem;
                if (item.SkuId.HasValue)
                {
                    var product = await productRepository.GetAllIncluding(c => c.Skus).Where(c => c.Id == item.ProductId).SingleAsync();
                    shoppingCartItem = new ShoppingCartItemEntity(entity, product, product.Skus.Single(c => c.Id == item.SkuId.Value), item.Quantity);
                }
                else
                {
                    var product = await productRepository.GetAsync(item.ProductId);
                    shoppingCartItem = new ShoppingCartItemEntity(entity, product, quantity: item.Quantity);
                }

                foreach (var i in item.ExtensionData)
                {
                    shoppingCartItem.SetData(i.Key, i.Value);
                }

                entity.AddItem(shoppingCartItem);//若已存在这个明细，会自动累加数量
            }

            foreach (var i in input.ExtensionData)
            {
                entity.SetData(i.Key, i.Value);
            }
            if (input.Items.Count > 0)
                await CurrentUnitOfWork.SaveChangesAsync();
            return ObjectMapper.Map<GetOutput>(entity);
        }

        /// <summary>
        /// 获取当前顾客的购物车
        /// <br />包含关联的顾客和明细(包含明细关联的商品、sku)
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<ShoppingCartEntity> GetShoppingCart()
        {
            var customerId = await GetCurrentCustomerIdAsync();
            var shoppingCart = await shoppingCartRepository.GetAll()
                                                           .Include(c => c.Items).ThenInclude(c => c.Product.Brand)
                                                           .Include(c => c.Items).ThenInclude(c => c.Product.Unit)
                                                           .Include(c => c.Items).ThenInclude(c => c.Sku)
                                                           .Where(c => c.CustomerId == customerId)
                                                           .SingleAsync();
            shoppingCart.RegisterValueChangedEvent();
            return shoppingCart;
        }
    }
}
