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
        #region 商品档案
        public const int ItemTitleMaxLength = 100;
        public const int ItemSkuMaxLength = 50;
        public const int ItemDescriptionShortMaxLength = 10000;
        public const int ItemImagesMaxLength = 5000;
        #endregion
        #region 订单
        public const int OrderNoMaxLength = 36;//guid长度 32+4个分隔符，将来可能使用其它格式的订单号
        public const int CustomerRemarkMaxLength = 500;
        public const int ConsigneeMaxLength = 20;
        public const int ConsigneePhoneNumberMaxLength = 50;
        public const int ReceivingAddressMaxLength = 200;
        public const int ZipCodeMaxLength = 50;
        public const int LogisticsNumberMaxLength = 50;
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
