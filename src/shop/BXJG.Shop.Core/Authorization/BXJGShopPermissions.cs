using Abp.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Authorization
{
    public class BXJGShopPermissions
    {
        public const string BXJGShop = "BXJGShop";

        public const string BXJGShopCatalogueItemDynamicField = "BXJGShopCatalogueItemDynamicField";
        public const string BXJGShopCatalogueItemDynamicFieldCreate = "BXJGShopCatalogueItemDynamicFieldCreate";
        public const string BXJGShopCatalogueItemDynamicFieldUpdate = "BXJGShopCatalogueItemDynamicFieldUpdate";
        public const string BXJGShopCatalogueItemDynamicFieldDelete = "BXJGShopCatalogueItemDynamicFieldDelete";
    }
}
