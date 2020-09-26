using System.ComponentModel;

namespace BXJG.Shop
{
    public class BXJGShopConsts
    {
        public const string LocalizationSourceName = "BXJGShop";

        /// <summary>
        /// 目前考虑所有商城用户属于静态角色，静态角色在BXJGShopCoreModule中配置
        /// ef初始化或顾客注册时设置此角色
        /// </summary>
        public const string CustomerRoleName = "Customer";
        /// <summary>
        /// 顾客登陆时会将顾客id存储到claim中，此值就是claim类型
        /// </summary>
        public const string CustomerIdClaim = "CustomerId";


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
        public const int ItemSpecificationMaxLength = 500;
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
        /// 商城中使用的数据字典是由外部提供，数据迁移后生成的id可能不同，迁移时会将值存储到settings中
        /// 因为abp提供了js库，可以在前端访问settings，这样数据迁移时生成的字典的id就能给到前端访问，
        /// 这里定义一个组，专门存放数据字典迁移后生成的id 的对应的设置项
        /// </summary>
        public const string DataDictionayMigrationValueSettingGroupKey = "DataDictionayMigrationValueSettingGroupKey";
        public const string DataDictionayMigrationValuepinpai= "DataDictionayMigrationValuepinpai";
        public const string DataDictionayMigrationValuezhifufangshi = "DataDictionayMigrationValuezhifufangshi";
        public const string DataDictionayMigrationValuepeisongfangshi = "DataDictionayMigrationValuepeisongfangshi";
        public const string DataDictionayMigrationValuedanwei = "DataDictionayMigrationValuedanwei";

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
