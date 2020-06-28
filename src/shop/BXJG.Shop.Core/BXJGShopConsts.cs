using System.ComponentModel;

namespace BXJG.Shop
{
    public class BXJGShopConsts
    {
        public const string LocalizationSourceName = "BXJGShop";

        #region 商品分类
        public const int ItemCategoryIconMaxLength = 200;
        public const int ItemCategoryImage1MaxLength = 200;
        public const int ItemCategoryImage2MaxLength = 200;
        #endregion

        #region settings
        /// <summary>
        /// 订单设置组名
        /// </summary>
        public const string OrderSettingGroupKey = "BXJGOrder";
        /// <summary>
        /// 订单税率设置键
        /// </summary>
        public const string OrderTaxRateSettingKey = "BXJGOrderTaxRate";
        /// <summary>
        /// 订单默认税率
        /// </summary>
        public const float OrderTaxRateSettingDefaultValue = 0.17f;
        #endregion

    }
}
