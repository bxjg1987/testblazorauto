using Abp.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Authorization
{
    public class PermissionNames
    {
        public const string BXJGShop = "BXJGShop";
        ////商城专用字典
        //public const string BXJGShopDictionary = "BXJGShopDictionary";
        //public const string BXJGShopDictionaryCreate = "BXJGShopDictionaryCreate";
        //public const string BXJGShopDictionaryUpdate = "BXJGShopDictionaryUpdate";
        //public const string BXJGShopDictionaryDelete = "BXJGShopDictionaryDelete";

        //商品分类
        public const string ProductCategory = "BXJGShopProductCategory";
        public const string ProductCategoryCreate = "BXJGShopProductCategoryCreate";
        public const string ProductCategoryUpdate = "BXJGShopProductCategoryUpdate";
        public const string ProductCategoryDelete = "BXJGShopProductCategoryDelete";

        //产品信息/上架信息
        public const string Product = "BXJGShopProduct";
        public const string ProductCreate = "BXJGShopProductCreate";
        public const string ProductUpdate = "BXJGShopProductUpdate";
        public const string ProductDelete = "BXJGShopProductDelete";
        //订单管理
        public const string Order = "BXJGShopOrder";
        public const string OrderShip = "BXJGShopOrderShipment";
        public const string OrderRefund = "BXJGShopOrderRefund";
        public const string OrderCancel = "BXJGShopOrderCancel";
        //顾客管理
        public const string Customer = "BXJGShopCustomer";
        public const string CustomerCreate = "BXJGShopCustomerCreate";
        public const string CustomerUpdate = "BXJGShopCustomerUpdate";
        public const string CustomerDelete = "BXJGShopCustomerDelete";
    }
}
