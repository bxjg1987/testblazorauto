using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.ShoppingCart.Customer
{
    /// <summary>
    /// 顾客端针对自己的购物车的应用服务接口
    /// </summary>
    public interface ICustomerShoppingCartAppService : IApplicationService
    {
        /// <summary>
        /// 顾客获取自己的购物车
        /// <br />若是刚刚登陆时获取购物车，需要提供本地购物车数据，服务端将尝试将本地购物车和服务端的购物车合并后返回，否则只返回服务端购物车数据
        /// </summary>
        /// <param name="input">客户端本地购物车信息</param>
        /// <returns></returns>
        Task<GetOutput> Get(GetInput input);
        /// <summary>
        /// 顾客将商品添加到购物车
        /// </summary>
        /// <param name="input">购物车明细信息（商品和数量）</param>
        /// <returns></returns>
        Task<AddItemOutput> AddItem(AddItemInput input);
        /// <summary>
        /// 调整购物车明细数量
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ChangeItemQuantityOutput> ChangeItemQuantity(ChangeItemQuantityInput input);
        /// <summary>
        /// 从购物车中移除明细
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<RemoveItemOutput> RemoveItem(RemoveItemInput input);
        /// <summary>
        /// 清空购物车
        /// <br />对应Clear，使用Remove动态生成的api将匹配http方法delete
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ClearOutput> RemoveAll(ClearInput input);
    }
}
