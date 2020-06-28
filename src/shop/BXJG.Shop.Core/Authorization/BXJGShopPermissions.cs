using Abp.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Authorization
{
    public class BXJGShopPermissions
    {
        public const string BXJGShop = "BXJGShop";
        //商城专用字典
        public const string BXJGShopDictionary = "BXJGShopDictionary";
        public const string BXJGShopDictionaryCreate = "BXJGShopDictionaryCreate";
        public const string BXJGShopDictionaryUpdate = "BXJGShopDictionaryUpdate";
        public const string BXJGShopDictionaryDelete = "BXJGShopDictionaryDelete";

        //商品分类
        public const string BXJGShopItemCategory = "BXJGShopItemCategory";
        public const string BXJGShopItemCategoryCreate = "BXJGShopItemCategoryCreate";
        public const string BXJGShopItemCategoryUpdate = "BXJGShopItemCategoryUpdate";
        public const string BXJGShopItemCategoryDelete = "BXJGShopItemCategoryDelete";

        //产品信息/上架信息
        public const string BXJGShopItem = "BXJGShopItem";
        public const string BXJGShopItemCreate = "BXJGShopItemCreate";
        public const string BXJGShopItemUpdate = "BXJGShopItemUpdate";
        public const string BXJGShopItemDelete = "BXJGShopItemDelete";
        //订单管理
        public const string BXJGShopOrder = "BXJGShopOrder";
        public const string BXJGShopOrderShip = "BXJGShopOrderShipment";
        public const string BXJGShopOrderRefund = "BXJGShopOrderRefund";
        public const string BXJGShopOrderCancel = "BXJGShopOrderCancel";
        //顾客管理
        public const string BXJGShopCustomer = "BXJGShopCustomer";
        public const string BXJGShopCustomerCreate = "BXJGShopCustomerCreate";
        public const string BXJGShopCustomerUpdate = "BXJGShopCustomerUpdate";
        public const string BXJGShopCustomerDelete = "BXJGShopCustomerDelete";
    }
}
