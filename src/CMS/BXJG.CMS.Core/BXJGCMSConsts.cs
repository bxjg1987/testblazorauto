namespace BXJG.CMS
{
    public class BXJGCMSConsts
    {
        public const string LocalizationSourceName = "BXJGCMS";

        #region MyRegion
        //由于ColumnEntity是泛型，将验证定义在这里
        public const int ColumnTitleMaxLength = 50;
        public const int ColumnIconMaxLength = 200;
        public const int ColumnSeoTitleMaxLength = 2000;
        public const int ColumnSeoDescriptionMaxLength = 5000;
        public const int ColumnSeoKeywordMaxLength = 1000;
        public const int ColumnListTemplateMaxLength = 200;
        public const int ColumnDetailTemplateMaxLength = 200;
        #endregion

        #region Article
        public const int ArticleTitleMaxLength = 500;
        public const int ArticleSeoTitleMaxLength = 2000;
        public const int ArticleSeoDescriptionMaxLength = 5000;
        public const int ArticleSeoKeywordMaxLength = 1000;
        public const int ArticleSummaryMaxLength = 5000;
        #endregion

        #region settings
        ///// <summary>
        ///// 订单设置组名
        ///// </summary>
        //public const string OrderSettingGroupKey = "BXJGOrder";
        ///// <summary>
        ///// 订单税率设置键
        ///// </summary>
        //public const string OrderTaxRateSettingKey = "BXJGOrderTaxRate";
        ///// <summary>
        ///// 订单默认税率
        ///// </summary>
        //public const float OrderTaxRateSettingDefaultValue = 0.17f;
        #endregion

    }
}
